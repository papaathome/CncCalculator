namespace As.Tools.Data.Scales
{
    /// <summary>
    /// ScaledUnit: (Unit, (int)Exponent)
    /// </summary>
    /// <remarks>
    /// Where a Units is measure, a ScaledUnit is able to represent a unit in different forms.
    /// e.g.: a Hz scale is in 1/[s] = [s^-1] = (UnitTime.s, -1)
    /// e.g.: a cubic foot in in [ft] * [ft] * [ft] = [ft^3] = (UnitLength.ft, 3)
    /// The ScaledUnit class (and Scale class) integrates the use of Units in ScaledType.
    /// </remarks>
    public class ScaledUnit : ICloneable
    {
        #region Operators
        /// <summary>
        /// Equals: is the left side (syntactical) equal to the right side.
        /// </summary>
        /// <param name="left">a scale to compare</param>
        /// <param name="right">a scale to compare</param>
        /// <returns>true if the units and exponent on the left side are (syntactical) equal to the ones from the right side</returns>
        public static bool operator ==(ScaledUnit? left, ScaledUnit? right)
        {
            if (ReferenceEquals(left, right)) return true;
            if ((left is null) && (right is null)) return true;
            if ((left is null) || (right is null)) return false;
            return left.Equals(right);
        }

        /// <summary>
        /// Inequals: is the left side (syntactical) inequal to the right side.
        /// </summary>
        /// <param name="left">a scale to compare</param>
        /// <param name="right">a scale to compare</param>
        /// <returns>true if the units and exponent on the left side are (syntactical) inequal to the ones from the right side</returns>
        public static bool operator !=(ScaledUnit? left, ScaledUnit? right)
        {
            return !(left == right);
        }
        #endregion Operators

        #region Static Actions
        public static bool TryParse(string? value, out Unit unit)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                unit = Unit.c;
                return false;
            }
            var result = UnitConstantX.TryParse(value, out unit);
            if (!result) result = UnitLengthX.TryParse(value, out unit);
            if (!result) result = UnitWeightX.TryParse(value, out unit);
            if (!result) result = UnitTimeX.TryParse(value, out unit);
            if (!result) result = UnitRotationX.TryParse(value, out unit);
            return result;
        }
        #endregion Static Actions

        #region .ctor
        public ScaledUnit() : this(Unit.c) {}

        /// <summary>
        /// .ctor for a scale with given unit and (optional) exponent.
        /// </summary>
        /// <param name="unit">Unit for this scale</param>
        /// <param name="exp">Exponent for this scale (default: 1)</param>
        public ScaledUnit(Unit unit, int exp = 1)
        {
            if (exp == 0)
            {
                Unit = Unit.c;
                Exp = 1;
            }
            else
            {
                Unit = unit;
                Exp = exp;
            }
        }

        /// <summary>
        /// .ctor copy construtor to create a copy of another scale.
        /// </summary>
        /// <param name="other">scale to copy</param>
        public ScaledUnit(ScaledUnit other)
        {
            Unit = other.Unit;
            Exp = other.Exp;
        }
        #endregion .ctor

        #region Properties
        /// <summary>
        /// Unit the scale is in.
        /// </summary>
        public Unit Unit { get; private set; }

        /// <summary>
        /// The power to which the Unit is raised for this scale.
        /// </summary>
        public int Exp { get; private set; }
        #endregion Properteis

        #region Actions
        public ScaledUnit Clone()
        {
            return new ScaledUnit(this);
        }

        object ICloneable.Clone()
        {
            return new ScaledUnit(this);
        }

        /// <summary>
        /// Increment the Exponent with an given _value.
        /// </summary>
        /// <param name="value">Value to add to the exponent.</param>
        public void ExpAdd(int value) { Exp += value; }

        public void ExpSet(int value) { if (value != 0) Exp = value; }

        public void ExpReset() => ExpSet(1);

        /// <summary>
        /// Factor between the Unit and its base unit, taking the Exponent into account.
        /// </summary>
        /// <returns>Unit.Factor raised by the power of Exponent.</returns>
        public double Factor()
        {
            return Exp switch
            {
                0 => 1.0,
                1 => Unit.Factor(),
                _ => Math.Pow(Unit.Factor(), Exp),
            };
        }

        /// <summary>
        /// Equals: is this scale (syntactical) equal to another one.
        /// </summary>
        /// <param name="obj">Other scale to compare with</param>
        /// <returns>true if this scale units and exponent are (syntactical) equal to the other ones</returns>
        public override bool Equals(object? obj)
        {
            var scale = obj as ScaledUnit;
            return
                (scale is not null) &&
                (Unit == scale.Unit) &&
                (Exp == scale.Exp);
        }

        /// <summary>
        /// GetHashCode: Calculate the hash code for this scale.
        /// </summary>
        /// <returns>Hash _value</returns>
        public override int GetHashCode()
        {
            var hashCode = -2002594041;
            hashCode = hashCode * -1521134295 + Unit.GetHashCode();
            hashCode = hashCode * -1521134295 + Exp.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// ToString: readable representation of this scale, with or without brackers.
        /// </summary>
        /// <param name="append_brackets">Use brackes or not</param>
        /// <returns></returns>
        public string ToString(bool append_brackets, bool reciproce)
        {
            var e = (reciproce) ? -Exp : Exp;
            var r = (e == 1)
                ? $"{Unit.ToString(append_brackets: false)}"
                : $"{Unit.ToString(append_brackets: false)}^{e}";
            return (append_brackets) ? $"[{r}]" : r;
        }

        /// <summary>
        /// ToString: readable representation of this scale, with brackers.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToString(append_brackets: true, reciproce: false);
        }
        #endregion Actions
    }
}
