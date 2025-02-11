namespace As.Tools.Data.Scales
{
    public enum UnitLength : ulong
    {
           m = BaseUnit.Length | BaseAttributes.Metric   | BaseAttributes.Scale10Base    | 0x1UL << 0,
          dm = BaseUnit.Length | BaseAttributes.Metric                                   | 0x1UL << 1,
          cm = BaseUnit.Length | BaseAttributes.Metric                                   | 0x1UL << 2,
          mm = BaseUnit.Length | BaseAttributes.Metric                                   | 0x1UL << 3,

        mile = BaseUnit.Length | BaseAttributes.Imperial | BaseAttributes.ScaleIrregular | 0x1UL << 4,
          yd = BaseUnit.Length | BaseAttributes.Imperial | BaseAttributes.ScaleIrregular | 0x1UL << 5,
          ft = BaseUnit.Length | BaseAttributes.Imperial | BaseAttributes.ScaleIrregular | 0x1UL << 6,
        inch = BaseUnit.Length | BaseAttributes.Imperial | BaseAttributes.ScaleIrregular | 0x1UL << 7,
        //                                                                          last = 0x1UL << 47
        Mask =                                                                             0xffUL
    }

    static public class UnitLengthX
    {
        static readonly Unit BASE = Unit.m;

        static public double Factor(this UnitLength me)
        {
            switch (me)
            {
                case UnitLength. m: return 1.0;                                       //      meter [m],           1 m = Standard unit of length
                case UnitLength.dm: return 10.0 * UnitLength.m.Factor();              //  decimeter [dm/m],        1 m = 10 dm
                case UnitLength.cm: return 100.0 * UnitLength.m.Factor();             // centimeter [cm/m],        1 m = 100 cm
                case UnitLength.mm: return 1000.0 * UnitLength.m.Factor();            //  milimeter [mm/m],        1 m = 1000 mm

                // international yard and pound agreemend 1959.
                case UnitLength.mile: return UnitLength.yd.Factor() / 1760.0;           //       mile [mile/yd], 1760 yd = 1 mile
                case UnitLength.  yd: return (1250.0 / 1143.0) * UnitLength.m.Factor(); //       yard [yd/m],    1250 (international) yards = 1143 meters
                case UnitLength.  ft: return 3.0 * UnitLength.yd.Factor();              //       foot [ft/yd],      3 ft = 1 yd
                case UnitLength.inch: return 36.0 * UnitLength.yd.Factor();             //       inch [in/yd],     36 in = 1 yd
                default: return 0.0;
            }
        }

        static public Unit Base(this UnitLength me)
        {
            return BASE;
        }

        static public string ToString(this UnitLength me, bool append_brackets)
        {
            switch (me)
            {
                case UnitLength.   m: return (append_brackets) ? "[m]" : "m";
                case UnitLength.  dm: return (append_brackets) ? "[dm]" : "dm";
                case UnitLength.  cm: return (append_brackets) ? "[cm]" : "cm";
                case UnitLength.  mm: return (append_brackets) ? "[mm]" : "mm";

                case UnitLength.mile: return (append_brackets) ? "[mile]" : "mile";
                case UnitLength.  yd: return (append_brackets) ? "[yd]" : "yd";
                case UnitLength.  ft: return (append_brackets) ? "[ft]" : "ft";
                case UnitLength.inch: return (append_brackets) ? "[in]" : "in";
                default:
                    var msg = $"(UnitLength)0x{(ulong)me:x16}";
                    return (append_brackets) ? $"[{msg}]" : msg;
            }
        }

        static public string Description(this UnitLength me)
        {
            switch (me)
            {
                case UnitLength.   m: return "meter: Standard unit of length, [m]";
                case UnitLength.  dm: return "decimeter: 1 [m] = 10 [dm]";
                case UnitLength.  cm: return "centimeter: 1 [m] = 100 [cm]";
                case UnitLength.  mm: return "milimeter: 1 [m] = 1000 [mm]";

                case UnitLength.mile: return "mile: 1760 [yd] = 1 [mile]";
                case UnitLength.  yd: return "(international) yard: 1250 [yd] = 1143 [m] (Int yard and pound agreemend 1959)";
                case UnitLength.  ft: return "foot: 3 [ft] = 1 [yd]";
                case UnitLength.inch: return "inch:  36 [in] = 1 [yd]";
                default:
                    var msg = $"(UnitLength)0x{(ulong)me:x16}";
                    return $"Length: [{msg}]";
                    }
        }

        static public bool TryParse(string value, out Unit unit)
        {
            switch (value.Trim())
            {
                case "m":    unit = Unit.m;    return true;
                case "dm":   unit = Unit.dm;   return true;
                case "cm":   unit = Unit.cm;   return true;
                case "mm":   unit = Unit.mm;   return true;

                case "mile": unit = Unit.mile; return true;
                case "yd":   unit = Unit.yd;   return true;
                case "ft":   unit = Unit.ft;   return true;
                case "in":   unit = Unit.inch; return true;
                default:     unit = BASE;      return false;
            }
        }
    }

    // example meters to inches:
    // double x_m  = 1;            // length in meters.
    // double x_in = y_m * inches; // length in inches.
    //
    // check:
    // x_m  = 1[m]
    // x_in = x_m * inches[in/m];
    //      = 1[m] * ( 36.0[in/yd] * yd[yd/m] )                 # substitute yd for inches
    //      = 1[m] * ( 36.0[in/yd] * (1250.0[yd]/1143.0[m]))    # substitute m for yd
    //      = 36.0 * (1250.0 / 1143.0)[m][yd/m][in/yd]          # reorder
    //      = 39.370[in]                                        # reduced

    // example foot to meters:
    // double y_ft = 3;         // _value in foot.
    // double y_m  = y_ft / ft; // _value in meters.
    //
    // check:
    // y_ft = 3[ft]
    // y_m  = y_ft/ft[ft/m]
    //      = 3[ft]/( 3.0[ft]/1[yd] * yd[yd/m] )                # substitute yd for ft
    //      = 3[ft]/( 3.0[ft]/1[yd] * (1250.0[yd]/1143.0[m]) )  # substitute m for yd
    //      = 3/3.0 * (1143.0/1250.0) [ft][yd/ft][m/yd]         # reorder
    //      = 0.9144[m]                                         # reduced
}
