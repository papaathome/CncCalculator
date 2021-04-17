using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace As.Tools.Data.Scales
{
    /// <summary>
    /// A scale is a list of scaled units.
    /// </summary>
    /// <remarks>
    /// The Scale class (and ScaledUnit class) integrates the use of Units in ScaledType.
    /// </remarks>
    public class Scale : ObservableCollection<ScaledUnit>
    {
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
        /// <param name="target">scale to normalise</param>
        /// <returns>Scale without redundand or multiple scale units.</returns>
        public static Scale Normalise(Scale target)
        {
            if ((object)target == null) return new Scale();

            var result = new Scale();
            foreach (var s in target)
            {
                if (s.Exp == 0) continue;
                var r = result.FirstOrDefault(t => (t.Unit == s.Unit));
                if (r == null) result.Add(new ScaledUnit(s));
                else if (r.Exp == -s.Exp) result.Remove(r);
                else r.ExpAdd(s.Exp);
            }

            var c = result.FirstOrDefault(t => (t.Unit == Unit.c));
            if (c != null) result.Remove(c);
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
        public static bool AreEqual(Scale left, Scale right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (((object)left == null) && ((object)right == null)) return true;
            if (((object)left == null) || ((object)right == null)) return false;

            var ln = Normalise(left);
            var rn = Normalise(right);

            if (ln.Count != rn.Count) return false;

            foreach (var l in ln)
            {
                var r = rn.FirstOrDefault(t => (t.Unit == l.Unit));
                if ((r == null) || (l.Exp != r.Exp)) return false;
            }
            return true;
        }

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
        public static bool operator ==(Scale left, Scale right)
        {
            return AreEqual(left, right);
        }

        /// <summary>
        /// Not equals operator !=: it the left side not equal to the right side (in value and scale)
        /// </summary>
        /// <param name="left">a scale to compare</param>
        /// <param name="right">a scale to compare</param>
        /// <returns>true if scales are not unequal.</returns>
        /// <seealso cref="AreEqual"/>
        public static bool operator !=(Scale left, Scale right)
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
            var result = new Scale(left);
            result.Append(right);
            return result;
        }

        /// <summary>
        /// Operator *: append two scales into one, the second one in reciproce mode.
        /// </summary>
        /// <param name="left">Scale to append to</param>
        /// <param name="right">Scale to append in reciproce mode</param>
        /// <returns></returns>
        public static Scale operator /(Scale left, Scale right)
        {
            var result = new Scale(left);
            result.Append(right, reciproce: true);
            return result;
        }

        /// <summary>
        /// .ctor: create a new scale for a given unit.
        /// </summary>
        /// <param name="unit">The scale unit</param>
        public Scale(ScaledUnit unit) : base()
        {
            Add(unit);
        }

        /// <summary>
        /// .ctor: create a new scale with a default scale of [#]
        /// </summary>
        public Scale()
        {
            Add(new ScaledUnit(Unit.c, 1));
        }

        /// <summary>
        /// .ctor: create a new scale and initialse from a list of scaled units.
        /// </summary>
        /// <param name="collection"></param>
        public Scale(IEnumerable<ScaledUnit> collection) : base(collection)
        { }

        /// <summary>
        /// Append: Multiply (or divide) this Scale with an extra factor
        /// </summary>
        /// <param name="scale">Scale to append to this Scale</param>
        /// <param name="reciproce">true: append [scale^-1] else append [scale]</param>
        public void Append(Scale scale, bool reciproce = false)
        {
            if (((object)scale == null) || (scale.Count == 0)) return;
            foreach (var s in scale)
            {
                Append(s.Unit, (reciproce) ? -s.Exp : s.Exp);
            }
        }

        /// <summary>
        /// Append: Multiply (or divide) this Scale with an extra factor
        /// </summary>
        /// <param name="scale">Scale to append to this Scale</param>
        /// <param name="reciproce">true: append [scale^-1] else append [scale]</param>
        public void Append(ScaledUnit scale, bool reciproce = false)
        {
            Append(scale.Unit, (reciproce) ? -scale.Exp : scale.Exp);
        }

        /// <summary>
        /// Append: Multiply (or divide) this Scale with an extra factor
        /// </summary>
        /// <param name="scale">ScaledUnit to append to this Scale</param>
        /// <param name="reciproce">true: append [scale^-1] else append [scale]</param>
        public void Append(Unit unit, int exp = 1)
        {
            var s = this.FirstOrDefault(t => (t.Unit == unit));
            if (s == null)
            {
                // factor not yet included.
                if (exp != 0) Add(new ScaledUnit(unit, exp));
            }
            else if (s.Exp == -exp)
            {
                // new factor cancels out available factor.
                Remove(s);
            }
            else
            {
                // new factor will modify available factor.
                if (exp != 0) s.ExpAdd(exp);
            }

            if (Count == 0) Add(new ScaledUnit(Unit.c, 1));
            else if (1 < Count)
            {
                s = this.FirstOrDefault(t => (t.Unit == Unit.c));
                if (s != null) Remove(s);
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
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            return (obj is Scale) ? AreEqual(this, obj as Scale) : false;
        }

        /// <summary>
        /// GetHashCode: Get the Hash code.
        /// </summary>
        /// <returns>The Hash value</returns>
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
            if (this.Count == 0) sb.Append(Unit.c.ToString(append_brackets: false));
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
                        sb.Append("1");
                        break;
                    case 1:
                        sb.Append(numerator[0].ToString(append_brackets: false, reciproce: false));
                        break;
                    default:
                        if (denominator.Count != 0) sb.Append("(");
                        bool first = true;
                        foreach (var s in numerator)
                        {
                            if (first) first = false;
                            else sb.Append(" ");
                            sb.Append(s.ToString(append_brackets: false, reciproce: false));
                        }
                        if (denominator.Count != 0) sb.Append(")");
                        break;
                }
                switch (denominator.Count)
                {
                    case 0:
                        break;
                    case 1:
                        sb.Append("/");
                        sb.Append(denominator[0].ToString(append_brackets: false, reciproce: true));
                        break;
                    default:
                        sb.Append("/");
                        sb.Append("(");
                        bool first = true;
                        foreach (var s in denominator)
                        {
                            if (first) first = false;
                            else sb.Append(" ");
                            sb.Append(s.ToString(append_brackets: false, reciproce: true));
                        }
                        sb.Append(")");
                        break;
                }
            }
            if (with_brackers)
            {
                sb.Insert(0, "[");
                sb.Append("]");
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
    }
}
