#if USE_SI_UNITS
using System.Numerics;

namespace As.Tools.Data.Scales
{
    // TODO: use SI units

    /// <summary>Defines a scaled number type.</summary>
    /// <typeparam name="TSelf">The type that implements the interface.</typeparam>
    public interface ISiScaledNumber<TSelf> :
        INumber<TSelf>,
        ISiScale
        where TSelf : ISiScaledNumber<TSelf>?
    {
        // IScaledNumber = (T value, S scale) where T : INumber, S : IScale
    }

    public interface ISiScale
    {
        // a scale is a product of powers of units.
        void Append(Scale scale, bool reciproce = false);
        void Append(ScaledUnit scale, bool reciproce = false);
        void Append(Unit unit, int exp = 1);
    }

    // units: https://en.wikipedia.org/wiki/International_System_of_Units
    enum SiUnitBase
    {
        /// <summary>
        /// second, time in [s]
        /// </summary>
        s,

        /// <summary>
        /// meter, length in [m]
        /// </summary>
        m,

        /// <summary>
        /// kilogram, weight in [kg]
        /// </summary>
        kg,

        /// <summary>
        /// ampere, electric current in [A]
        /// </summary>
        A,

        /// <summary>
        /// kelvin, thermodynamic temperature in [K]
        /// </summary>
        K,

        /// <summary>
        /// mole, amount of substance in [mol]
        /// </summary>
        mol,

        /// <summary>
        /// candela, luminous intencity in [cd]
        /// </summary>
        cd,
    }

    // see: https://en.wikipedia.org/wiki/SI_derived_unit
    enum SiUnitDerived // can always be represented as products of powers of the base units.
    {
        /// <summary>
        /// dimensionless constant: unitless in [x/x] where x is any base unit, not part of SI base or derived units.
        /// </summary>
        //c,

        /// <summary>
        /// hertz: frequency in [1/s]
        /// </summary>
        Hz = 1,

        /// <summary>
        /// radian: angle in [m/m]
        /// </summary>
        rad,

        /// <summary>
        /// steradian: solid angle in [m^2/m^2]
        /// </summary>
        sr,

        /// <summary>
        /// newton: force or weight in [kg m/s^2]
        /// </summary>
        N,

        /// <summary>
        /// pascal: preassure or stress in [kg / m s^2] or [N/m^]
        /// </summary>
        Pa,

        /// <summary>
        /// joule: energy, work or heat in [kg m^2/s^2] or [m N], [C V], [W s]
        /// </summary>
        J,

        /// <summary>
        /// watt: power, radiant flux in [kg m^2 / s^3] or [J/s], [V A]
        /// </summary>
        W,

        /// <summary>
        /// coulomb: electric charge in [A s] or [F V]
        /// </summary>
        C,

        /// <summary>
        /// volt: electrical potential dirrerence in [kg m^2 / s^3 A] or [W/A], [J/s]
        /// </summary>
        V,

        /// <summary>
        /// farad: electrical capacitance in [s^4 S^2 / kg⋅m^2] or [C/V], [s/Ω]
        /// </summary>
        F,

        /// <summary>
        /// ohm: electrical resistance in [kg⋅m^2 /⋅s^3⋅A^2] or [1/S], [V/A]
        /// </summary>
        Ω,

        /// <summary>
        /// ohm: electrical resistance in [kg⋅m^2 /⋅s^3⋅A^2] or [1/S], [V/A]
        /// </summary>
        Ohm = Ω,

        /// <summary>
        /// siemens: electrical conductance in [s^3⋅A^2 / kg⋅m^2] or [1/Ω], [A/V]
        /// </summary>
        S,

        /// <summary>
        /// weber: magnetical flux in [kg⋅m^2 /⋅s^2⋅A] or [J/A], [T⋅m^2] [V⋅s]
        /// </summary>
        Wb,

        /// <summary>
        /// tesla: magnetical induction in [kg / ⋅s^2⋅A] or [V⋅s / m^2], [Wb/m^2], [N / A⋅m]
        /// </summary>
        T,

        /// <summary>
        /// henry: electrical inductance in [kg⋅m2⋅/ s^2⋅A^2] or [V⋅s/A], [Ω⋅s], [Wb/A]
        /// </summary>
        H,

        /// <summary>
        /// °C: temperatur relative to 273.15 [K] in [K]
        /// </summary>
        //°C, not accepted by C#

        /// <summary>
        /// °C: temperatur relative to 273.15 [K] in [K]
        /// </summary>
        degC,

        /// <summary>
        /// lumen: luminous flux in [cd] or [cd⋅sr]
        /// </summary>
        lm,

        /// <summary>
        /// lux: illuminance in [cd⋅/ m^2] or [lm/m^2]
        /// </summary>
        lx,

        /// <summary>
        /// becquerel: radioactivity (decays per unit time) in [1/s]
        /// </summary>
        Bq,

        /// <summary>
        /// gray: absorbed dose (of ionizing radiation) in [m^2 /⋅s^2] or [J/kg]
        /// </summary>
        Gy,

        /// <summary>
        /// sievert: equivalent dose (of ionizing radiation) in [m^2⋅/ s^2] or [J/kg]
        /// </summary>
        Sv,

        /// <summary>
        /// katal: catalytic activity in [mol/s]
        /// </summary>
        kat
    }
}
#endif // USE_SI_UNITS
