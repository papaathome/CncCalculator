using System.Globalization;
using System.Numerics;

namespace As.Tools.Data.Scales
{
    public class ScaledType<T> :
        IScaledType<ScaledType<T>, T>
        where T : INumber<T>
    {
#if USE_LOG4NET
        protected static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
#endif

        const NumberStyles Numeric =
            NumberStyles.AllowLeadingWhite |
            NumberStyles.AllowTrailingWhite |
            NumberStyles.AllowLeadingSign |
            NumberStyles.AllowTrailingSign |
            NumberStyles.AllowDecimalPoint;

        #region Operations
        /// <summary>
        /// Equals operator ==: it the left side equal to the right side (in _value and scale)
        /// </summary>
        /// <param name="left">A _value and scale to compare</param>
        /// <param name="right">A _value and scale to compare</param>
        /// <returns>true if both _value and scale are equal.</returns>
        public static bool operator ==(ScaledType<T>? left, ScaledType<T>? right)
        {
            if (ReferenceEquals(left, right)) return true;
            if ((left is null) && (right is null)) return true;
            if ((left is null) || (right is null)) return false;
            return left.Equals(right);
        }

        /// <summary>
        /// Not equals operator !=: it the left side not equal to the right side (in _value and scale)
        /// </summary>
        /// <param name="left">A _value and scale to compare</param>
        /// <param name="right">A _value and scale to compare</param>
        /// <returns>true if at least one of _value or scale is unequal.</returns>
        public static bool operator !=(ScaledType<T>? left, ScaledType<T>? right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Add operator +: Add two values with equal scale.
        /// </summary>
        /// <param name="left">A _value to add to</param>
        /// <param name="right">A _value to add</param>
        /// <exception cref="ArgumentOutOfRangeException">left scale is not (semantical) equal to right scale.</exception>
        /// <returns>(left._value + right._value, [scale])</returns>
        public static ScaledType<T> operator +(ScaledType<T> left, ScaledType<T> right)
        {
            return Add(left, right);
        }

        /// <summary>
        /// Sub operator -: Subtract two values with equal scale.
        /// </summary>
        /// <param name="left">A _value to subtract from</param>
        /// <param name="right">A _value to subtract</param>
        /// <exception cref="ArgumentOutOfRangeException">left scale is not (semantical) equal to right scale.</exception>
        /// <returns>(left._value - right._value, [scale])</returns>
        public static ScaledType<T> operator -(ScaledType<T> left, ScaledType<T> right)
        {
            return Sub(left, right);
        }

        /// <summary>
        /// Mul operator *: Multiply two values.
        /// </summary>
        /// <param name="left">A _value to multiply with</param>
        /// <param name="right">A _value to multiply</param>
        /// <returns>(left._value * right._value, [Normalised(left.scale right.scale)])</returns>
        public static ScaledType<T> operator *(ScaledType<T> left, ScaledType<T> right)
        {
            return Mul(left, right);
        }

        /// <summary>
        /// Mul operator *: Multiply a _value and a scalar.
        /// </summary>
        /// <param name="left">A _value to multiply with</param>
        /// <param name="right">A scalar to multiply</param>
        /// <returns>(left._value * right, [left.scale])</returns>
        public static ScaledType<T> operator *(ScaledType<T> left, T right)
        {
            return new ScaledType<T>(left.Value * right, left.Scale);
        }

        /// <summary>
        /// Mul operator *: Multiply a scalar and a _value.
        /// </summary>
        /// <param name="left">A scalar to multiply with</param>
        /// <param name="right">A _value to multiply</param>
        /// <returns>(left * right._value, [right.scale])</returns>
        public static ScaledType<T> operator *(T left, ScaledType<T> right)
        {
            return new ScaledType<T>(left * right.Value, right.Scale);
        }

        /// <summary>
        /// Div operator /: Divide two values.
        /// </summary>
        /// <param name="left">A _value to divide to</param>
        /// <param name="right">A _value to divide with</param>
        /// <returns>(left._value / right._value, [Normalised(left.scale, right.scale^-1)])</returns>
        public static ScaledType<T> operator /(ScaledType<T> left, ScaledType<T> right)
        {
            return Div(left, right);
        }

        /// <summary>
        /// Div operator /: Divide a _value and a scalar.
        /// </summary>
        /// <param name="left">A _value to divide to</param>
        /// <param name="right">A scalar to divide with</param>
        /// <returns>(left._value / right, [left.scale])</returns>
        public static ScaledType<T> operator /(ScaledType<T> left, T right)
        {
            return Div(left, new ScaledType<T>(right));
        }

        /// <summary>
        /// Div operator /: Divide a scalar and a _value.
        /// </summary>
        /// <param name="left">A scalar to divide to</param>
        /// <param name="right">A _value to divide with</param>
        /// <returns>(left / right._value, [right.scale^-1])</returns>
        public static ScaledType<T> operator /(T left, ScaledType<T> right)
        {
            return Div(new ScaledType<T>(left), right);
        }
        #endregion Operations

        #region Static actions
        /// <summary>
        /// Scan a string for a ScaledType<T> _value with no or a bracketless scale."
        /// </summary>
        /// <param name="value">Value to scan.</param>
        /// <param name="value_default">Default _value to use if scan fails for the _value part.</param>
        /// <param name="unit_default">Default scale to use if scan fails for the scale part.</param>
        /// <returns></returns>
        private static readonly char[] separator = [' '];

        public static ScaledType<T> Scan(string value, T value_default, string unit_default)
        {
            T V = value_default;
            string[] Va;
            if (string.IsNullOrWhiteSpace(value))
            {
                Va = ["", unit_default];
            }
            else
            {
                Va = value.Trim().Split(separator, StringSplitOptions.RemoveEmptyEntries);
                switch (Va.Length)
                {
                    default:
                        Va = ["", unit_default];
                        break;
                    case 1:
                        Va = [Va[0], unit_default];
                        goto case 2;
                    case 2:
                        V = (T.TryParse(Va[0], Numeric, CultureInfo.InvariantCulture, out T? v) && (v != null)) ? v : value_default;
                        break;
                }
            }
            return new ScaledType<T>(V, $"[{Va[1]}]");
        }

        /// <summary>
        /// Add operator +: Add two values with equal scale or both without scale.
        /// </summary>
        /// <param name="left">A _value to add to</param>
        /// <param name="right">A _value to add</param>
        /// <exception cref="ArgumentOutOfRangeException">left scale is not (semantical) equal to right scale.</exception>
        /// <returns>(left._value + right._value, [scale])</returns>
        public static ScaledType<T> Add(ScaledType<T> left, ScaledType<T> right)
        {
            if (left.Scale != right.Scale)
            {
                var msg = $"ScaledType<T>: operation not supported: {left.Scale} + {right.Scale}";
                //log.Debug(msg);
                throw new ArgumentOutOfRangeException(msg);
            }
            return new ScaledType<T>(left.Value + right.Value, left.Scale);
        }

        /// <summary>
        /// Sub operator -: Subtract two values with equal scale or both without scale.
        /// </summary>
        /// <param name="left">A _value to subtract from</param>
        /// <param name="right">A _value to subtract</param>
        /// <exception cref="ArgumentOutOfRangeException">left scale is not (semantical) equal to right scale.</exception>
        /// <returns>(left._value - right._value, [scale])</returns>
        public static ScaledType<T> Sub(ScaledType<T> left, ScaledType<T> right)
        {
            if (left.Scale != right.Scale)
            {
                var msg = $"ScaledType<T>: operation not supported: {left.Scale} - {right.Scale}";
                //log.Debug(msg);
                throw new ArgumentOutOfRangeException(msg);
            }
            return new ScaledType<T>(left.Value - right.Value, left.Scale);
        }

        /// <summary>
        /// Mul operator *: Multiply two values.
        /// </summary>
        /// <param name="left">A _value to multiply with</param>
        /// <param name="right">A _value to multiply</param>
        /// <returns>(left._value * right._value, [Normalised(left.scale right.scale)])</returns>
        public static ScaledType<T> Mul(ScaledType<T> left, ScaledType<T> right)
        {
            var result = new ScaledType<T>(left.Value * right.Value, left.Scale);
            result.Append(right.Scale);
            return result;
        }

        /// <summary>
        /// Div operator /: Divide two values.
        /// </summary>
        /// <param name="left">A _value to divide to</param>
        /// <param name="right">A _value to divide with</param>
        /// <returns>(left._value / right._value, [Normalised(left.scale, right.scale^-1)])</returns>
        public static ScaledType<T> Div(ScaledType<T> left, ScaledType<T> right)
        {
            if (T.IsZero(right.Value)) throw new DivideByZeroException($"{left} / {right} ");
            var result = new ScaledType<T>(left.Value / right.Value, left.Scale);
            result.Append(right.Scale, reciproce: true);
            return result;
        }
        #endregion Static actions

        #region .ctor
        public ScaledType()
        {
            Scale.CollectionChanged += Scale_CollectionChanged;
        }

        public ScaledType(T value) : this()
        {
            Value = value;
        }

        /// <summary>
        /// .ctor: create a ScaledType instance form a given (optional)_value and (optional)scale.
        /// </summary>
        /// <param name="scale">The scale of the ScaledType, (default: [#])</param>
        public ScaledType(Scale scale)
        {
            Scale = scale.Clone();
        }

        /// <summary>
        /// .ctor: create a ScaledType instance from a given _value and unit.
        /// </summary>
        /// <param name="value">The _value of the ScaledType</param>
        /// <param name="unit">The scale of the ScaledType</param>
        public ScaledType(Unit unit, int exp = 1) : this()
        {
            Scale = new Scale(unit, exp);
        }

        /// <summary>
        /// .ctor: create a ScaledType instance from a given value and unit.
        /// </summary>
        /// <param name="value">The _value of the ScaledType</param>
        /// <param name="scale">The scale of the ScaledType</param>
        /// <exception cref="ArgumentException">Cannot parse the scale</exception>
        public ScaledType(string scale)
        {
            if (TrySetScale(scale, out string err)) return;
            var msg = $"ScaledType<{nameof(T)}>: .ctor arg={nameof(scale)}; {err}";
#if USE_LOG4NET
            Log.Debug(msg);
#endif
            throw new ArgumentException(msg, nameof(scale));
        }

        /// <summary>
        /// .ctor: create a ScaledType instance from a given _value and unit.
        /// </summary>
        /// <param name="value">The _value of the ScaledType</param>
        /// <param name="unit">The scale of the ScaledType</param>
        public ScaledType(T value, Unit unit, int exp = 1) : this(unit, exp)
        {
            Value = value;
        }

        /// <summary>
        /// .ctor: create a ScaledType instance from a given balue and unit.
        /// </summary>
        /// <param name="value">The _value of the ScaledType</param>
        /// <param name="scale">The scale of the ScaledType</param>
        /// <exception cref="ArgumentException">Cannot parse the scale</exception>
        public ScaledType(T value, string scale) : this(scale)
        {
            Value = value;
        }

        /// <summary>
        /// .ctor: create a ScaledType instance form a given (optional)_value and (optional)scale.
        /// </summary>
        /// <param name="value">The _value of the ScaledType, (default: 0.0)</param>
        /// <param name="scale">The scale of the ScaledType, (default: [#])</param>
        public ScaledType(T value, Scale scale) : this(scale)
        {
            Value = value;
        }

        /// <summary>
        /// .ctor: copy constructor.
        /// </summary>
        /// <param name="other">Data to copy.</param>
        public ScaledType(ScaledType<T> other) : this(other.Scale)
        {
            Value = other.Value;
        }

        public ScaledType(IScaledType<ScaledType<T>, T> value) : this(value.Value, value.Scale) { }
        #endregion .ctor

        #region Value
        /// <summary>
        /// The value of a ScaledType instance.
        /// </summary>
        public T Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    ValueChanged();
                }
            }
        }

        /// <summary>
        /// Container for Value.
        /// </summary>
        T _value = default!;

        /// <summary>
        /// Event on change of the _value (for all children to use).
        /// </summary>
        public event ScaledTypeEvent? OnValueChanged;

        /// <summary>
        /// Fire OnValueChanged event.
        /// </summary>
        protected void ValueChanged()
        {
            var e = new ScaledTypeEventArgs(value_changed: true);
            OnValueChanged?.Invoke(this, e);
            Changed(e);
        }

        /// <summary>
        /// Assign: assign a new value to this instance, scaling checked.
        /// See <see cref="SetValueScaled(ScaledType<T> value)"/> for unchecked version.
        /// </summary>
        /// <param name="value">The new value</param>
        /// <remarks>It is not possible to override operator=()</remarks>
        public void Assign(ScaledType<T> other)
        {
            if (ReferenceEquals(this, other)) return;
            if (Scale != other.Scale)
            {
                var msg = $"Assign: not supported: Value {Scale} = {other.Value} {other.Scale}";
#if USE_LOG4NET
                Log.Debug(msg);
#endif
                throw new ArgumentOutOfRangeException(msg);
            }
            Value = other.Value;
        }

        /// <summary>
        /// SetValue: change to value from another ScaledType, no scaling checked.
        /// See <see cref="Assign(ScaledType<T> value)"/> for checked version.
        /// </summary>
        /// <param name="other">ScaledType from which the value should be used.</param>
        /// <returns>True if the value is accepted by the child.</returns>
        public bool SetValue(ScaledType<T> other)
        {
            if (ReferenceEquals(this, other)) return true;
            Value = other.Value;
            return true;
        }

        /// <summary>
        /// SetValue: change to value from another ScaledType, check for convertability of scales and rescale the new Value.
        /// See <see cref="Assign(ScaledType<ScaledType<T>, T> value)"/> for unchecked version.
        /// </summary>
        /// <param name="value">New value (possibly in different scale)</param>
        /// <returns>True if scales were convertable and a new value is set.</returns>
        public bool SetValueScaled(ScaledType<T> value)
        {
            if (ReferenceEquals(this, value)) return true;
            return SetValueScaled(value, (ScaledType<T> v2) =>
            {
                Value = v2.Value;
            });
        }

        /// <summary>
        /// SetValue: change to value from another IScaledType, no scaling checked.
        /// See <see cref="Assign(IScaledType<ScaledType<T>, T> value)"/> for unchecked version.
        /// </summary>
        /// <param name="other">ScaledType from which the value should be used.</param>
        /// <returns>True if the value is accepted by the child.</returns>
        bool IScaledType<ScaledType<T>, T>.SetValue(IScaledType<ScaledType<T>, T> other)
        {
            if (ReferenceEquals(this, other)) return true;
            Value = other.Value;
            return true;
        }

        /// <summary>
        /// SetValue: change to value from another IScaledType, check for convertability of scales and rescale the new Value.
        /// </summary>
        /// <param name="value">New value (possibly in different scale)</param>
        /// <returns>True if scales were convertable and a new value is set.</returns>
        bool IScaledType<ScaledType<T>, T>.SetValueScaled(IScaledType<ScaledType<T>, T> value)
        {
            if (ReferenceEquals(this, value)) return true;

            var c = new ScaledType<T>(value);
            if (c is null) return false;
            return SetValueScaled(c, (ScaledType<T> v2) =>
            {
                Value = v2.Value;
            });
        }

        /// <summary>
        /// SetValue: change to value from string content.
        /// </summary>
        /// <param name="value">string representation of a new value</param>
        /// <returns>True if the value is accepted by the child.</returns>
        public bool SetValue(string value, CultureInfo? culture = null)
        {
            var result = T.TryParse(
                value,
                Numeric,
                culture ?? CultureInfo.InvariantCulture,
                out T? v) && (v != null);
            if (result) Value = v!;
            return result;
        }

        /// <summary>
        /// SetValueScaled: change to value from another ScaledType Value, check for convertability of scales.
        /// </summary>
        /// <param name="value">New value (possibly in different scale)</param>
        /// <param name="copy">Action to be done by the child to get the correct value.</param>
        /// <returns>True if scales were convertable and a new value is set.</returns>
        /// <remarks>
        /// A value in ScaledTypeOfT is required for scaling.
        /// Scaling is done with factors and these are (by design) doubles.
        /// </remarks>
        protected bool SetValueScaled(ScaledType<T> value, Action<ScaledType<T>> copy)
        {
            if (Scale == value.Scale)
            {
                copy(value);
                return true;
            }

            var v1b = BaseFactor();
            var v2b = value.BaseFactor();
            if (v1b.Scale == v2b.Scale)
            {
                var v1n = BaseNormal();
                var v2n = value.BaseNormal();
                var v3n = v1n / v2n;
                var v4n = T.CreateChecked(v3n.Value);
                copy(value * v4n);
                return true;

                // extra check:
                //var result = (scale == v2.scale);
                //if (result) Value = v2.Value;
                //return result;
            }
            return false;
        }
        #endregion Value

        #region Scale
        /// <summary>
        /// The scale of a ScaledType instance.
        /// </summary>
        public Scale Scale
        {
            get => _scale;
            set
            {
                var old_scale = _scale.Clone();
                Scale.CollectionChanged -= Scale_CollectionChanged;
                _scale = Scale.Normalise(value);
                Scale.CollectionChanged += Scale_CollectionChanged;
                ScaleChanged(old_scale);
            }
        }
        Scale _scale = [];

        /// <summary>
        /// Event on change of the scale.
        /// </summary>
        public event ScaledTypeEvent? OnScaleChanged;

        /// <summary>
        /// Fire OnScaleChaged event
        /// </summary>
        private void ScaleChanged(Scale old_scale)
        {
            var e = new ScaledTypeEventArgs(scale_changed: true, old_scale: old_scale, new_scale: Scale);
            OnScaleChanged?.Invoke(this, e);
            Changed(e);
        }

        public bool TrySetScale(string scale, out string err)
        {
            if (!Scale.TryParse(scale, out Scale? s))
            {
                err = $"parse failed; scale=\"{scale}\"";
                return false;
            }
            err = "";
            Scale = s;
            return true;
        }

        /// <summary>
        /// Event on change of the scale.
        /// </summary>
        private void Scale_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ScaleChanged(Scale);
        }

        /// <summary>
        /// Append: Multiply (or divide) this Scale with an extra factor
        /// </summary>
        /// <param name="other">Scale to append to this Scale</param>
        /// <param name="reciproce">true: append [scale^-1] else append [scale]</param>
        public void Append(ScaledType<T> other, bool reciproce = false)
        {
            Append(other.Scale, reciproce);
        }

        void IScaledType<ScaledType<T>, T>.Append(IScaledType<ScaledType<T>, T> other, bool reciproce)
        {
            Append((ScaledType<T>)other, reciproce);
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
        public void Append(Unit unit, int exp = 1, bool reciproce = false)
        {
            Scale.Append(unit, exp, reciproce);
        }
        #endregion Scale

        #region Actions
        /// <summary>
        /// Duplicate: create a copy of this.
        /// </summary>
        /// <returns>Copy of this.</returns>
        public ScaledType<T> Clone()
        {
            return new ScaledType<T>(this);
        }

        object ICloneable.Clone()
        {
            return new ScaledType<T>(this);
        }

        /// <summary>
        /// Event on change of the scale or _value.
        /// </summary>
        public event ScaledTypeEvent? OnChanged;

        /// <summary>
        /// BaseNormal: expressing the ScaleType _value of (1, [Unit.Base]) of the ScaleType
        /// </summary>
        /// <remarks>
        /// Example: ScaleType (x, [cm])
        ///    The Unit for [cm] is Unit.Base, the meter [m]
        ///    BaseNormal = (100, [cm]), there are 100 [cm] in (1, [m]).
        /// 
        /// This _value is usefull for converting from one ScaledType _value to another
        /// (on the premisse that both ScaledTypes are based on the same base units set).
        /// To use this for confersion of e.g. a _value (x, [cm]) to (y, [ft])
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
        ScaledType<double> BaseNormal(Scale target)
        {
            var result = new ScaledType<double>(1.0);
            foreach (var s in target)
            {
                result.Append(s.Unit, s.Exp);
                result *= s.Factor();
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
        /// This _value is usefull for checking if a conversion is possible from one _value
        /// to another. Both _value BaseFactors must have identical scales.
        /// 
        /// Pitfall: Do not use for conversion!
        /// e.g.:
        ///    (C1, [m]) = (1, [ft]) = BaseFactor([ft])
        ///    (C2, [m]) = (1, [cm]) = BaseFactor([cm])
        ///    then
        ///    (y, [ft]) = (x, [cm])*(C2, [m])/(C1, [m])
        ///              = (x*C2/C1, [cm])
        ///              The _value appears to be correct but the scale is incorrect.
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
        ScaledType<double> BaseFactor(Scale target)
        {
            var result = new ScaledType<double>(1.0);
            foreach (var s in target)
            {
                result.Append(s.Unit.Base(), s.Exp);
                result /= s.Factor();
            }
            return result;
        }

        /// <summary>
        /// BaseNormal version of this instance.
        /// </summary>
        /// <returns>
        /// (x, [Scale]), the conversion of (1, [Basic Units]) where Basic Units are in { [m], [g], [s],... }
        /// </returns>
        public ScaledType<double> BaseNormal()
        {
            return BaseNormal(Scale);
        }

        /// <summary>
        /// BaseFactor with this instance
        /// </summary>
        /// <returns>
        /// (x, [Basic Units]), the conversion of (1, [Scale]) where Basic Units are in { [m], [g], [s],... }
        /// </returns>
        public ScaledType<double> BaseFactor()
        {
            return BaseFactor(Scale);
        }

        /// <summary>
        /// Fire OnChaged event
        /// </summary>
        private void Changed(ScaledTypeEventArgs? e = null)
        {
            OnChanged?.Invoke(this, e ?? new ScaledTypeEventArgs());
        }

        /// <summary>
        /// Equals: is this scaled _value (syntactical) equal to the other scaled _value.
        /// </summary>
        /// <param name="obj">a scaled _value to compare with</param>
        /// <returns>
        /// true if the values the scale from this are
        /// (syntactical) equal to the ones from the other
        /// </returns>
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            var type = obj as ScaledType<T>;
            return
                (type is not null) &&
                (Scale == type.Scale) &&
                (Value == type.Value);
        }

        /// <summary>
        /// GetHashCode: Get the Hash code.
        /// </summary>
        /// <returns>The Hash _value</returns>
#pragma warning disable IDE0070 // Use 'System.HashCode'
        public override int GetHashCode()
#pragma warning restore IDE0070 // Use 'System.HashCode'
        {
            var hashCode = -2079756921;
            hashCode ^= Scale.GetHashCode();
            hashCode ^= Value.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// ToString: Readable representation of this (_value [Scale]) instance.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Value} {Scale}";
        }
        #endregion Actions
    }

    public delegate void ScaledTypeEvent(object caller, ScaledTypeEventArgs e);

    /// <summary>
    /// event arguments: both .ctor arguments false -> both event values true
    /// </summary>
    /// <param name="value_changed"></param>
    /// <param name="scale_changed"></param>
    public class ScaledTypeEventArgs(
        bool value_changed=false,
        bool scale_changed=false,
        Scale? old_scale=null,
        Scale? new_scale=null) :

        EventArgs
    {
        public readonly bool IsValueChanged = value_changed || !scale_changed;

        public readonly bool IsScaleChanged = scale_changed || !value_changed;

        public readonly Scale? OldScale = old_scale;

        public readonly Scale? NewScale = new_scale;

        public readonly bool IsEquivalentScale = (old_scale == new_scale);
    }
}
