using System.Collections.ObjectModel;

namespace As.Applications.Test.Models
{
    public class CncCalculatorTest : BaseTest
    {
        // Note: CncCalculator does not implement IEqualable<T> -> Assert.That(x, IsEqualTo(y)) is checking reference equality.

        const string FIRST = "[mm]";
        const string SECOND = "[cm]";

        [SetUp]
        public void Setup() { }

        static readonly ReadOnlyCollection<string> Lengths = new([FIRST, SECOND, "[in]", "[ft]"]);

        #region .ctor tests
        // TODO
        #endregion .ctor tests
    }
}
