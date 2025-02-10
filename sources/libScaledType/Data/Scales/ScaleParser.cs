using As.Tools.Data.Parsers;
using As.Tools.Data.Scanners;

namespace As.Tools.Data.Scales
{
    internal class ScaleParser(IScanner scanner) : Parser(scanner)
    {
        public static bool TryParse(string value, out Scale scale)
        {
            var result = false;
            try
            {
                IScanner scanner = new ScaleScanner(value);
                var parser = new ScaleParser(scanner);

                parser.Execute();

                result = parser.ParseOK && (parser.Result is not null);
                scale = parser.Result ?? [];
            }
#if USE_LOG4NET
            catch (Exception x)
            {
                log.Debug($"ScaleParser: problem while parsing: '{x}'");
#else
            catch (Exception)
            {
#endif
                result = false;
                scale = [];
            }
            return result;
        }

        public Scale? Result { get; private set; }

        #region grammar v1.0
        #region preamble
        // %start scale
        //
        // %union {
        //     Scale      u1;
        //     ScaledUnit u2;
        // }
        //
        // %type <Scale> scale
        // %type <Scale> groups
        // %type <Scale> segment
        // %type <Scale> part
        // %type <ScaledUnit> unit
        //
        // %token NAME INT
        // %token BRACKET_OPEN BRACKET_CLOSE // '[' ']'
        // %token BRACE_OPEN BRACE_CLOSE     // '(' ')'
        // %token PWR DIV                    // '^' '/'
        //
        // %{
        // %}
        #endregion

        // %%

        #region grammar
        // scale: '[' groups ']'
        // { $$ = $2; };

        // groups: segment groups?
        // { $$ = ($2 == null) ? $1 : $1.Append($2) };

        // segment: part
        // { $$ = $1 };

        // segment: 1 '/' part
        // { $$ = new Scale( new ScaledUnit($3, -1) ); };

        // segment: part '/' part
        // { $$ = $1.Append( new ScaledUnit($3, -1); };

        // part: unit
        // { $$ = $1; };

        // part: '(' groups ')'
        // { $$ = $2; };

        // unit: NAME
        // { $$ = new ScaledUnit($1) }

        // unit: NAME '^' INT
        // { $$ = new ScaledUnit($1, $3) }
        #endregion

        // %%

        #region Postamble
        // empty.
        #endregion
        #endregion

        #pragma warning disable IDE1006 // Naming Styles
        #region preamble
        //
        // %start scale
        //
        // ...
        protected override void ParseGrammar()
        {
            Scanner.SetState(ScaleScanner.States.NORMAL);

            Result = scale();
        }
        #endregion

        // %%

        #region grammar
        // scale: '[' groups ']'
        // { $$ = $2; };

        Scale scale()
        {
            var t = PeekToken();
            if (!t.Id.EQ(TokenId.BRACKET_OPEN)) throw new ParserException(t, "scale ::= .'[' groups ']'");
            GetToken();

            var ss = groups();

            t = PeekToken();
            if (!t.Id.EQ(TokenId.BRACKET_CLOSE)) throw new ParserException(t, "scale ::= '[' groups . ']'");
            GetToken();

            return ss;
        }

        // groups: segment groups?
        // { $$ = ($2 == null) ? $1 : $1.Append($2) };

        Scale groups()
        {
            var ss = segment();

            var t = PeekToken();
            while (is_groups_starter(ref t))
            {
                ss.Append(segment());
                t = PeekToken();
            }

            return ss;
        }

        static bool is_groups_starter(ref Token t)
        {
            return is_segment_starter(ref t);
        }

        // segment: part
        // { $$ = $1 };

        // segment: 1 '/' part
        // { $$ = new Scale( new ScaledUnit($3, -1) ); };

        // segment: part '/' part
        // { $$ = $1.Append( new ScaledUnit($3, -1); };

        Scale segment()
        {
            Scale ss;

            var s3_mandatory = false;

            var t = PeekToken();
            if (t.Id.EQ(TokenId.INT))
            {
                GetToken();

                if (!int.TryParse(t.Value, out int one) || (one != 1)) throw new ParserException(t, "segment ::= . 1 '/' '(' groups ')'");

                ss = new Scale( new ScaledUnit(Unit.c, 1));
                s3_mandatory = true;
            }
            else
            {
                ss = part();
            }

            t = PeekToken();
            if (t.Id.EQ(TokenId.DIV))
            {
                GetToken();
                ss.Append(part(), reciproce: true);
            }
            else if (s3_mandatory) throw new ParserException(t, "segment ::= 1 . '/' '(' groups ')'");

            return ss;
        }

        static bool is_segment_starter(ref Token t)
        {
            return
                t.Id.EQ(TokenId.INT) |
                is_part_starter(ref t);
        }

        // part: unit
        // { $$ = $1; };

        // part: '(' groups ')'
        // { $$ = $2; };

        Scale part()
        {
            var t = PeekToken();
            if (t.Id.EQ(TokenId.PARENTESES_OPEN))
            {
                GetToken();

                var ss = groups();

                t = PeekToken();
                if (!t.Id.EQ(TokenId.PARENTESES_CLOSE)) throw new ParserException(t, "part ::= '(' groups . ')'");
                GetToken();

                return ss;
            }
            else
            {
                return new Scale(unit());
            }
        }

        static bool is_part_starter(ref Token t)
        {
            return
                t.Id.EQ(TokenId.PARENTESES_OPEN) ||
                is_unit_starter(ref t);
        }

        // unit: NAME
        // { $$ = new ScaledUnit($1) }

        // unit: NAME '^' INT
        // { $$ = new ScaledUnit($1, $3) }

        ScaledUnit unit()
        {
            var t = PeekToken();
            if (!t.Id.EQ(TokenId.NAME)) throw new ParserException(t, "unit ::= . NAME ( '^' INT )?");
            GetToken();

            if (!ScaledUnit.TryParse(t.Value, out Unit s1)) throw new ParserException(t, $"unit ::= . '{t.Value}' ( '^' INT )? ; NAME '{t.Value}' not recognised.");

            int s3 = 1;
            t = PeekToken();
            if (t.Id.EQ(TokenId.PWR))
            {
                GetToken();

                t = PeekToken();
                if (!t.Id.EQ(TokenId.INT)) throw new ParserException(t, "unit ::= NAME '^' . INT");
                GetToken();

                if (t.Value is not null) s3 = int.Parse(t.Value);
                else throw new ParserException(t, "unit ::= NAME '^' . INT");
            }

            return new ScaledUnit(s1, s3);
        }

        static bool is_unit_starter(ref Token t)
        {
            return t.Id.EQ(TokenId.NAME);
        }
        #endregion

        // %%

#pragma warning restore IDE1006 // Naming Styles
    }
}
