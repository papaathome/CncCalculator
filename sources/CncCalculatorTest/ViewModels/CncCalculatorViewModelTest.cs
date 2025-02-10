using System.Collections.ObjectModel;

using As.Applications.ViewModels;

namespace As.Applications.Test.ViewModels
{
    public class CncCalculatorViewModelTest : BaseTest
    {
        // Note: CncCalculatorViewModel does not implement IEqualable<T> -> Assert.That(x, IsEqualTo(y)) is checking reference equality.

        const string FIRST = "[mm]";
        const string SECOND = "[cm]";

        [SetUp]
        public void Setup() { }

        static readonly ReadOnlyCollection<string> Lengths = new([FIRST, SECOND, "[in]", "[ft]"]);

        #region .ctor tests
        [Test]
        public void Ctor_Default()
        {
            // prepare
            Exception? e_expected = null;

            // execute
            Exception? e = null;
            CncCalculatorViewModel? result = null;
            try
            {
                result = new CncCalculatorViewModel();
            }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                AssertExceptionType(e, e_expected);
                if (result == null)
                {
                    Assert.Fail("result is null");
                }
                else
                {
                    Assert.That(result.FeedAndSpeed, Is.Not.Null);
                    Assert.That(result.Converter, Is.Not.Null);
                }
            });
        }
        #endregion .ctor tests
    }
}
