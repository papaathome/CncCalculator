namespace As.Tools.Data.Scales
{
    public enum UnitTime : ulong
    {
        s   = BaseUnit.Time | BaseAttributes.Metric         | BaseAttributes.Scale10Base | 0x1UL << 0,
        min = BaseUnit.Time | BaseAttributes.ScaleIrregular |                              0x1UL << 1,
        h   = BaseUnit.Time | BaseAttributes.ScaleIrregular |                              0x1UL << 2,
        //                                                                          last = 0x1UL << 47
        Mask =                                                                             0xffUL
    }

    public static class UnitTimeX
    {
        static readonly Unit BASE = Unit.s;

        static public double Factor(this UnitTime me)
        {
            switch (me)
            {
                case UnitTime.s:   return 1.0;                                 // second [s, "],    1" = Standard unit of time
                case UnitTime.min: return UnitTime.s.Factor() / 60.0;          // minute [min, '], 60" = 1'
                case UnitTime.h:   return UnitTime.s.Factor() / (60.0 * 60.0); //   hour [h],      60' = 1 h
                default: return 0.0;
            }
        }

        static public Unit Base(this UnitTime me)
        {
            return BASE;
        }

        static public string ToString(this UnitTime me, bool append_brackets)
        {
            switch (me)
            {
                case UnitTime.  s: return (append_brackets) ? "[s]" : "s";
                case UnitTime.min: return (append_brackets) ? "[min]" : "min";
                case UnitTime.  h: return (append_brackets) ? "[h]" : "h";
                default:
                    var msg = $"(UnitTime)0x{(ulong)me:x16}";
                    return (append_brackets) ? $"[{msg}]" : msg;
                    }
        }

        static public string Description(this UnitTime me)
        {
            switch (me)
            {
                case UnitTime.  s: return "second: Standard unit of time, [s]";
                case UnitTime.min: return "minute: 1 [min] = 60 [s]";
                case UnitTime.  h: return "hour: 1 [h] = 3600 [s]";
            }
            var msg = $"(UnitTime)0x{(ulong)me:x16}";
            return $"Time [{msg}]";
        }

        static public bool TryParse(string value, out Unit unit)
        {
            switch (value.Trim())
            {
                case   "s": unit = Unit.s; return true;
                case "min": unit = Unit.min; return true;
                case   "h": unit = Unit.h; return true;
            }
            unit = BASE;
            return false;
        }
    }

    // example: seconds to minutes:
    // double x_s   = 120;
    // double x_min = x_s * min;
    //
    // check:
    // x_s = 120["]
    // x_m = 120["] * min['/"]
    //     = 120["] * (1[']/60["])
    //     = 120/60 ["]['/"]
    //     = 2[']

    // example: hour to seconds:
    // double x_h = 24;
    // double x_s = x_h / h;
    //
    // check:
    // x_h = 24[h]
    // x_s = 24[h] / h[h/"]
    //     = 24[h] / ( 1[h] / (60.0 * 60.0)["] )
    //     = 24*60.0*60.0[h]/[h/"]
    //     = 86400["]
}
