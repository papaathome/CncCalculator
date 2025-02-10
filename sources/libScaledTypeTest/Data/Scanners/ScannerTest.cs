using As.Tools.Data.Scanners;

using static As.Tools.Data.Scanners.Scanner;

namespace As.Tools.Test.Data.Scanners
{
    public class ScannerTest : BaseTest
    {
        [SetUp]
        public void Setup()
        {
            // nothing to do.
        }

        [Test]
        public void Ctor_Default()
        {
            // prepare
            // nothing to do.

            // execute
            var result = new TestScanner();
            var result_position = result?.Position;

            // assert
            Assert.Multiple(() =>
            {
                if (result is null)
                {
                    Assert.Fail();
                }
                else
                {
                    Assert.That(result.GetState(), Is.EqualTo(ScannerState.NORMAL));
                    Assert.That(result.IncludeNewlines, Is.False);
                    Assert.That(result.ScanNewlines, Is.False);
                    Assert.That(result.ScanWhitespace, Is.False);
                    Assert.That(result.FileName, Is.Null);
                }

                if (result_position is null)
                {
                    Assert.Fail();
                }
                else
                {
                    Assert.That(result_position.FileName, Is.Null);
                    Assert.That(result_position.Line, Is.EqualTo(0));
                    Assert.That(result_position.Column, Is.EqualTo(0));
                    Assert.That(result_position.Offset, Is.EqualTo(0));
                }
            });
        }

        [Test]
        public void GetToken()
        {
            // prepare
            var scanner = new TestScanner();

            // execute
            var result = scanner.GetToken();

            // assert
            Assert.Multiple(() =>
            {
                if (result is null)
                {
                    Assert.Fail();
                }
                else
                {
                    Assert.That(result.ScannerState, Is.EqualTo(ScannerState.NORMAL));
                    Assert.That(result.Symbol, Is.EqualTo(TokenIdBase._EOT_));
                    Assert.That(result.Id, Is.EqualTo((int)TokenIdBase._EOT_));
                    Assert.That(result.Name, Is.EqualTo($"{TokenIdBase._EOT_}"));
                    Assert.That(result.Value, Is.Null);

                    var result_position = result.Position;
                    if (result_position is null)
                    {
                        Assert.Fail();
                    }
                    else
                    {
                        Assert.That(result_position.FileName, Is.Null);
                        Assert.That(result_position.Line, Is.EqualTo(0));
                        Assert.That(result_position.Column, Is.EqualTo(0));
                        Assert.That(result_position.Offset, Is.EqualTo(0));
                    }
                }
            });
        }

        [Test]
        public void GetToken2()
        {
            // prepare
            var scanner = new TestScanner();
            scanner.GetToken();

            // execute
            var result = scanner.GetToken();

            // assert
            Assert.Multiple(() =>
            {
                if (result is null)
                {
                    Assert.Fail();
                }
                else
                {
                    Assert.That(result.ScannerState, Is.EqualTo(ScannerState.NORMAL));
                    Assert.That(result.Symbol, Is.EqualTo(TokenIdBase._EOT_));
                    Assert.That(result.Id, Is.EqualTo((int)TokenIdBase._EOT_));
                    Assert.That(result.Name, Is.EqualTo($"{TokenIdBase._EOT_}"));
                    Assert.That(result.Value, Is.Null);

                    var result_position = result.Position;
                    if (result_position is null)
                    {
                        Assert.Fail();
                    }
                    else
                    {
                        Assert.That(result_position.FileName, Is.Null);
                        Assert.That(result_position.Line, Is.EqualTo(0));
                        Assert.That(result_position.Column, Is.EqualTo(0));
                        Assert.That(result_position.Offset, Is.EqualTo(0));
                    }
                }
            });
        }
    }
}