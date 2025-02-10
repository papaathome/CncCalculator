namespace As.Tools.Data.Scales
{
    // all base units used are from the metric mgs system (meters, grams, seconds).

    // On use of the constants for dimentions.
    // Each dimention has a base unit for met mgs system.
    // All calculations are normalised to base units.
    //
    // The constant is an expression of 1 [base unit] = c [named_unit]
    // e.g.: 1 [m] = 10 [cm] -> the constant cm expresses that there are 10 cm in 1 m.
    //
    // Another way to look at it is that the constant x is itself in [x/base_x] units and
    // gives how many times it fits in the base units or 'x units per base unit'
    // Then the constant cm is defined as 10 [cm / m].
    // e.g.: 1 * cm = 1[m] * cm[cm/m]
    //              = 10 [m*cm/m]
    //              = 10[cm]
    //
    // Any expression of _value * units will give a result in base units,
    // be sure that your initial _value is in the used units.
    //
    // Any expression of vaule / units will give a result in the units,
    // be sure that your inital _value in in base units.
    //

    /// <summary>
    /// Groups of Units supported.
    /// </summary>
    public enum BaseUnit : ulong
    {
        /// <summary>
        /// Constant: representing the dimentionless proportions used.
        /// </summary>
        Constant =  0,

        /// <summary>
        /// Length: metric and US or imperial system length units.
        /// </summary>
        Length   = (0x1UL << 0) << 56,

        /// <summary>
        /// Weight: metric and US or imperial system weight units.
        /// </summary>
        Weight = (0x1UL << 2) << 56,

        /// <summary>
        /// Time: metric and standard time units.
        /// </summary>
        Time = (0x1UL << 1) << 56,

        /// <summary>
        /// Rotation/Time: Time related units for rotations.
        /// </summary>
        Rotation = (0x1UL << 3) << 56,

        // last  = (0x1UL << 7) << 56

        /// <summary>
        /// Mask for the BaseUnit information.
        /// </summary>
        Mask = 0xffUL << 56
    }

    /// <summary>
    /// List of possible attributes with a unit.
    /// </summary>
    public enum BaseAttributes : ulong
    {
        /// <summary>
        /// Unit is scalable in factors of 10
        /// </summary>
        Scale10Base    = (0x1UL << 0) << 48,

        /// <summary>
        /// Unit is scalable with unrgular or not on 10 based factors.
        /// </summary>
        ScaleIrregular = (0x1UL << 1) << 48,

        /// <summary>
        /// The unit is a member of the metric system.
        /// </summary>
        Metric = (0x1UL << 2) << 48,

        /// <summary>
        /// The unit is a member of the US or imperial system.
        /// </summary>
        Imperial = (0x1UL << 3) << 48,

        // last = (0x1UL << 7) << 48

        /// <summary>
        /// Mask for the BaseAttribute information
        /// </summary>
        Mask =  0xffUL  << 48
    }

    /// <summary>
    /// List of all supported units, size with a unit factor in resprect to a standard unit.
    /// </summary>
    public enum Unit : ulong
    {
        /// <summary>
        /// Constant: no unit name or dimention, standard factor 1.0
        /// </summary>
        c = UnitConstant.c,

        /// <summary>
        /// Constant: named, no dimention, standard factor 1.0
        /// </summary>
        tooth = UnitConstant.tooth,

        /// <summary>
        /// Length: meter [m], standard unit.
        /// </summary>
        m = UnitLength.m,

        /// <summary>
        /// Length: decimeter [dm] with a factor agains the meter [m]
        /// </summary>
        dm = UnitLength.dm,

        /// <summary>
        /// Length: centimeter [cm] with a factor against the meter [m]
        /// </summary>
        cm = UnitLength.cm,

        /// <summary>
        /// Length: milimeter [mm] with a factor againt the meter [m]
        /// </summary>
        mm = UnitLength.mm,

        /// <summary>
        /// Length: mile [mile] with a factor againg the yard [yd]
        /// </summary>
        mile = UnitLength.mile,

        /// <summary>
        /// Length: yard [yd] with a factor against the meter [m]
        /// </summary>
        yd = UnitLength.yd,

        /// <summary>
        /// Length: foot [ft] with a factor against the yard [yd]
        /// </summary>
        ft = UnitLength.ft,

        /// <summary>
        /// Length: inch [in] with a factor against the yard [yd]
        /// </summary>
        inch = UnitLength.inch,

        /// <summary>
        /// Weight: kilogram [kg] with a factor against the gram [g]
        /// </summary>
        kg = UnitWeight.kg,

        /// <summary>
        /// Weight: gram [g] standard unit (for mass).
        /// </summary>
        g = UnitWeight.g,

        /// <summary>
        /// Weight: stone [st] with a factor against the pound [lb]
        /// </summary>
        st = UnitWeight.st,

        /// <summary>
        /// Weight: pound [lb] with a factor against the kilogram [kg]
        /// </summary>
        lb = UnitWeight.lb,

        /// <summary>
        /// Weight: ounce [oz] with a factor against the pound [lb]
        /// </summary>
        oz = UnitWeight.oz,

        /// <summary>
        /// Weight: dram [dr] with a factor against the pound [lb]
        /// </summary>
        dr = UnitWeight.dr,

        /// <summary>
        /// Weight: grain [gr] with a factor against the pound [lb]
        /// </summary>
        gr = UnitWeight.gr,

        /// <summary>
        /// Time: second [s] standard unit.
        /// </summary>
        s = UnitTime.s,

        /// <summary>
        /// Time: minute [min] with a factor against the second [s]
        /// </summary>
        min = UnitTime.min,

        /// <summary>
        /// Time: hour [h] with a factor against the second [s]
        /// </summary>
        h = UnitTime.h,

        /// <summary>
        /// Rotation/Time: rotation per second [rpm] with a factor against the second [1/s]
        /// </summary>
        rps = UnitRotation.rps,

        /// <summary>
        /// Rotation/Time: rotation per minute [rpm] with a factor against the second [1/s]
        /// </summary>
        rpm = UnitRotation.rpm,

        /// <summary>
        /// Rotation/Time: rotation per hour [rph] with a factor against the second [1/s]
        /// </summary>
        rph = UnitRotation.rph
    }

    /// <summary>
    /// Extening the data in enum Unit.
    /// </summary>
    public static class UnitX
    {
#if USE_LOG4NET
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
#endif

        /// <summary>
        /// Factor against the base unit (for US or imperial units firt to a base unit in the US or imperial unit system)
        /// </summary>
        /// <param name="me">Unit type</param>
        /// <returns>Factor [Unit type : Unit base type]</returns>
        static public double Factor(this Unit me)
        {
            switch ( (BaseUnit)(((ulong)me) & (ulong)BaseUnit.Mask) )
            {
                case BaseUnit.Constant: return ((UnitConstant)me).Factor();
                case BaseUnit.Length:   return ((UnitLength)me).Factor();
                case BaseUnit.Weight:   return ((UnitWeight)me).Factor();
                case BaseUnit.Time:     return ((UnitTime)me).Factor();
                case BaseUnit.Rotation: return ((UnitRotation)me).Factor();
                default:
#if USE_LOG4NET
                    log.Debug($"Unit: type not recognised: '(Unit)0x{(ulong)me:x16}'");
#endif
                    return 0.0;
            }
        }

        /// <summary>
        /// Unit that forms the base for a group, a reference unit.
        /// </summary>
        /// <param name="me">Unit type</param>
        /// <returns>Base unit for the unit type</returns>
        static public Unit Base(this Unit me)
        {
            switch ((BaseUnit)(((ulong)me) & (ulong)BaseUnit.Mask))
            {
                case BaseUnit.Constant: return ((UnitConstant)me).Base();
                case BaseUnit.Length: return ((UnitLength)me).Base();
                case BaseUnit.Weight: return ((UnitWeight)me).Base();
                case BaseUnit.Time: return ((UnitTime)me).Base();
                case BaseUnit.Rotation: return ((UnitRotation)me).Base();
                default:
#if USE_LOG4NET
                    log.Debug($"Unit: type not recognised: '(Unit)0x{(ulong)me:x16}'");
#endif
                    throw new ArgumentException("Unit not recognised.", me.ToString());
            }
        }

        /// <summary>
        /// Conventional notation of the Unit type
        /// </summary>
        /// <param name="me">Unit type</param>
        /// <returns>Human readable representation of the Unit type</returns>
        static public string ToString(this Unit me, bool append_brackets)
        {
            switch ((BaseUnit)((ulong)me & (ulong)BaseUnit.Mask))
            {
                case BaseUnit.Constant: return ((UnitConstant)me).ToString(append_brackets);
                case BaseUnit.Length: return ((UnitLength)me).ToString(append_brackets);
                case BaseUnit.Weight: return ((UnitWeight)me).ToString(append_brackets);
                case BaseUnit.Time: return ((UnitTime)me).ToString(append_brackets);
                case BaseUnit.Rotation: return ((UnitRotation)me).ToString(append_brackets);
                default:
                    var msg = $"(Unit)0x{(ulong)me:x16}";
#if USE_LOG4NET
                    log.Debug($"Unit: type not recognised: '{msg}'");
#endif
                    return (append_brackets) ? $"[{msg}]" : msg;
            }
        }

        /// <summary>
        /// Short description of the Unit type relation against the Unit base type
        /// </summary>
        /// <param name="me">Unit type</param>
        /// <returns>Short description of the Unit type</returns>
        static public string Description(this Unit me)
        {
            switch ((BaseUnit)(((ulong)me) & (ulong)BaseUnit.Mask))
            {
                case BaseUnit.Constant: return ((UnitConstant)me).Description();
                case BaseUnit.Length: return ((UnitLength)me).Description();
                case BaseUnit.Weight: return ((UnitWeight)me).Description();
                case BaseUnit.Time: return ((UnitTime)me).Description();
                case BaseUnit.Rotation: return ((UnitRotation)me).Description();
                default:
                    var msg = $"(Unit)0x{(ulong)me:x8}";
#if USE_LOG4NET
                    log.Debug($"Unit: type not recognised: '{msg}'");
#endif
                    return $"Unit: {msg}";
            }
        }
    }
}
