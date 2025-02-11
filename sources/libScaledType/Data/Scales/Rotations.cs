namespace As.Tools.Data.Scales
{
    public enum UnitRotation : ulong
    {
        rps = BaseUnit.Rotation | BaseAttributes.Metric         | BaseAttributes.ScaleIrregular | 0x1UL << 0,
        rpm = BaseUnit.Rotation | BaseAttributes.ScaleIrregular                                 | 0x1UL << 1,
        rph = BaseUnit.Rotation | BaseAttributes.ScaleIrregular                                 | 0x1UL << 2,
        //                                                                                 last = 0x1UL << 47
        Mask = 0xffUL
    }

    public static class UnitRotationX
    {
        static readonly Unit BASE = Unit.rps;

        static public double Factor(this UnitRotation me)
        {
            switch (me)
            {
                case UnitRotation.rps: return 1.0 / UnitTime.s.Factor();   // rotation per second [rps, 1/s, Hz],  1 rps = 1 Hz.
                case UnitRotation.rpm: return 1.0 / UnitTime.min.Factor(); // rotation per minute [rpm],          60 rpm = 1 rps
                case UnitRotation.rph: return 1.0 / UnitTime.h.Factor();   //   rotation per hour [rph],          60 rph = 1 rpm
                default: return 0.0;
            }
        }

        static public Unit Base(this UnitRotation me)
        {
            return BASE;
        }

        static public string ToString(this UnitRotation me, bool append_brackets)
        {
            switch (me)
            {
                case UnitRotation.rps: return (append_brackets) ? "[rps]" : "rps";
                case UnitRotation.rpm: return (append_brackets) ? "[rpm]" : "rpm";
                case UnitRotation.rph: return (append_brackets) ? "[rph]" : "rph";
                default:
                    var msg = $"(UnitRotation)0x{(ulong)me:x16}";
                    return (append_brackets) ? $"{msg}]" : msg;
            }
        }

        static public string Description(this UnitRotation me)
        {
            switch (me)
            {
                case UnitRotation.rps: return "Rotations per second: [rmp], [1/s]";
                case UnitRotation.rpm: return "Rotations per minute: [rmm], [1/min]";
                case UnitRotation.rph: return "Rotations per hour: [rmh], [1/h]";
            }
            var msg = $"(UnitRotation)0x{(ulong)me:x16}";
            return $"Rotation over time: [{msg}]";
        }

        static public bool TryParse(string value, out Unit unit)
        {
            switch (value.Trim())
            {
                case "rps": unit = Unit.rps; return true;
                case "rpm": unit = Unit.rpm; return true;
                case "rph": unit = Unit.rph; return true;
            }
            unit = BASE;
            return false;
        }
    }

    // example rps to rpm
    // double x_rps = 120;
    // double x_rpm = x_rps * rpm;
    //
    // check:
    // x_rps = 2[rps]
    // x_rpm = 2[rps] * rpm[rpm/rps]
    //       = 2[rps] * ( 1.0 / UnitTime.min['/"] )
    //       = 2[rps] * ( 1.0 / ( 1[']/60["] )
    //       = 2[rps] * ( 1.0 / ( 1[']/60["] )
    //       = 2*60 [rps][(1/') "]
    //       = 2*60 [rps][rpm (1/rps)]
    //       = 120[rpm]

    // example rph to rps
    // double x_rph = 3600;
    // double x_rps = 3600 / rph;
    //
    // check:
    // x_rph = 3600[rph]
    // x_rps = 3600[rph] / rph[rph/rps]
    //       = 3600[rph] / ( 1.0 / UnitTime.h[h/"] )
    //       = 3600[rph] * UnitTime.h[h/"]
    //       = 3600[rph] * 1[h] / (60.0 * 60.0)["]
    //       = 3600/(60.0 * 60.0) [rph][h (1/")]
    //       = 3600/(60.0 * 60.0) [rph][(1/rph) rps]
    //       = 1[rps]
}
