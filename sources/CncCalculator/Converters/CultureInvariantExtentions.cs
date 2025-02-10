using System.Globalization;
using System.Numerics;

namespace As.Applications.Converters
{
    static internal class CultureInvariantExtentions
    {
        /// <summary>
        /// Convert int to string, culture invariant
        /// </summary>
        /// <param name="me">value to convert</param>
        /// <param name="format">output format, default to "G"</param>
        /// <param name="formatProvider">Format provider, default to CultureInfo.InvariantCulture</param>
        /// <returns>converted value</returns>
        static public string Ci(
            this int me,
            string? format = null,
            IFormatProvider? formatProvider = null)
            => me.ToString(
                format ?? "G",
                formatProvider ?? CultureInfo.InvariantCulture);

        /// <summary>
        /// Try convert string to int, culture invariant
        /// </summary>
        /// <param name="me">value to convert</param>
        /// <param name="value">Conversion result</param>
        /// <param name="formatProvider">Format provider, default to CultureInfo.InvariantCulture</param>
        /// <returns>True if value is valid, false otherwise</returns>
        static public bool Ci(
            this string me,
            out int value,
            IFormatProvider? formatProvider = null)
            => int.TryParse(
                me,
                formatProvider ?? CultureInfo.InvariantCulture,
                out value);

        /// <summary>
        /// Convert double to string, culture invariant
        /// </summary>
        /// <param name="me">value to convert</param>
        /// <param name="format">output format, default to "0.000"</param>
        /// <param name="formatProvider">Format provider, default to CultureInfo.InvariantCulture</param>
        /// <returns>converted value</returns>
        static public string Ci(
            this double me,
            string? format=null,
            IFormatProvider? formatProvider = null)
            => me.ToString(
                format ?? "0.###",
                formatProvider ?? CultureInfo.InvariantCulture);

        /// <summary>
        /// Try convert string to double, culture invariant
        /// </summary>
        /// <param name="me">value to convert</param>
        /// <param name="value">Conversion result</param>
        /// <param name="formatProvider">Format provider, default to CultureInfo.InvariantCulture</param>
        /// <returns>True if value is valid, false otherwise</returns>
        static public bool Ci(
            this string me,
            out double value,
            IFormatProvider? formatProvider = null)
            => double.TryParse(
                me,
                formatProvider ?? CultureInfo.InvariantCulture,
                out value);

        /// <summary>
        /// Convert int to string, culture invariant
        /// </summary>
        /// <param name="me">value to convert</param>
        /// <param name="format">output format, default to "G"</param>
        /// <param name="formatProvider">Format provider, default to CultureInfo.InvariantCulture</param>
        /// <returns>converted value</returns>
        static public string Ci<T>(
            this T me, string? format=null,
            IFormatProvider? formatProvider = null)
            where T : notnull
        {
            if (me is IFormattable value)
            {
                value.ToString(
                    format ?? "G",
                    formatProvider ?? CultureInfo.InvariantCulture);
            }
            return me.ToString() ?? $"{me.GetType()}";
        }

        /// <summary>
        /// Convert int to string, culture invariant
        /// </summary>
        /// <param name="me">value to convert</param>
        /// <param name="format">output format, default to "G"</param>
        /// <param name="formatProvider">Format provider, default to CultureInfo.InvariantCulture</param>
        /// <returns>converted value</returns>
        static public string CiNullable<T>(
            this T? me,
            string? format = null,
            IFormatProvider? formatProvider = null)
        {
            if (me == null) return "";
            if (me is IFormattable value)
            {
                value.ToString(
                    format ?? "G",
                    formatProvider ?? CultureInfo.InvariantCulture);
            }
            return me.ToString() ?? $"{me.GetType()}";
        }

        /// <summary>
        /// Convert string to T, culture invariant
        /// </summary>
        /// <param name="me">value to convert</param>
        /// <param name="formatProvider">Format provider, default to CultureInfo.InvariantCulture</param>
        /// <returns>converted value</returns>
        static public T Ci<T>(
            this string me,
            IFormatProvider? formatProvider = null)
            where T : notnull, INumber<T>
            => T.Parse(
                me,
                formatProvider ?? CultureInfo.InvariantCulture);

        /// <summary>
        /// Try convert string to T, culture invariant
        /// </summary>
        /// <param name="me">value to convert</param>
        /// <param name="value">Conversion result</param>
        /// <param name="formatProvider">Format provider, default to CultureInfo.InvariantCulture</param>
        /// <returns>True if value is valid, false otherwise</returns>
        static public bool TryCi<T>(
            this string me,
            out T value,
            IFormatProvider? formatProvider = null)
            where T : notnull, INumber<T>
        {
            var r = T.TryParse(
                me,
                formatProvider ?? CultureInfo.InvariantCulture,
                out T? v);
            value = (r) ? v! : default!;
            return r;
        }

        /// <summary>
        /// Convert string to T, culture invariant
        /// </summary>
        /// <param name="me">value to convert</param>
        /// <param name="formatProvider">Format provider, default to CultureInfo.InvariantCulture</param>
        /// <returns>converted value</returns>
        static public T? CiNullable<T>(
            this string? me,
            IFormatProvider? formatProvider = null)
            where T : IParsable<T>
        {
            if (me == null) return default;
            return T.Parse(
                me,
                formatProvider ?? CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Try convert string to T, culture invariant
        /// </summary>
        /// <param name="me">value to convert</param>
        /// <param name="result">Conversion result</param>
        /// <param name="formatProvider">Format provider, default to CultureInfo.InvariantCulture</param>
        /// <returns>True if value is valid, false otherwise</returns>
        static public bool TryCiNullable<T>(
            this string? me,
            out T? result,
            IFormatProvider? formatProvider = null)
            where T : IParsable<T>
        {
            if (me == null)
            {
                result = default;
                return false;
            }
            return T.TryParse(
                me,
                formatProvider ?? CultureInfo.InvariantCulture,
                out result);
        }
    }
}
