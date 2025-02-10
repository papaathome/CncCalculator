using System.Text;

using As.Tools.Collections;

namespace As.Tools.Data.Scales
{
    /// <summary>
    /// A scale is a list of scaled units.
    /// </summary>
    /// <remarks>
    /// The Scale class (and ScaledUnit class) integrates the use of Units in ScaledType.
    /// </remarks>
    public class Scale : ObservableCollectionSuspend<ScaledUnit>, IScale
    {
        public const string EMPTY = "[#]";

        #region Static actions
        /// <summary>
        /// TryParse: parse a readable form of a scale
        /// </summary>
        /// <param name="value">Readable representation of a scale</param>
        /// <param name="scale">Reference to a Scale variable</param>
        /// <returns>True if the parse was successfull and an instance is available in scale, false otherwise.</returns>
        public static bool TryParse(string value, out Scale scale)
        {
            return ScaleParser.TryParse(value, out scale);
        }

        /// <summary>
        /// Normalise: remover redundant or simplify scale factors.
        /// </summary>
        /// <remarks>
        /// (Syntactical) same ScaledUnit factors are combined into one factor, e.g.: [m][m] becomes [m^2]
        /// ScaledUnit factors with exponent 0 are changed to constant [#], e.g.: [s][s^-1] = [s, 0] becomes [#]
        /// If the scale has more than one factor and one is a constant [#] then the constant is removed.
        /// e.g.: [#][g] becomes [g]
        /// </remarks>
        /// <param name="value">scale to normalise</param>
        /// <returns>Scale without redundand or multiple scale units.</returns>
        public static Scale Normalise(Scale value)
        {
            var result = new Scale();
            foreach (var s in value)
            {
                if (s.Exp == 0) continue;
                var r = result.FirstOrDefault(t => (t.Unit == s.Unit));
                if (r is null) result.Add(new ScaledUnit(s));
                else if (r.Exp == -s.Exp) result.Remove(r);
                else r.ExpAdd(s.Exp);
            }

            if (1 < result.Count)
            {
                var c = result.FirstOrDefault(t => (t.Unit == Unit.c));
                if (c is not null) result.Remove(c);
            }
            else if (result[0].Unit == Unit.c)
            {
                result[0].ExpReset();
            }
            return result;
        }

        /// <summary>
        /// AreEquals: is the left side scale (syntactical) equal to the right side scale.
        /// </summary>
        /// <param name="left">a scale to compare</param>
        /// <param name="right">a scale to compare</param>
        /// <returns>
        /// true if the normalised units and exponent on the left side are
        /// (syntactical) equal to the normalised ones from the right side
        /// </returns>
        public static bool AreEqual(Scale? left, Scale? right)
        {
            if (ReferenceEquals(left, right)) return true;
            if ((left is null) && (right is null)) return true;
            if ((left is null) || (right is null)) return false;

            var ln = Normalise(left);
            var rn = Normalise(right);

            if (ln.Count != rn.Count) return false;

            foreach (var l in ln)
            {
                var r = rn.FirstOrDefault(t => (t.Unit == l.Unit));
                if ((r is null) || (l.Exp != r.Exp)) return false;
            }
            return true;
        }
        #endregion Static actions

        #region Operators
        /// <summary>
        /// Equals operator ==: is the left side scale (syntactical) equal to the right side scale.
        /// </summary>
        /// <param name="left">a scale to compare</param>
        /// <param name="right">a scale to compare</param>
        /// <returns>
        /// true if the normalised units and exponent on the left side are
        /// (syntactical) equal to the normalised ones from the right side
        /// </returns>
        /// <seealso cref="AreEqual"/>
        public static bool operator ==(Scale? left, Scale? right)
        {
            return AreEqual(left, right);
        }

        /// <summary>
        /// Not equals operator !=: it the left side not equal to the right side (in _value and scale)
        /// </summary>
        /// <param name="left">a scale to compare</param>
        /// <param name="right">a scale to compare</param>
        /// <returns>true if scales are not unequal.</returns>
        /// <seealso cref="AreEqual"/>
        public static bool operator !=(Scale? left, Scale? right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Operator *: append two scales into one.
        /// </summary>
        /// <param name="left">Scale to append to</param>
        /// <param name="right">Scale to append</param>
        /// <returns></returns>
        public static Scale operator *(Scale left, Scale right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            var result = new Scale(left);
            result.Append(right);
            return Normalise(result);
        }

        /// <summary>
        /// Operator /: append two scales into one, the second one in reciproce mode.
        /// </summary>
        /// <param name="left">Scale to append to</param>
        /// <param name="right">Scale to append in reciproce mode</param>
        /// <returns></returns>
        public static Scale operator /(Scale left, Scale right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            var result = new Scale(left);
            result.Append(right, reciproce: true);
            return Normalise(result);
        }
        #endregion Operators

        #region .ctor
        /// <summary>
        /// .ctor: create a new scale with a default scale of [#]
        /// </summary>
        public Scale() : this(Unit.c, 1) {}

        /// <summary>
        /// .ctor: create a new scale for a given unit and exponent.
        /// </summary>
        /// <param name="unit">The scale unit</param>
        public Scale(Unit unit, int exp = 1) : base()
        {
            Add((exp == 0)
                ? new ScaledUnit()
                : new ScaledUnit(unit, exp)
            );
        }

        /// <summary>
        /// .ctor: create a new scale for a given scaled unit.
        /// </summary>
        /// <param name="unit">The scale unit</param>
        public Scale(ScaledUnit unit) : base()
        {
            Add(unit);
        }

        /// <summary>
        /// .ctor: create a new scale and initialse from a list of scaled units.
        /// </summary>
        /// <param name="other"></param>
        public Scale(Scale other) : base()
        {
            // note: base(other) adds references, not clones.
            foreach (var o in other) Add(o.Clone());
        }
        #endregion .ctor

        #region Actions
        public Scale Clone()
        {
            return new Scale(this);
        }

        object ICloneable.Clone()
        {
            return new Scale(this);
        }

        /// <summary>
        /// Append: Multiply (or divide) this Scale with an extra factor
        /// </summary>
        /// <param name="scale">Scale to append to this Scale</param>
        /// <param name="reciproce">true: append [scale^-1] else append [scale]</param>
        public void Append(Scale scale, bool reciproce = false)
        {
            if ((scale is null) || (scale.Count == 0)) return;
            foreach (var s in scale)
            {
                Append(s.Unit, s.Exp, reciproce);
            }
        }

        /// <summary>
        /// Append: Multiply (or divide) this Scale with an extra factor
        /// </summary>
        /// <param name="scale">Scale to append to this Scale</param>
        /// <param name="reciproce">true: append [scale^-1] else append [scale]</param>
        public void Append(ScaledUnit scale, bool reciproce = false)
        {
            Append(scale.Unit, scale.Exp, reciproce);
        }

        /// <summary>
        /// Append: Multiply (or divide) this Scale with an extra factor
        /// </summary>
        /// <param name="scale">ScaledUnit to append to this Scale</param>
        /// <param name="reciproce">true: append [scale^-1] else append [scale]</param>
        public void Append(Unit unit, int exp = 1, bool reciproce = false)
        {
            var s = this.FirstOrDefault(t => (t.Unit == unit));
            if (s is null)
            {
                // factor not yet included.
                if (exp != 0) Add(new ScaledUnit(unit, (reciproce) ? -exp : exp));
            }
            else if (reciproce && (s.Exp == exp) || (!reciproce && (s.Exp == -exp)))
            {
                // new factor cancels out available factor.
                Remove(s);
            }
            else
            {
                // new factor will modify available factor.
                if (exp != 0) s.ExpAdd((reciproce) ? -exp : exp);
            }

            for (
                s = this.FirstOrDefault(t => (t.Unit == Unit.c));
                (s is not null) && (1 < Count);
                s = this.FirstOrDefault(t => (t.Unit == Unit.c))
            )
            {
                Remove(s);
                s = null;
            }
            if (s is not null) s.ExpReset();
            if (Count == 0)
            {
                Add(new ScaledUnit(Unit.c, 1));
            }
        }

        /// <summary>
        /// Normalise with suspended notifications.
        /// </summary>
        public void Normal()
        {
            bool suspended = IsNotificationsSuspended;
            try
            {
                if (!suspended) BeginUpdate();

                var N = Normalise(this);
                if (!N.IsIdentical(this))
                {
                    Clear();
                    foreach (var n in N) Add(n);
                }
            }
            finally
            {
                if (!suspended) EndUpdate();
            }
        }

        /// <summary>
        /// Equals: is this scale (syntactical) equal to the other scale.
        /// </summary>
        /// <param name="obj">a scale to compare</param>
        /// <returns>
        /// true if the normalised units and exponent from this are
        /// (syntactical) equal to the normalised ones from the other one
        /// </returns>
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            return (obj is Scale) && AreEqual(this, obj as Scale);
        }

        /// <summary>
        /// Identical: is this scale an equal bag of the other scale.
        /// 
        /// e.g: [cm cm s^-1] is identical to [s^-1 cm cm] but is not identical to [cm^2/s]
        /// </summary>
        /// <param name="other">other scale to compare with</param>
        /// <returns>true: other have the same scales with the same exponents; false other have at least a difference in one scale</returns>
        public bool IsIdentical(Scale other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (Count != other.Count) return false;

            List<ScaledUnit> a = [];
            foreach (var s in this) a.Add(s);
            foreach (var o in other)
            {
                var s = a.FirstOrDefault(e => e.Unit == o.Unit && e.Exp == o.Exp);
                if (s == null) return false;
                a.Remove(s);
            }
            return (a.Count == 0);
        }

        /// <summary>
        /// GetHashCode: Get the Hash code.
        /// </summary>
        /// <returns>The Hash _value</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// ToString: Readable representation of the Scale, with or without brackets.
        /// </summary>
        /// <param name="with_brackers">Use brackets or not</param>
        /// <returns>Human readable representation of a scale</returns>
        public string ToString(bool with_brackers = true)
        {
            var sb = new StringBuilder();
            if (Count == 0) sb.Append(Unit.c.ToString(append_brackets: false));
            else
            {
                var numerator = new List<ScaledUnit>();
                var denominator = new List<ScaledUnit>();
                foreach (var s in this)
                {
                    if (0 <= s.Exp) numerator.Add(s);
                    else denominator.Add(s);
                }

                switch (numerator.Count)
                {
                    case 0:
                        sb.Append('1');
                        break;
                    case 1:
                        sb.Append(numerator[0].ToString(append_brackets: false, reciproce: false));
                        break;
                    default:
                        if (denominator.Count != 0) sb.Append('(');
                        bool first = true;
                        foreach (var s in numerator)
                        {
                            if (first) first = false;
                            else sb.Append(' ');
                            sb.Append(s.ToString(append_brackets: false, reciproce: false));
                        }
                        if (denominator.Count != 0) sb.Append(')');
                        break;
                }
                switch (denominator.Count)
                {
                    case 0:
                        break;
                    case 1:
                        sb.Append('/');
                        sb.Append(denominator[0].ToString(append_brackets: false, reciproce: true));
                        break;
                    default:
                        sb.Append("/(");
                        bool first = true;
                        foreach (var s in denominator)
                        {
                            if (first) first = false;
                            else sb.Append(' ');
                            sb.Append(s.ToString(append_brackets: false, reciproce: true));
                        }
                        sb.Append(')');
                        break;
                }
            }
            if (with_brackers)
            {
                sb.Insert(0, "[");
                sb.Append(']');
            }
            return sb.ToString();
        }

        /// <summary>
        /// ToString: Readable representation of the Scale, with brackets.
        /// </summary>
        /// <returns>Human readable representation of a scale</returns>
        public override string ToString()
        {
            return ToString(with_brackers: true);
        }
        #endregion Actions
    }
}
