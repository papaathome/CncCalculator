using System.Globalization;
using System.Numerics;

namespace As.Tools.Data.Scales
{
    public interface IOnChanged
    {
        event ScaledTypeEvent? OnScaleChanged;
        event ScaledTypeEvent? OnValueChanged;
        event ScaledTypeEvent? OnChanged;
    }

    public interface IScaledType<TSelf, TBase> :
        ICloneable,
        IScaled,
        IOnChanged
        where TBase : INumber<TBase>
        where TSelf : IScaledType<TSelf, TBase>
    {

        TBase Value { get; set; }

        void Assign(TSelf other);
        bool SetValue(IScaledType<TSelf, TBase> value);
        bool SetValueScaled(IScaledType<TSelf, TBase> value);
        bool SetValue(string value, CultureInfo? culture = null);

        Scale Scale { get; set; }
        bool TrySetScale(string scale, out string err);

        void Append(
            IScaledType<TSelf, TBase> other,
            bool reciproce = false);
    }
}