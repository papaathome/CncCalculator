using As.Applications.Models;
using As.Applications.ViewModels;

namespace As.Applications.Test.ViewModels
{
    public class FeedAndSpeedViewModelTest : BaseTest
    {
        // Note: FeedAndSpeedViewModel does not implement IEqualable<T> -> Assert.That(x, IsEqualTo(y)) is checking reference equality.

        [SetUp]
        public void Setup() { }

        #region .ctor tests
        [Test]
        public void Ctor_Default()
        {
            // prepare
            Exception? e_expected = null;
            bool primary_expected = true;

            // execute
            Exception? e = null;
            FeedAndSpeedViewModel? result = null;
            try
            {
                result = new FeedAndSpeedViewModel();
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
                    Assert.That(result.Tools, Is.Not.Null);
                    Assert.That(result.ToolSelected, Is.Not.Null);

                    Assert.That(result.Materials, Is.Not.Null);
                    Assert.That(result.MaterialSelected, Is.Not.Null);

                    Assert.That(result.CuttingDepth, Is.Not.Null);
                    Assert.That(result.CuttingDepth.Name, Is.EqualTo("h"));
                    Assert.That(result.CuttingDepth.ValueIsReadOnly, Is.Not.True);
                    Assert.That(result.CuttingDepth.Domain, Is.EqualTo(Domain.Lengths));


                    Assert.That(result.Diameter, Is.Not.Null);
                    Assert.That(result.Diameter.Name, Is.EqualTo("D"));
                    Assert.That(result.Diameter.ValueIsReadOnly, Is.Not.True);
                    Assert.That(result.Diameter.Domain, Is.EqualTo(Domain.Lengths));

                    Assert.That(result.Flutes, Is.Not.Null);
                    Assert.That(result.Flutes.Name, Is.EqualTo("Z"));
                    Assert.That(result.Flutes.ValueIsReadOnly, Is.Not.True);
                    Assert.That(result.Flutes.Domain, Is.EqualTo(Domain.Counts));

                    Assert.That(result.MaterialCuttingSpeed, Is.Not.Null);
                    Assert.That(result.MaterialCuttingSpeed.Name, Is.EqualTo("Vc"));
                    Assert.That(result.IsMaterialCuttingSpeedPrimary, Is.EqualTo(primary_expected));
                    Assert.That(result.MaterialCuttingSpeed.ValueIsReadOnly, Is.Not.EqualTo(primary_expected));
                    Assert.That(result.MaterialCuttingSpeed.Domain, Is.EqualTo(Domain.Speeds));

                    Assert.That(result.MaterialSpindleSpeed, Is.Not.Null);
                    Assert.That(result.MaterialSpindleSpeed.Name, Is.EqualTo("n"));
                    Assert.That(result.IsMaterialSpindleSpeedPrimary, Is.Not.EqualTo(primary_expected));
                    Assert.That(result.MaterialSpindleSpeed.ValueIsReadOnly, Is.EqualTo(primary_expected));
                    Assert.That(result.MaterialSpindleSpeed.Domain, Is.EqualTo(Domain.Rotations));

                    Assert.That(result.MaterialFeedPerTooth, Is.Not.Null);
                    Assert.That(result.MaterialFeedPerTooth.Name, Is.EqualTo("fz"));
                    Assert.That(result.MaterialFeedPerTooth.ValueIsReadOnly, Is.Not.True);
                    Assert.That(result.MaterialFeedPerTooth.Domain, Is.EqualTo(Domain.LengthsOverCounts));

                    Assert.That(result.CuttingSpeed, Is.Not.Null);
                    Assert.That(result.CuttingSpeed.Name, Is.EqualTo("Vc"));
                    Assert.That(result.CuttingSpeed.ValueIsReadOnly, Is.True);
                    Assert.That(result.CuttingSpeed.Domain, Is.EqualTo(Domain.Speeds));

                    Assert.That(result.SpindleSpeed, Is.Not.Null);
                    Assert.That(result.SpindleSpeed.Name, Is.EqualTo("n"));
                    Assert.That(result.SpindleSpeed.ValueIsReadOnly, Is.True);
                    Assert.That(result.SpindleSpeed.Domain, Is.EqualTo(Domain.Rotations));

                    Assert.That(result.Feed, Is.Not.Null);
                    Assert.That(result.Feed.Name, Is.EqualTo("Vf"));
                    Assert.That(result.Feed.ValueIsReadOnly, Is.True);
                    Assert.That(result.Feed.Domain, Is.EqualTo(Domain.Speeds));
                }
            });
        }
        #endregion .ctor tests

        // TODO: Properties and Fields
        // ToolSelected
        // MaterialSelected
        // IsMaterialCuttingSpeedPrimary
        // IsMaterialSpindleSpeedPrimary

        // TODO: Menu actions
        // ICommand_LoadToolsFile
        // ICommand_LoadMaterialsFile
        // ICommand_Exit
    }
}
