namespace As.Tools.Data.Scales
{
    public enum UnitConstant : ulong
    {
        c     = BaseUnit.Constant | 0,
        tooth = BaseUnit.Constant | 1 << 0,
        //             bbbb  last = 0x1UL << 47
        Mask = 0xffUL
    }

    public static class UnitConstantX
    {
        static readonly Unit BASE = Unit.c;

        static public double Factor(this UnitConstant me)
        {
            switch (me)
            {
                case UnitConstant.c: return 1.0; // constant ([#], 1) = dimentionless constant.
                case UnitConstant.tooth: return 1.0; // constant ([tooth], 1) = named dimentionless constant.
                default: return 0.0;
            }
        }

        static public Unit Base(this UnitConstant me)
        {
            return BASE;
        }

        static public string ToString(
            this UnitConstant me,
            bool append_brackets)
        {
            switch (me)
            {
                case UnitConstant.c: return (append_brackets) ? "[#]" : "#";
                case UnitConstant.tooth: return (append_brackets) ? "[tooth]" : "tooth";
                default:
                    var msg = "(UnitConstant)0x{(ulong)me:x16}";
                    return (append_brackets) ? $"[{msg}]" : msg;
            }
        }

        static public string Description(this UnitConstant me)
        {
            switch (me)
            {
                case UnitConstant.c: return "constant: dimentionless constant, [#]";
                case UnitConstant.tooth: return "tooth: named dimentionless constant, [tooth]";
                default:
                    var msg = $"(UnitConstant)0x{(ulong)me:x16}";
                    return $"Constant: dimentionless constant [{msg}]";
            }
        }

        static public bool TryParse(string value, out Unit unit)
        {
            switch (value.Trim())
            {
                case "#":     unit = Unit.c; return true;
                case "tooth": unit = Unit.tooth; return true;
                default:      unit = BASE; return false;
            }
        }
    }
}
