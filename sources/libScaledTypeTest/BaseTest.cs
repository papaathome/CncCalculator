namespace As.Tools.Test
{
    public class BaseTest
    {
        /// <summary>
        /// Relative tolerance check (X, Y) -> check |Y(1 - relative_tolerance)| .LE. |X| .LE. |Y(1 + relative_tolerance)| 
        /// </summary>
        protected const double DefaultRelativeFloatingPointTolerance = 1e-15;

        /// <summary>
        /// Check exception type (or null), do not check exception data.
        /// </summary>
        /// <param name="result">exception value to type check</param>
        /// <param name="expected">exception value to type check against</param>
        protected void AssertExceptionType(
            Exception? result,
            Exception? expected)
        {
            if ((result is null) && (expected is null)) return;
            if (expected is not null)
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result?.GetType(), Is.EqualTo(expected.GetType()));
            }
            else
            {
                Assert.That(result, Is.Null);
            }
        }

        /// <summary>
        /// Relative tolerance check (X, Y) -> check |Y(1 - relative_tolerance)| .LE. |X| .LE. |Y(1 + relative_tolerance)| 
        /// </summary>
        /// <param name="value">X, value to check</param>
        /// <param name="expected">Y, value to check against</param>
        /// <param name="relative_tolerance">mismatch fraction allowed</param>
        protected void AssertFloatIsNear(
            double value,
            double expected,
            double relative_tolerance = DefaultRelativeFloatingPointTolerance)
            => Assert.That(value, Is.InRange(expected * (1 - relative_tolerance), expected * (1 + relative_tolerance)));
    }
}
