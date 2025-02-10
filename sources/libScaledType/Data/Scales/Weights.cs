namespace As.Tools.Data.Scales
{
    public enum UnitWeight : ulong
    {
        kg = BaseUnit.Weight | BaseAttributes.Metric                                   | 0x1UL << 0,
         g = BaseUnit.Weight | BaseAttributes.Metric   | BaseAttributes.Scale10Base    | 0x1UL << 1,

        st = BaseUnit.Weight | BaseAttributes.Imperial | BaseAttributes.ScaleIrregular | 0x1UL << 2,
        lb = BaseUnit.Weight | BaseAttributes.Imperial | BaseAttributes.ScaleIrregular | 0x1UL << 3,
        oz = BaseUnit.Weight | BaseAttributes.Imperial | BaseAttributes.ScaleIrregular | 0x1UL << 4,
        dr = BaseUnit.Weight | BaseAttributes.Imperial | BaseAttributes.ScaleIrregular | 0x1UL << 5,
        gr = BaseUnit.Weight | BaseAttributes.Imperial | BaseAttributes.ScaleIrregular | 0x1UL << 6,
        //                                                                        last = 0x1UL << 47
        Mask =                                                                           0xffUL
    }

    static public class UnitWeightX
    {
#if USE_LOG4NET
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
#endif

        static readonly Unit BASE = Unit.g;

        static public double Factor(this UnitWeight me)
        {
            switch (me)
            {
                case UnitWeight.kg: return UnitWeight.g.Factor() / 1000.0;       // kilogram [kg],  1000 g = 1 kg
                case UnitWeight.g: return 1.0;                                  //      gram [g],     1 g = Standard unit of weight (mass)

                // international yard and pound agreemend 1959.
                case UnitWeight.st: return UnitWeight.lb.Factor() / 14.0;        //    stone [st],   14 lb = 1 st;
                case UnitWeight.lb: return UnitWeight.kg.Factor() / 0.45359237;  //    pound [lb], 1 (avoirdupois) pound is exactly 0.45359237 kilograms
                case UnitWeight.oz: return 16.0 * UnitWeight.lb.Factor();        //    ounce [oz],   16 oz = 1 lb
                case UnitWeight.dr: return 16.0 * 16.0 * UnitWeight.lb.Factor(); //     dram [dr],   16 dr = 1 oz
                case UnitWeight.gr: return 7000.0 * UnitWeight.lb.Factor();      //    grain [gr], 7000 gr = 1 lb
                default:
#if USE_LOG4NET
                    log.Debug($"Weight: type not recognised: '(UnitWeight)0x{(ulong)me:x16}'");
#endif
                    return 0.0;
            }
        }

        static public Unit Base(this UnitWeight me)
        {
            return BASE;
        }

        static public string ToString(this UnitWeight me, bool append_brackets)
        {
            switch (me)
            {
                case UnitWeight.kg: return (append_brackets) ? "[kg]" : "kg";
                case UnitWeight.g: return (append_brackets) ? "[g]" : "g";

                case UnitWeight.st: return (append_brackets) ? "[st]" : "st";
                case UnitWeight.lb: return (append_brackets) ? "[lb]" : "lb";
                case UnitWeight.oz: return (append_brackets) ? "[oz]" : "oz";
                case UnitWeight.dr: return (append_brackets) ? "[dr]" : "dr";
                case UnitWeight.gr: return (append_brackets) ? "[gr]" : "gr";
                default:
                    var msg = $"(UnitWeight)0x{(ulong)me:x16}";
#if USE_LOG4NET
                    log.Debug($"Weight: type not recognised: '{msg}'");
#endif
                    return (append_brackets) ? $"[{msg}]" : msg;
            }
        }

        static public string Description(this UnitWeight me)
        {
            switch (me)
            {
                case UnitWeight.kg: return "kilogram: 1000 [g] = 1 [kg]";
                case UnitWeight.g: return "gram: Standard unit of weight (mass), [g]";

                case UnitWeight.st: return "stone: 1 [st] = 14 [lb]";
                case UnitWeight.lb: return "(avoirdupois) pound: 1 [lb] = 0.45359237 [kg] (Int yard and pound agreemend 1959)";
                case UnitWeight.oz: return "ounce: 16 [oz] 1 [lb]";
                case UnitWeight.dr: return "dram: 256 [dr] = 16 [oz] = 1 [lb]";
                case UnitWeight.gr: return "grain 7000 [gr] = 1 [lb]";
                default:
                    var msg = $"(UnitWeight)0x{(ulong)me:x16}";
#if USE_LOG4NET
                    log.Debug($"Weight: type not recognised: '{msg}'");
#endif
                    return $"Weight: {msg}";
            }
        }

        static public bool TryParse(string value, out Unit unit)
        {
            switch (value.Trim())
            {
                case "kg": unit = Unit.kg; return true;
                case "g": unit = Unit.g; return true;

                case "st": unit = Unit.st; return true;
                case "lb": unit = Unit.lb; return true;
                case "oz": unit = Unit.oz; return true;
                case "dr": unit = Unit.dr; return true;
                case "gr": unit = Unit.gr; return true;
                default:   unit = BASE;    return false;
            }
        }
    }

    // example: grams to grains:
    // double x_g  = 100;
    // double x_gr = x_g * gr;
    //
    // check:
    // x_g  = 100[g]
    // x_gr = 100[g] * gr[gr/g]
    //      = 100[g] * ( (7000.0[gr]/1.0[lb]) * lb[lb/g] )
    //      = 100[g] * ( (7000.0[gr]/1.0[lb]) * 1[lb]/0.45359237[kg] * kg[kg/g] )
    //      = 100[g] * ( (7000.0[gr]/1.0[lb]) * 1[lb]/0.45359237[kg] * 1.0[kg]/1000.0[g] )
    //      = (100 * 7000.0) / (0.45359237 * 1000.0) [g][kg/g][lb/kg][gr/lb]
    //      = 1543.2358[gr]

    // example: stones to grams:
    // double x_st  = 1;
    // double x_g   = x_st / st;
    //
    // check:
    // x_st = 1[st]
    // x_g  = 1[st] / st[st/g]
    //      = 1[st] / ( (1[st] / 14.0[lb]) * lb[lb/g] )
    //      = 1[st] / ( (1[st] / 14.0[lb]) * ( 1[lb]/0.45359237[kg] * kg[kg/g] ))
    //      = 1[st] / ( (1[st] / 14.0[lb]) * ( 1[lb]/0.45359237[kg] * 1.0[kg]/1000.0[g] ))
    //      = 14.0 * 0.45359237 * 1000.0 [st][lb/st][kg/lb][g/kg]
    //      = 6350[g]
}
