using As.Applications.Models;
using As.Tools.Data.Scales;

namespace As.Applications.Test.Models
{
    public class ConverterTest : BaseTest
    {
        // Note: Converter does not implement IEqualable<T> -> Assert.That(x, IsEqualTo(y)) is checking reference equality.

        [SetUp]
        public void Setup() { }

        void AssertIsNear(
            ScaledType<double> value,
            ScaledType<double> expected,
            double relative_tolerance = DefaultRelativeFloatingPointTolerance)
        {
            AssertFloatIsNear(value.Value, expected.Value, relative_tolerance);
            Assert.That(value.Scale, Is.EqualTo(expected.Scale));
        }

        #region .ctor tests
        [Test]
        public void Ctor_Default()
        {
            // prepare
            Exception? e_expected = null;

            // execute
            Exception? e = null;
            Converter? result = null;
            try
            {
                result = new Converter();
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
                    Assert.That(result.XScales, Is.EqualTo(Domain.Lengths));
                    Assert.That(result.X1, Is.Not.Null);
                    Assert.That(result.X2, Is.Not.Null);
                    Assert.That(result.X3, Is.Not.Null);

                    Assert.That(result.YScales, Is.EqualTo(Domain.Speeds));
                    Assert.That(result.Y1, Is.Not.Null);
                    Assert.That(result.Y2, Is.Not.Null);
                    Assert.That(result.Y3, Is.Not.Null);
                }
            });
        }
        #endregion .ctor tests

        #region Value and Scale
        const bool XSet = true;
        const bool YSet = false;

        const int mm = 0;
        const int cm = 1;
        const int inch = 2;

        const int mm_sec = 0;
        const int mm_min = 1;
        const int cm_min = 2;

        static readonly object[] Assign =
        {
            // V1, V2, V3, others, X true or Y false, Index, value?, scale? -> V1, V2, V3
            new object?[]
            {
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Speeds[mm_sec]),
                XSet, 0, 1.0, null,
                new ScaledType<double>(1, Domain.Lengths[mm]),
                new ScaledType<double>(1, Domain.Lengths[mm]),
                new ScaledType<double>(1, Domain.Lengths[mm]),
            },
            new object?[]
            {
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Speeds[mm_sec]),
                XSet, 1, 1.0, null,
                new ScaledType<double>(1, Domain.Lengths[mm]),
                new ScaledType<double>(1, Domain.Lengths[mm]),
                new ScaledType<double>(1, Domain.Lengths[mm]),
            },
            new object?[]
            {
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Speeds[mm_sec]),
                XSet, 2, 1.0, null,
                new ScaledType<double>(1, Domain.Lengths[mm]),
                new ScaledType<double>(1, Domain.Lengths[mm]),
                new ScaledType<double>(1, Domain.Lengths[mm]),
            },

            new object?[]
            {
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Speeds[mm_sec]),
                XSet, 0, null, Domain.Lengths[cm],
                new ScaledType<double>(0, Domain.Lengths[cm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
            },
            new object?[]
            {
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Speeds[mm_sec]),
                XSet, 1, null, Domain.Lengths[cm],
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[cm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
            },
            new object?[]
            {
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Speeds[mm_sec]),
                XSet, 2, null, Domain.Lengths[cm],
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[cm]),
            },

            new object?[]
            {
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Speeds[mm_sec]),
                XSet, 0, null, Domain.Speeds[mm_sec],
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
            },
            new object?[]
            {
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Speeds[mm_sec]),
                XSet, 1, null, Domain.Speeds[mm_sec],
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
            },
            new object?[]
            {
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Speeds[mm_sec]),
                XSet, 2, null, Domain.Speeds[mm_sec],
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
            },

            new object?[]
            {
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Speeds[mm_sec]),
                XSet, 0, 1.0, Domain.Lengths[cm],
                new ScaledType<double>(1, Domain.Lengths[cm]),
                new ScaledType<double>(10, Domain.Lengths[mm]),
                new ScaledType<double>(10, Domain.Lengths[mm]),
            },
            new object?[]
            {
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Speeds[mm_sec]),
                XSet, 1, 1.0, Domain.Lengths[cm],
                new ScaledType<double>(10, Domain.Lengths[mm]),
                new ScaledType<double>(1, Domain.Lengths[cm]),
                new ScaledType<double>(10, Domain.Lengths[mm]),
            },
            new object?[]
            {
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Speeds[mm_sec]),
                XSet, 2, 1.0, Domain.Lengths[cm],
                new ScaledType<double>(10, Domain.Lengths[mm]),
                new ScaledType<double>(10, Domain.Lengths[mm]),
                new ScaledType<double>(1, Domain.Lengths[cm]),
            },

            new object?[]
            {
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[cm]),
                new ScaledType<double>(0, Domain.Lengths[inch]),
                new ScaledType<double>(0, Domain.Speeds[mm_sec]),
                XSet, 0, 1.0, null,
                new ScaledType<double>(1, Domain.Lengths[mm]),
                new ScaledType<double>(0.1, Domain.Lengths[cm]),
                new ScaledType<double>(0.1/2.54, Domain.Lengths[inch]),
            },
            new object?[]
            {
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[cm]),
                new ScaledType<double>(0, Domain.Lengths[inch]),
                new ScaledType<double>(0, Domain.Speeds[mm_sec]),
                XSet, 1, 1.0, null,
                new ScaledType<double>(10, Domain.Lengths[mm]),
                new ScaledType<double>(1, Domain.Lengths[cm]),
                new ScaledType<double>(1.0/2.54, Domain.Lengths[inch]),
            },
            new object?[]
            {
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[cm]),
                new ScaledType<double>(0, Domain.Lengths[inch]),
                new ScaledType<double>(0, Domain.Speeds[mm_sec]),
                XSet, 2, 1.0, null,
                new ScaledType<double>(25.4, Domain.Lengths[mm]),
                new ScaledType<double>(2.54, Domain.Lengths[cm]),
                new ScaledType<double>(1, Domain.Lengths[inch]),
            },

            new object?[]
            {
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Speeds[mm_sec]),
                new ScaledType<double>(0, Domain.Speeds[mm_sec]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                YSet, 0, 1.0, null,
                new ScaledType<double>(1, Domain.Speeds[mm_sec]),
                new ScaledType<double>(1, Domain.Speeds[mm_sec]),
                new ScaledType<double>(1, Domain.Speeds[mm_sec]),
            },
            new object?[]
            {
                new ScaledType<double>(0, Domain.Speeds[mm_sec]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Speeds[mm_sec]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                YSet, 1, 1.0, null,
                new ScaledType<double>(1, Domain.Speeds[mm_sec]),
                new ScaledType<double>(1, Domain.Speeds[mm_sec]),
                new ScaledType<double>(1, Domain.Speeds[mm_sec]),
            },
            new object?[]
            {
                new ScaledType<double>(0, Domain.Speeds[mm_sec]),
                new ScaledType<double>(0, Domain.Speeds[mm_sec]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                new ScaledType<double>(0, Domain.Lengths[mm]),
                YSet, 2, 1.0, null,
                new ScaledType<double>(1, Domain.Speeds[mm_sec]),
                new ScaledType<double>(1, Domain.Speeds[mm_sec]),
                new ScaledType<double>(1, Domain.Speeds[mm_sec]),
            },
        };

        [TestCaseSource(nameof(Assign))]
        public void Assign_ValueScale(
            ScaledType<double> V1,
            ScaledType<double> V2,
            ScaledType<double> V3,
            ScaledType<double> others,
            bool set,
            int index,
            double? value,
            string? scale,
            ScaledType<double> V1_expected,
            ScaledType<double> V2_expected,
            ScaledType<double> V3_expected)
        {
            // prepare
            var result = new Converter();
            if (set == XSet)
            {
                result.X1 = V1.Clone();
                result.X2 = V2.Clone();
                result.X3 = V3.Clone();
                result.Y1 = others.Clone();
                result.Y2 = others.Clone();
                result.Y3 = others.Clone();
            }
            else // (set == YSet)
            {
                result.X1 = others.Clone();
                result.X2 = others.Clone();
                result.X3 = others.Clone();
                result.Y1 = V1.Clone();
                result.Y2 = V2.Clone();
                result.Y3 = V3.Clone();
            }
            Exception? e_expected = null;

            // execute
            Exception? e = null;
            try
            {
                if (set == XSet)
                {
                    if (value is not null)
                    {
                        switch (index)
                        {
                            case 0: result.X1.Value = (double)value; break;
                            case 1: result.X2.Value = (double)value; break;
                            case 2: result.X3.Value = (double)value; break;
                            default: throw new ArgumentOutOfRangeException($"index");
                        }
                    }
                    if (scale is not null)
                    {
                        switch (index)
                        {
                            case 0: result.X1.TrySetScale(scale, out _); break;
                            case 1: result.X2.TrySetScale(scale, out _); break;
                            case 2: result.X3.TrySetScale(scale, out _); break;
                            default: throw new ArgumentOutOfRangeException($"index");
                        }
                    }
                }
                else // (set == YSet)
                {
                    if (value is not null)
                    {
                        switch (index)
                        {
                            case 0: result.Y1.Value = (double)value; break;
                            case 1: result.Y2.Value = (double)value; break;
                            case 2: result.Y3.Value = (double)value; break;
                            default: throw new ArgumentOutOfRangeException($"index");
                        }
                    }
                    if (scale is not null)
                    {
                        switch (index)
                        {
                            case 0: result.Y1.TrySetScale(scale, out _); break;
                            case 1: result.Y2.TrySetScale(scale, out _); break;
                            case 2: result.Y3.TrySetScale(scale, out _); break;
                            default: throw new ArgumentOutOfRangeException($"index");
                        }
                    }
                }
            }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                AssertExceptionType(e, e_expected);
                if (set == XSet)
                {
                    AssertIsNear(result.X1, V1_expected);
                    AssertIsNear(result.X2, V2_expected);
                    AssertIsNear(result.X3, V3_expected);
                    Assert.That(result.Y1, Is.EqualTo(others));
                    Assert.That(result.Y2, Is.EqualTo(others));
                    Assert.That(result.Y3, Is.EqualTo(others));
                }
                else // (set == YSet)
                {
                    Assert.That(result.X1, Is.EqualTo(others));
                    Assert.That(result.X2, Is.EqualTo(others));
                    Assert.That(result.X3, Is.EqualTo(others));
                    AssertIsNear(result.Y1, V1_expected);
                    AssertIsNear(result.Y2, V2_expected);
                    AssertIsNear(result.Y3, V3_expected);
                }
            });
        }
        #endregion Value and Scale
    }
}
