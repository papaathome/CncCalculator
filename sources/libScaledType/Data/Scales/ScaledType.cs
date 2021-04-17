using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace As.Tools.Data.Scales
{
    /// <summary>
    /// ScaledType: a value with scale definition attached to it. (value, scale)
    /// </summary>
    /// <remarks>
    /// The scale in this class is a set of Scale class instances, scale = { s | s = Scale instance }
    /// All Scale instances are multiplied with each other to get the correct scale.
    /// e.g: a cubic foot = 1 [ft^3] = (1, { (ft, 3) })
    /// e.g: speed in meters per second = 1 [m/s] = 1[m s^-1] = (1.0, { (m, 1), (s, -1) })
    /// </remarks>
    public abstract class ScaledType
    {
#if USE_LOG4NET
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
#endif

        public object scaledTypeToolCuttingDepth;

        /// <summary>
        /// BaseNormal: expressing the ScaleType value of (1, [Unit.Base]) of the ScaleType
        /// </summary>
        /// <remarks>
        /// Example: ScaleType (x, [cm])
        ///    The Unit for [cm] is Unit.Base, the meter [m]
        ///    BaseNormal = (100, [cm]), there are 100 [cm] in (1, [m]).
        /// 
        /// This value is usefull for converting from one ScaledType value to another
        /// (on the premisse that both ScaledTypes are based on the same base units set).
        /// To use this for confersion of e.g. a value (x, [cm]) to (y, [ft])
        ///    (c1, [ft]) = (1, [m]) = BaseNormal([ft])
        ///    (c2, [cm]) = (1, [m]) = BaseNormal([cm])
        ///    then
        ///    (y, [ft]) = (x, [cm]) * (c1, [ft])/(c2, [cm])
        ///              = (c*c1/c2, [cm ft cm^-1])
        /// 
        /// Note: The relation between
        ///       (c1, [Unit.Base]) = BaseNormal([Unit]) and
        ///       (C1, [Unit]) = BaseFactor([Unit])
        ///       is c1 = 1/C1
        /// </remarks>
        /// <seealso cref="BaseFactor"/>
        /// <param name="target">A list of ScaledUnit instances that form the scale to use.</param>
        /// <returns>
        /// (x, [Unit]), the conversion of (1, [Unit.Base]) where Unit.Base is in { [m], [g], [s],... }
        /// </returns>
        static DoubleST BaseNormal(Scale target)
        {
            var result = new DoubleST(1.0);
            foreach (var s in target)
            {
                result.Append(s.Unit, s.Exp);
                result = result * s.Factor();
            }
            return result;
        }

        /// <summary>
        /// BaseFactor: expressing the ScaleType (1, [target]) in basic Units of the ScaleType
        /// </summary>
        /// <remarks>
        /// Example: ScaleType (1, [cm])
        ///    The BaseUnit for [cm] is Unit.Base, the meter [m]
        ///    BaseNormal = (0.01, [m]), (1, [cm]) = (0.01, [m])
        /// 
        /// This value is usefull for checking if a conversion is possible from one value
        /// to another. Both value BaseFactors must have identical scales.
        /// 
        /// Pitfall: Do not use for conversion!
        /// e.g.:
        ///    (C1, [m]) = (1, [ft]) = BaseFactor([ft])
        ///    (C2, [m]) = (1, [cm]) = BaseFactor([cm])
        ///    then
        ///    (y, [ft]) = (x, [cm])*(C2, [m])/(C1, [m])
        ///              = (x*C2/C1, [cm])
        ///              The value appears to be correct but the scale is incorrect.
        /// 
        /// Note: The relation between
        ///       (c1, [Unit.Base]) = BaseNormal([Unit]) and
        ///       (C1, [Unit]) = BaseFactor([Unit])
        ///       is c1 = 1/C1
        /// </remarks>
        /// <seealso cref="BaseNormal"/>
        /// <param name="target">A list of ScaledUnit instances that form the scale to use.</param>
        /// <returns>
        /// (x, [Unit.Base]), the conversion of (1, [Unit]) where Unit.Base is in { [m], [g], [s],... }
        /// </returns>
        static DoubleST BaseFactor(Scale target)
        {
            var result = new DoubleST(1.0);
            foreach (var s in target)
            {
                result.Append(s.Unit.Base(), s.Exp);
                result = result / s.Factor();
            }
            return result;
        }

        /// <summary>
        /// Equals operator ==: it the left side equal to the right side (in value and scale)
        /// </summary>
        /// <param name="left">A value and scale to compare</param>
        /// <param name="right">A value and scale to compare</param>
        /// <returns>true if both value and scale are equal.</returns>
        public static bool operator ==(ScaledType left, ScaledType right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (((object)left == null) && ((object)right == null)) return true;
            if (((object)left == null) || ((object)right == null)) return false;
            return left.Equals(right);
        }

        /// <summary>
        /// Not equals operator !=: it the left side not equal to the right side (in value and scale)
        /// </summary>
        /// <param name="left">A value and scale to compare</param>
        /// <param name="right">A value and scale to compare</param>
        /// <returns>true if at least one of value or scale is unequal.</returns>
        public static bool operator !=(ScaledType left, ScaledType right)
        {
            return !(left == right);
        }

        /// <summary>
        /// .ctor: create a ScaledType instance from a given value and unit.
        /// </summary>
        /// <param name="value">The value of the ScaledType</param>
        /// <param name="unit">The scale of the ScaledType</param>
        public ScaledType(Unit unit)
        {
            SetScale(new Scale(new ScaledUnit(unit)));
        }

        /// <summary>
        /// .ctor: create a ScaledType instance from a given balue and unit.
        /// </summary>
        /// <param name="value">The value of the ScaledType</param>
        /// <param name="scale">The scale of the ScaledType</param>
        /// <exception cref="ArgumentException">Cannot parse the scale</exception>
        public ScaledType(string scale)
        {
            Scale s;
            if (Scale.TryParse(scale, out s)) SetScale(s);
            else
            {
                var msg = $"ScaledType: .ctor, cannot parse '{scale}'";
#if USE_LOG4NET
                log.Debug(msg);
#endif
                throw new ArgumentException(msg, "scale");
            }
        }

        /// <summary>
        /// .ctor: create a ScaledType instance form a given (optional)value and (optional)scale.
        /// </summary>
        /// <param name="value">The value of the ScaledType, (default: 0.0)</param>
        /// <param name="scale">The scale of the ScaledType, (default: [#])</param>
        public ScaledType(Scale scale = null)
        {
            if (scale == null) SetScale(new Scale(new ScaledUnit(Unit.c, 1)));
            else SetScale(Scale.Normalise(scale));
        }

        /// <summary>
        /// Duplicate: create an duplicate of this child.
        /// </summary>
        /// <returns>Copy of child instance.</returns>
        public abstract ScaledType Duplicate();

        /// <summary>
        /// The scale of a ScaledType instance.
        /// </summary>
        public Scale Scale { get; private set; } = null;

        /// <summary>
        /// Event on change of the scale or value.
        /// </summary>
        public event EventHandler OnChanged;

        /// <summary>
        /// Fire OnChaged event
        /// </summary>
        private void Changed()
        {
            OnChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Event on change of the scale.
        /// </summary>
        public event EventHandler OnScaleChanged;

        /// <summary>
        /// Fire OnScaleChaged event
        /// </summary>
        private void ScaleChanged()
        {
            OnScaleChanged?.Invoke(this, EventArgs.Empty);
            Changed();
        }

        /// <summary>
        /// GetScale: Get the scale, normalise to create an indemendent copy.
        /// </summary>
        /// <returns>Copy of the scale</returns>
        public Scale GetScale()
        {
            return Scale.Normalise(Scale);
        }

        /// <summary>
        /// SetScale: Set to a new scale, normalise on use to create an independent copy.
        /// </summary>
        /// <param name="scale">Scale to copy.</param>
        public void SetScale(Scale scale)
        {
            if (Scale != null) Scale.CollectionChanged -= Scale_CollectionChanged;
            Scale = Scale.Normalise(scale);
            if (Scale != null) Scale.CollectionChanged += Scale_CollectionChanged;
            ScaleChanged();
        }

        /// <summary>
        /// Event on change of the scale.
        /// </summary>
        private void Scale_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ScaleChanged();
        }

        /// <summary>
        /// BaseNormal version of this instance.
        /// </summary>
        /// <returns>
        /// (x, [Scale]), the conversion of (1, [Basic Units]) where Basic Units are in { [m], [g], [s],... }
        /// </returns>
        public DoubleST BaseNormal()
        {
            return BaseNormal(Scale);
        }

        /// <summary>
        /// BaseFactor with this instance
        /// </summary>
        /// <returns>
        /// (x, [Basic Units]), the conversion of (1, [Scale]) where Basic Units are in { [m], [g], [s],... }
        /// </returns>
        public DoubleST BaseFactor()
        {
            return BaseFactor(Scale);
        }

        /// <summary>
        /// IsFloatingPoint: indicator about the value type
        /// </summary>
        public abstract bool IsFloatingPoint { get; }

        /// <summary>
        /// Event on change of the value (for all children to use).
        /// </summary>
        public event EventHandler OnValueChanged;

        /// <summary>
        /// Fire OnValueChanged event.
        /// </summary>
        protected void ValueChanged()
        {
            OnValueChanged?.Invoke(this, EventArgs.Empty);
            Changed();
        }

        /// <summary>
        /// Through the parent use of the value.
        /// </summary>
        /// <returns>The value as used by the child.</returns>
        public abstract object GetValue();

        /// <summary>
        /// Set child value.
        /// </summary>
        /// <param name="value">string representation of a new value</param>
        /// <returns>True if the value is accepted by the child.</returns>
        public abstract bool SetValue(string value);

        /// <summary>
        /// Set child value.
        /// </summary>
        /// <param name="value">ScaledType child from which the value should be used.</param>
        /// <returns>True if the value is accepted by the child.</returns>
        public abstract bool SetValue(ScaledType value);

        /// <summary>
        /// SetValue: change to value from another ScaledType, check for convertability of scales and rescale the new value.
        /// </summary>
        /// <param name="value">New value (possibly in different scale)</param>
        /// <returns>True if scales were convertable and a new value is set.</returns>
        public abstract bool SetValueScaled(ScaledType value);

        /// <summary>
        /// SetValueScaled: change value from another ScaledType value, check for convertability of scales.
        /// </summary>
        /// <param name="value">New value (possibly in different scale)</param>
        /// <param name="copy">Action to be done by the child to get the correct value.</param>
        /// <returns>True if scales were convertable and a new value is set.</returns>
        /// <remarks>
        /// A value in DoubleST is required for scaling.
        /// Scaling is done with factors and these are (by design) doubles.
        /// </remarks>
        protected bool SetValueScaled(DoubleST value, Action<DoubleST>copy)
        {
            if (Scale == value.Scale)
            {
                copy(null);
                return true;
            }

            var v1b = BaseFactor();
            var v2b = value.BaseFactor();
            if (v1b.Scale == v2b.Scale)
            {
                var v1n = BaseNormal();
                var v2n = value.BaseNormal();
                // var v2 = value * v1n / v2n
                copy(value * v1n / v2n);
                return true;

                // extra check:
                //var result = (scale == v2.scale);
                //if (result) Value = v2.Value;
                //return result;
            }
            return false;
        }

        /// <summary>
        /// Equals: is this scaled value (syntactical) equal to the other scaled value.
        /// </summary>
        /// <param name="obj">a scaled value to compare with</param>
        /// <returns>
        /// true if the values the scale from this are
        /// (syntactical) equal to the ones from the other
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            var type = obj as ScaledType;
            return type != null &&
                   Scale == type.Scale;
        }

        /// <summary>
        /// GetHashCode: Get the Hash code.
        /// </summary>
        /// <returns>The Hash value</returns>
        public override int GetHashCode()
        {
            var hashCode = -2079756921;
            hashCode = hashCode * -1521134295 + Scale.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Append: Multiply (or divide) this Scale with an extra factor
        /// </summary>
        /// <param name="other">Scale to append to this Scale</param>
        /// <param name="reciproce">true: append [scale^-1] else append [scale]</param>
        public void Append(ScaledType other, bool reciproce = false)
        {
            Append(other.Scale, reciproce);
        }

        /// <summary>
        /// Append: Multiply (or divide) this Scale with an extra factor
        /// </summary>
        /// <param name="scale">Scale to append to this Scale</param>
        /// <param name="reciproce">true: append [scale^-1] else append [scale]</param>
        public void Append(Scale scale, bool reciproce = false)
        {
            Scale.Append(scale, reciproce);
        }

        /// <summary>
        /// Append: Multiply (or divide) this Scale with an extra factor
        /// </summary>
        /// <param name="scale">Scale to append to this Scale</param>
        /// <param name="reciproce">true: append [scale^-1] else append [scale]</param>
        public void Append(ScaledUnit scale, bool reciproce = false)
        {
            Scale.Append(scale, reciproce);
        }

        /// <summary>
        /// Append: Multiply (or divide) this Scale with an extra factor
        /// </summary>
        /// <param name="scale">ScaledUnit to append to this Scale</param>
        /// <param name="reciproce">true: append [scale^-1] else append [scale]</param>
        public void Append(Unit unit, int exp = 1)
        {
            Scale.Append(unit, exp);
        }

        /// <summary>
        /// Assign: assign a new value to this instance
        /// </summary>
        /// <param name="value">The new value</param>
        /// <exception cref="ArgumentOutOfRangeException">Scales of this and the other must be (syntactical) equivalents</exception>
        /// <remarks>It is not possible to override operator=()</remarks>
        protected void Assign(ScaledType other)
        {
            if (Scale != other.Scale)
            {
                var msg = $"Assign: not supported: ({Scale})Value = ({other.Scale}){other.GetValue()}";
#if USE_LOG4NET
                log.Debug(msg);
#endif
                throw new ArgumentOutOfRangeException(msg);
            }
        }
    }
}
