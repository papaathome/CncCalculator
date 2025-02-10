using As.Applications.ViewModels;

namespace As.Applications.Test.ViewModels
{
    public class ConverterViewModelTest : BaseTest
    {
        // Note: ConverterViewModel does not implement IEqualable<T> -> Assert.That(x, IsEqualTo(y)) is checking reference equality.

        [SetUp]
        public void Setup() { }

        #region .ctor tests
        [Test]
        public void Ctor_Default()
        {
            // prepare
            Exception? e_expected = null;

            // execute
            Exception? e = null;
            ConverterViewModel? result = null;
            try
            {
                result = new ConverterViewModel();
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
                    Assert.That(result.X1, Is.Not.Null);
                    Assert.That(result.X2, Is.Not.Null);
                    Assert.That(result.X3, Is.Not.Null);
                    Assert.That(result.Y1, Is.Not.Null);
                    Assert.That(result.Y2, Is.Not.Null);
                    Assert.That(result.Y3, Is.Not.Null);
                }
            });
        }
        #endregion .ctor tests
    }
}
