using System.Collections.Specialized;
using System.ComponentModel;

using As.Tools.Data.Scales;

namespace As.Tools.Test.Data.Scales
{
    public class ScaleTest : BaseTest
    {
        // TODO: Scale.TryParse_StringScale -> ScaleParser.TryParse_StringScale (requires parser testing)

        [SetUp]
        public void Setup()
        {
            // nothing to do.
        }

        #region .ctor tests
        [Test]
        public void Ctor_Default()
        {
            // prepare
            // nothing to do.

            // execute
            var result = new Scale();

            // assert
            Assert.Multiple(() =>
            {
                if (result == null)
                {
                    Assert.Fail("result is null");
                }
                else
                {
                    Assert.That(result, Has.Count.EqualTo(1));
                    Assert.That(result[0].Unit, Is.EqualTo(Unit.c));
                    Assert.That(result[0].Exp, Is.EqualTo(1));
                }
            });
        }

        [Test]
        public void Ctor_UnitCmExp()
        {
            // prepare
            // nothing to do

            // execute
            var result = new Scale(Unit.cm);

            // assert
            Assert.Multiple(() =>
            {
                if (result == null)
                {
                    Assert.Fail("result is null");
                }
                else
                {
                    Assert.That(result, Has.Count.EqualTo(1));
                    Assert.That(result[0].Unit, Is.EqualTo(Unit.cm));
                    Assert.That(result[0].Exp, Is.EqualTo(1));
                }
            });
        }

        [Test]
        public void Ctor_UnitCmExp2()
        {
            // prepare
            // nothing to do

            // execute
            var result = new Scale(Unit.cm, 2);

            // assert
            Assert.Multiple(() =>
            {
                if (result == null)
                {
                    Assert.Fail("result is null");
                }
                else
                {
                    Assert.That(result, Has.Count.EqualTo(1));
                    Assert.That(result[0].Unit, Is.EqualTo(Unit.cm));
                    Assert.That(result[0].Exp, Is.EqualTo(2));
                }
            });
        }

        [Test]
        public void Ctor_ScaledUnitCm()
        {
            // prepare
            var scaled_unit = new ScaledUnit(Unit.cm);

            // execute
            var result = new Scale(scaled_unit);

            // assert
            Assert.Multiple(() =>
            {
                if (result == null)
                {
                    Assert.Fail("result is null");
                }
                else
                {
                    Assert.That(result, Has.Count.EqualTo(1));
                    Assert.That(result[0].Unit, Is.EqualTo(Unit.cm));
                    Assert.That(result[0].Exp, Is.EqualTo(1));
                }
            });
        }

        [Test]
        public void Ctor_Other()
        {
            // prepare
            var scaled_unit = new ScaledUnit(Unit.cm);
            var other = new Scale(scaled_unit);

            // execute
            var result = new Scale(other);

            // assert
            Assert.Multiple(() =>
            {
                if (result == null)
                {
                    Assert.Fail("result is null");
                }
                else
                {
                    Assert.That(result, Has.Count.EqualTo(other.Count));
                    Assert.That(result[0].Unit, Is.EqualTo(other[0].Unit));
                    Assert.That(result[0].Exp, Is.EqualTo(other[0].Exp));
                    Assert.That(result[0], Is.Not.SameAs(other[0]));
                }
            });
        }
        #endregion .ctor tests

        #region Operator tests
        static readonly object[] EQ =
        {
            // left, right -> left == right
            new object?[] { null, null, true },
            new object?[] { new Scale(), null, false },
            new object?[] { null, new Scale(), false },
            new object?[] { new Scale(), new Scale(), true },
            new object?[] { new Scale(), new Scale(new ScaledUnit(Unit.cm)), false },
            new object?[] { new Scale(new ScaledUnit(Unit.cm)), new Scale(), false },
            new object?[] { new Scale(new ScaledUnit(Unit.cm, 2)), new Scale(new ScaledUnit(Unit.cm, -2)), false },
            new object?[] { new Scale(new ScaledUnit(Unit.cm, 2)), new Scale(new ScaledUnit(Unit.cm, 2)), true },
        };

        [TestCaseSource(nameof(EQ))]
        public void AreEqual_LeftRight(Scale? left, Scale? right, bool expected)
        {
            // prepare
            // nothing to do

            // execute
            Exception? e = null;
            bool result = !expected;
            try { result = Scale.AreEqual(left, right); }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(e, Is.Null);
                Assert.That(result, Is.EqualTo(expected));
            });
        }

        [TestCaseSource(nameof(EQ))]
        public void OpEq_LeftRight(Scale? left, Scale? right, bool expected)
        {
            // prepare
            // nothing to do

            // execute
            Exception? e = null;
            bool result = !expected;
            try { result = (left == right); }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(e, Is.Null);
                Assert.That(result, Is.EqualTo(expected));
            });
        }

        [TestCaseSource(nameof(EQ))]
        public void OpNe_LeftRight(Scale? left, Scale? right, bool expected)
        {
            // prepare
            // nothing to do

            // execute
            Exception? e = null;
            bool result = expected;
            try { result = (left != right); }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(e, Is.Null);
                Assert.That(result, Is.EqualTo(!expected));
            });
        }

        static readonly object[] Combine =
        {
            // id, left, right -> left*right, left/right, exception
            new object?[] { null, null,                                      null, null, new ArgumentNullException("left") },
            new object?[] { new Scale(), null,                               null, null, new ArgumentNullException("right") },
            new object?[] { null, new Scale(),                               null, null, new ArgumentNullException("left") },
            new object?[] { new Scale(), new Scale(),                        new Scale(), new Scale(), null },
            new object?[] { new Scale(), new Scale(new ScaledUnit(Unit.cm)), new Scale(new ScaledUnit(Unit.cm, 1)), new Scale(new ScaledUnit(Unit.cm, -1)), null },
            new object?[] { new Scale(new ScaledUnit(Unit.cm)), new Scale(), new Scale(new ScaledUnit(Unit.cm, 1)), new Scale(new ScaledUnit(Unit.cm, 1)), null },

            new object?[] { new Scale(new ScaledUnit(Unit.cm, 1)), new Scale(new ScaledUnit(Unit.cm, 1)), new Scale(new ScaledUnit(Unit.cm, 2)), new Scale(), null },
            new object?[] { new Scale(new ScaledUnit(Unit.cm, 2)), new Scale(new ScaledUnit(Unit.cm, 1)), new Scale(new ScaledUnit(Unit.cm, 3)), new Scale(new ScaledUnit(Unit.cm, 1)), null },
        };

        [TestCaseSource(nameof(Combine))]
        public void OpMul_LeftRight(Scale? left, Scale? right, Scale? mul_expected, Scale? div_expected, Exception? e_expected)
        {
            // prepare
            // nothing to do

            // execute
            Exception? e = null;
            Scale? result = null;
            try
            {
#pragma warning disable CS8604 // Possible null reference argument.
                result = left * right;
#pragma warning restore CS8604 // Possible null reference argument.
            }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                if (e_expected is null)
                {
                    Assert.That(e, Is.Null);
                }
                else if (e == null)
                {
                    Assert.Fail($"expected e={e_expected}");
                }
                else
                {
                    Assert.That(e.GetType(), Is.EqualTo(e_expected.GetType()));
                }
                if (result == null)
                {
                    Assert.That(mul_expected, Is.Null);
                }
                else
                {
                    Assert.That(result, Is.EqualTo(mul_expected));
                }
            });
        }


        [TestCaseSource(nameof(Combine))]
        public void OpDiv_LeftRight(Scale? left, Scale? right, Scale? mul_expected, Scale? div_expected, Exception? e_expected)
        {
            // prepare
            // nothing to do

            // execute
            Exception? e = null;
            Scale? result = null;
            try
            {
#pragma warning disable CS8604 // Possible null reference argument.
                result = left / right;
#pragma warning restore CS8604 // Possible null reference argument.
            }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                if (e_expected is null)
                {
                    Assert.That(e, Is.Null);
                }
                else if (e == null)
                {
                    Assert.Fail($"expected e={e_expected}");
                }
                else
                {
                    Assert.That(e.GetType(), Is.EqualTo(e_expected.GetType()));
                }
                if (result == null)
                {
                    Assert.That(div_expected, Is.Null);
                }
                else
                {
                    Assert.That(result, Is.EqualTo(div_expected));
                }
            });
        }
        #endregion Operator tests

        #region Action tests
        // AreEqual_ScaleScale -> operator ==
        // Equals -> AreEqual

        public class ScaleDescriptor(bool is_null, Unit unit, int exp)
        {
            public readonly bool IsNull = is_null;
            public readonly Unit Unit = unit;
            public readonly int Exp = exp;
        }

        static readonly object[] Normalised =
        {
            // description -> expected
            new object?[]
            {
                "[#] -> [#]",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1)
                },
            },
            new object?[]
            {
                "[# #] -> [#]",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1),
                    new(false, Unit.c, 1)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1)
                }
            },
            new object?[]
            {
                "[#^2] -> [#]",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 2)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1)
                }
            },
            new object?[]
            {
                "[# / #] -> [#]",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1),
                    new(false, Unit.c, -1)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1)
                }
            },
            new object?[]
            {
                "[# cm] -> [cm]",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1),
                    new(false, Unit.cm, 1)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 1)
                }
            },
            new object?[]
            {
                "[# / cm] -> [cm^-1]",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1),
                    new(false, Unit.cm, -1)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, -1)
                }
            },
            new object?[]
            {
                "[cm #] -> [cm]",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 1),
                    new(false, Unit.c, 1),
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 1)
                }
            },
            new object?[]
            {
                "[cm / #] -> [cm]",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 1),
                    new(false, Unit.c, -1),
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 1)
                }
            },
            new object?[]
            {
                "[cm cm] -> [cm^2]",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 1),
                    new(false, Unit.cm, 1),
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 2)
                }
            },
            new object?[]
            {
                "[cm / cm] -> [#]",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 1),
                    new(false, Unit.cm, -1),
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1)
                }
            },
            new object?[]
            {
                "[cm cm / s s ] -> [cm^2/s^2]",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 1),
                    new(false, Unit.s, -1),
                    new(false, Unit.cm, 1),
                    new(false, Unit.s, -1),
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 2),
                    new(false, Unit.s, -2),
                }
            },
        };

        static readonly object[] Identical =
        {
            // action, description ,permutation -> expected
            new object?[]
            {
                "[#], [#] -> expected",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1)
                },
                true,
            },
            new object?[]
            {
                "[# cm], [cm #] -> expected",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1),
                    new(false, Unit.cm, 1),
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 1),
                    new(false, Unit.c, 1),
                },
                true,
            },
            new object?[]
            {
                "[c cm s], [c cm s] -> expected",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1),
                    new(false, Unit.cm, 1),
                    new(false, Unit.s, 1),
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1),
                    new(false, Unit.cm, 1),
                    new(false, Unit.s, 1),
                },
                true,
            },
            new object?[]
            {
                "[c cm s], [c s cm] -> expected",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1),
                    new(false, Unit.cm, 1),
                    new(false, Unit.s, 1),
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1),
                    new(false, Unit.s, 1),
                    new(false, Unit.cm, 1),
                },
                true,
            },
            new object?[]
            {
                "[c cm s], [cm c s] -> expected",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1),
                    new(false, Unit.cm, 1),
                    new(false, Unit.s, 1),
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 1),
                    new(false, Unit.c, 1),
                    new(false, Unit.s, 1),
                },
                true,
            },
            new object?[]
            {
                "[c cm s], [cm s c] -> expected",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1),
                    new(false, Unit.cm, 1),
                    new(false, Unit.s, 1),
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 1),
                    new(false, Unit.s, 1),
                    new(false, Unit.c, 1),
                },
                true,
            },
            new object?[]
            {
                "[c cm s], [s c cm] -> expected",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1),
                    new(false, Unit.cm, 1),
                    new(false, Unit.s, 1),
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.s, 1),
                    new(false, Unit.c, 1),
                    new(false, Unit.cm, 1),
                },
                true,
            },
            new object?[]
            {
                "[c cm s], [s cm c] -> expected",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1),
                    new(false, Unit.cm, 1),
                    new(false, Unit.s, 1),
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.s, 1),
                    new(false, Unit.cm, 1),
                    new(false, Unit.c, 1),
                },
                true,
            },
            new object?[]
            {
                "[c cm s], [cm cm c] -> expected",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1),
                    new(false, Unit.cm, 1),
                    new(false, Unit.s, 1),
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 1),
                    new(false, Unit.cm, 1),
                    new(false, Unit.c, 1),
                },
                false,
            },
             new object?[]
            {
                "[c cm s], [s^2 cm c] -> expected",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1),
                    new(false, Unit.cm, 1),
                    new(false, Unit.s, 1),
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.s, 2),
                    new(false, Unit.cm, 1),
                    new(false, Unit.c, 1),
                },
                false,
            },
            new object?[]
            {
                "[c cm s], [s cm cm] -> expected",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1),
                    new(false, Unit.cm, 1),
                    new(false, Unit.s, 1),
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.s, 1),
                    new(false, Unit.cm, 1),
                    new(false, Unit.cm, 1),
                },
                false,
            },
            new object?[]
            {
                "[c cm s], [s cm c^2] -> expected",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1),
                    new(false, Unit.cm, 1),
                    new(false, Unit.s, 1),
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.s, 1),
                    new(false, Unit.cm, 1),
                    new(false, Unit.c, 2),
                },
                false,
            },
       };

        static readonly object[] Append =
        {
            // action, root, add -> expected, reciprode_expected
            new object?[]
            {
                "[#], [#] -> [#], [#]",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1)
                },
            },
            new object?[]
            {
                "[#], [cm^-1] -> [cm^-1], [cm]",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, -1)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, -1)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 1)
                },
            },
            new object?[]
            {
                "[#], [cm] -> [cm], [cm^-1]",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 1)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 1)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, -1)
                },
            },
            new object?[]
            {
                "[#], [cm^2] -> [cm^2], [cm^-2]",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 2)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 2)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, -2)
                },
            },
            new object?[]
            {
                "[s], [cm^-1] -> [s/cm], [s cm]",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.s, 1)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, -1)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.s, 1),
                    new(false, Unit.cm, -1)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.s, 1),
                    new(false, Unit.cm, 1)
                },
            },
            new object?[]
            {
                "[s], [cm] -> [s cm] [s/cm]",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.s, 1)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 1)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.s, 1),
                    new(false, Unit.cm, 1)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.s, 1),
                    new(false, Unit.cm, -1)
                },
            },
            new object?[]
            {
                "[s], [cm^2] -> [ s cm^2], [s/cm^2]",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.s, 1)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 2)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.s, 1),
                    new(false, Unit.cm, 2),
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.s, 1),
                    new(false, Unit.cm, -2),
                },
            },
            new object?[]
            {
                "[cm], [cm^-1] -> [#], [cm^2]",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 1)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, -1)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 2)
                },
            },
            new object?[]
            {
                "[cm], [cm] -> [cm^2], [#]",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 1)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 1)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 2)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.c, 1)
                },
            },
            new object?[]
            {
                "[cm], [cm^2] -> [ cm^3], [cm^-1]",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 1)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 2)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 3)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, -1)
                },
            },
            new object?[]
            {
                "[cm s], [cm^-1] -> [s], [s cm]",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 1),
                    new(false, Unit.s, 1),
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, -1)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.s, 1)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.s, 1),
                    new(false, Unit.cm, 2)
                },
            },
            new object?[]
            {
                "[cm s], [cm] -> [cm^2 s], [s]",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 1),
                    new(false, Unit.s, 1),
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 1)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 2),
                    new(false, Unit.s, 1),
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.s, 1),
                },
            },
            new object?[]
            {
                "[cm s], [cm^2] -> [ cm^3 s], [s/cm]",
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 1),
                    new(false, Unit.s, 1),
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 2)
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, 3),
                    new(false, Unit.s, 1),
                },
                new List<ScaleDescriptor>()
                {
                    new(false, Unit.cm, -1),
                    new(false, Unit.s, 1),
                },
            },
        };

        static Scale BuildScale(List<ScaleDescriptor> value)
        {
            var result = new Scale();
            result.Clear();
            foreach (var d in value)
            {
                result.Add(new ScaledUnit(d.Unit, d.Exp));
            }
            return result;
        }

        [Test]
        public void Clone()
        {
            // prepare
            var value = new Scale();

            // execute
            var result = value.Clone();

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                if (result is not null)
                {
                    Assert.That(result, Is.EqualTo(value));
                    Assert.That(ReferenceEquals(result, value), Is.False);
                }
            });
        }

        [Test]
        public void IClonable_Clone()
        {
            // prepare
            var value = new Scale();

            // execute
            var result = ((ICloneable)value).Clone();

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                if (result is not null)
                {
                    Assert.That(result, Is.EqualTo(value));
                    Assert.That(ReferenceEquals(result, value), Is.False);
                }
            });
        }

        [TestCaseSource(nameof(Normalised))]
        public void Normalise_Scale(string action, List<ScaleDescriptor> descriptor, List<ScaleDescriptor> exp)
        {
            // prepare
            var value = BuildScale(descriptor);
            var expected = BuildScale(exp);

            // execute
            var result = Scale.Normalise(value);

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.IsIdentical(expected), Is.True, $"{result}; {action}");
            });
        }

        [TestCaseSource(nameof(Normalised))]
        public void Normal(string action, List<ScaleDescriptor> descriptor, List<ScaleDescriptor> exp)
        {
            // prepare
            var result = BuildScale(descriptor);
            var expected = BuildScale(exp);

            // execute
            result.Normal();

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.IsIdentical(expected), Is.True, $"{result}; {action}");
            });
        }

        [Test]
        public void NormalUnsuspended()
        {
            // prepare
            var expected = BuildScale(new List<ScaleDescriptor>() { new(false, Unit.c, 1) });
            var result = BuildScale(new List<ScaleDescriptor>()
            {
                new(false, Unit.c, 1),
                new(false, Unit.c, 1)
            });

            int collection_changes = 0;
            int property_changes = 0;

            NotifyCollectionChangedEventHandler c_changed = (object? sender, NotifyCollectionChangedEventArgs e) => collection_changes++;
            PropertyChangedEventHandler p_changed = (object? sender, PropertyChangedEventArgs e) => property_changes++;

            result.CollectionChanged += c_changed;
            ((INotifyPropertyChanged)result).PropertyChanged += p_changed;

            // execute
            result.Normal();

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.IsIdentical(expected), Is.True, "[# #] -> [#]");
                Assert.That(collection_changes, Is.EqualTo(1));
                Assert.That(property_changes, Is.EqualTo(2));
            });
        }

        [Test]
        public void NormalSuspended()
        {
            // prepare
            var expected = BuildScale(new List<ScaleDescriptor>() { new(false, Unit.c, 1) });
            var result = BuildScale(new List<ScaleDescriptor>()
            {
                new(false, Unit.c, 1),
                new(false, Unit.c, 1)
            });

            int collection_changes = 0;
            int property_changes = 0;

            NotifyCollectionChangedEventHandler c_changed = (object? sender, NotifyCollectionChangedEventArgs e) => collection_changes++;
            PropertyChangedEventHandler p_changed = (object? sender, PropertyChangedEventArgs e) => property_changes++;

            result.CollectionChanged += c_changed;
            ((INotifyPropertyChanged)result).PropertyChanged += p_changed;

            result.BeginUpdate();

            // execute
            result.Normal();

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.IsIdentical(expected), Is.True, "[# #] -> [#]");
                Assert.That(collection_changes, Is.EqualTo(0));
                Assert.That(property_changes, Is.EqualTo(0));
            });
        }

        [TestCaseSource(nameof(Identical))]
        public void IsIdentical(string action, List<ScaleDescriptor> desc, List<ScaleDescriptor> per, bool expected)
        {
            // prepare
            var descriptor = BuildScale(desc);
            var permutation = BuildScale(per);

            // execute
            var result = descriptor.IsIdentical(permutation);

            // assert
            Assert.That(result, Is.EqualTo(expected), $"{result}; {action}");
        }

        [TestCaseSource(nameof(Append))]
        public void Append_ScaleBoolFalse(
            string action,
            List<ScaleDescriptor> left,
            List<ScaleDescriptor> right,
            List<ScaleDescriptor> exp,
            List<ScaleDescriptor> rexp)
        {
            // prepare
            var result = BuildScale(left);
            var add = BuildScale(right);
            var expected = BuildScale(exp);

            // execute
            result.Append(add);

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.IsIdentical(expected), Is.True, $"{result}; {action}");
            });
        }

        [TestCaseSource(nameof(Append))]
        public void Append_ScaleBoolTrue(
            string action,
            List<ScaleDescriptor> left,
            List<ScaleDescriptor> right,
            List<ScaleDescriptor> exp,
            List<ScaleDescriptor> rexp)
        {
            // prepare
            var result = BuildScale(left);
            var add = BuildScale(right);
            var expected = BuildScale(rexp);

            // execute
            result.Append(add, reciproce: true);

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.IsIdentical(expected), Is.True, $"{result}; {action}");
            });
        }

        [TestCaseSource(nameof(Append))]
        public void Append_ScaleUnitBoolFalse(
            string action,
            List<ScaleDescriptor> left,
            List<ScaleDescriptor> right,
            List<ScaleDescriptor> exp,
            List<ScaleDescriptor> rexp)
        {
            // prepare
            var result = BuildScale(left);
            var add = BuildScale(right);
            var expected = BuildScale(exp);

            // execute
            foreach (var s in add)
            {
                result.Append(s, false);
            }

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.IsIdentical(expected), Is.True, $"{result}; {action}");
            });
        }

        [TestCaseSource(nameof(Append))]
        public void Append_ScaleUnitBoolTrue(
            string action,
            List<ScaleDescriptor> left,
            List<ScaleDescriptor> right,
            List<ScaleDescriptor> exp,
            List<ScaleDescriptor> rexp)
        {
            // prepare
            var result = BuildScale(left);
            var add = BuildScale(right);
            var expected = BuildScale(rexp);

            // execute
            foreach (var s in add)
            {
                result.Append(s, true);
            }

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.IsIdentical(expected), Is.True, $"{result}; {action}");
            });
        }

        [TestCaseSource(nameof(Append))]
        public void Append_UnitBoolFalse(
            string action,
            List<ScaleDescriptor> left,
            List<ScaleDescriptor> right,
            List<ScaleDescriptor> exp,
            List<ScaleDescriptor> rexp)
        {
            // prepare
            var result = BuildScale(left);
            var expected = BuildScale(exp);

            // execute
            foreach (var s in right)
            {
                result.Append(s.Unit, s.Exp, false);
            }

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.IsIdentical(expected), Is.True, $"{result}; {action}");
            });
        }

        [TestCaseSource(nameof(Append))]
        public void Append_UnitBoolTrue(
            string action,
            List<ScaleDescriptor> left,
            List<ScaleDescriptor> right,
            List<ScaleDescriptor> exp,
            List<ScaleDescriptor> rexp)
        {
            // prepare
            var result = BuildScale(left);
            var expected = BuildScale(rexp);

            // execute
            foreach (var s in right)
            {
                result.Append(s.Unit, s.Exp, true);
            }

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.IsIdentical(expected), Is.True, $"{result}; {action}");
            });
        }
        #endregion Action tests
    }
}
