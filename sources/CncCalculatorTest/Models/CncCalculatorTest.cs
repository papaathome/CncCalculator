using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        #endregion .ctor tests
    }
}
