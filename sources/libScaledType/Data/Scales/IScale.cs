using System.Collections.Specialized;

namespace As.Tools.Data.Scales
{
    public interface IScaled
    {
        void Append(Scale scale, bool reciproce = false);
        void Append(ScaledUnit scale, bool reciproce = false);
        void Append(Unit unit, int exp = 1, bool reciproce = false);
    }

    public interface IScale :
        ICollection<ScaledUnit>,
        INotifyCollectionChanged,
        ICloneable,
        IScaled
    {
        string ToString(bool with_brackers = true);
    }
}