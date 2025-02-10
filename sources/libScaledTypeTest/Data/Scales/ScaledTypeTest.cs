using System.Globalization;

using As.Tools.Data.Scales;

namespace As.Tools.Test.Data.Scales
{
    public class ScaledTypeTest : BaseTest
    {
        // TODO: Scan_StringTString (requires parser testing)
        // TODO: TrySetScale_StringString (requires parser testing)
        // TODO: BaseNormal (requires conversion data testing)
        // TODO: BaseFactor (requires conversion data testing)

        [SetUp]
        public void Setup()
        {
            // nothing to do.
        }

        #region .ctor tests
        [Test]
        public void IntegerCtor_Default()
        {
            // prepare
            // nothing to do.

            // execute
            Exception? e = null;
            ScaledType<int>? result = null;
            try
            {
                result = new ScaledType<int>();
            }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(e, Is.Null);
                if (result == null)
                {
                    Assert.Fail("result is null");
                }
                else
                {
                    Assert.That(result.Value, Is.EqualTo(0));
                    Assert.That(result.Scale.Count, Is.EqualTo(1));
                    Assert.That(result.Scale[0].Unit, Is.EqualTo(Unit.c));
                    Assert.That(result.Scale[0].Exp, Is.EqualTo(1));
                }
            });
        }

        [Test]
        public void IntegerCtor_Value1()
        {
            // prepare
            // nothing to do.

            // execute
            Exception? e = null;
            ScaledType<int>? result = null;
            try { result = new ScaledType<int>(1); }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(e, Is.Null);
                if (result == null)
                {
                    Assert.Fail("result is null");
                }
                else
                {
                    Assert.That(result.Value, Is.EqualTo(1));
                    Assert.That(result.Scale.Count, Is.EqualTo(1));
                    Assert.That(result.Scale[0].Unit, Is.EqualTo(Unit.c));
                    Assert.That(result.Scale[0].Exp, Is.EqualTo(1));
                }
            });
        }

        [Test]
        public void IntegerCtor_ScaleCm()
        {
            // prepare
            var scale = new Scale();
            scale.Append(Unit.cm);

            // execute
            Exception? e = null;
            ScaledType<int>? result = null;
            try { result = new ScaledType<int>(scale); }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(e, Is.Null);
                if (result == null)
                {
                    Assert.Fail("result is null");
                }
                else
                {
                    Assert.That(result.Value, Is.EqualTo(0));
                    Assert.That(result.Scale.Count, Is.EqualTo(1));
                    Assert.That(result.Scale[0].Unit, Is.EqualTo(Unit.cm));
                    Assert.That(result.Scale[0].Exp, Is.EqualTo(1));
                }
            });
        }

        [Test]
        public void IntegerCtor_ScaleCm2()
        {
            // prepare
            // nothing to do.

            // execute
            Exception? e = null;
            ScaledType<int>? result = null;
            try { result = new ScaledType<int>("[cm^2]"); }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(e, Is.Null);
                if (result == null)
                {
                    Assert.Fail("result is null");
                }
                else
                {
                    Assert.That(result.Value, Is.EqualTo(0));
                    Assert.That(result.Scale.Count, Is.EqualTo(1));
                    Assert.That(result.Scale[0].Unit, Is.EqualTo(Unit.cm));
                    Assert.That(result.Scale[0].Exp, Is.EqualTo(2));
                }
            });
        }

        [Test]
        public void IntegerCtor_UnitCmInt()
        {
            // prepare
            // nothing to do.

            // execute
            Exception? e = null;
            ScaledType<int>? result = null;
            try { result = new ScaledType<int>(Unit.cm); }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(e, Is.Null);
                if (result == null)
                {
                    Assert.Fail("result is null");
                }
                else
                {
                    Assert.That(result.Value, Is.EqualTo(0));
                    Assert.That(result.Scale.Count, Is.EqualTo(1));
                    Assert.That(result.Scale[0].Unit, Is.EqualTo(Unit.cm));
                    Assert.That(result.Scale[0].Exp, Is.EqualTo(1));
                }
            });
        }

        [Test]
        public void IntegerCtor_UnitCmInt2()
        {
            // prepare
            // nothing to do

            // execute
            Exception? e = null;
            ScaledType<int>? result = null;
            try { result = new ScaledType<int>(Unit.cm, 2); }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(e, Is.Null);
                if (result == null)
                {
                    Assert.Fail("result is null");
                }
                else
                {
                    Assert.That(result.Value, Is.EqualTo(0));
                    Assert.That(result.Scale.Count, Is.EqualTo(1));
                    Assert.That(result.Scale[0].Unit, Is.EqualTo(Unit.cm));
                    Assert.That(result.Scale[0].Exp, Is.EqualTo(2));
                }
            });
        }

        [Test]
        public void IntegerCtor_StringScaleCm()
        {
            // prepare
            // nothing to do.

            // execute
            Exception? e = null;
            ScaledType<int>? result = null;
            try { result = new ScaledType<int>("[cm]"); }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(e, Is.Null);
                if (result == null)
                {
                    Assert.Fail("result is null");
                }
                else
                {
                    Assert.That(result.Value, Is.EqualTo(0));
                    Assert.That(result.Scale.Count, Is.EqualTo(1));
                    Assert.That(result.Scale[0].Unit, Is.EqualTo(Unit.cm));
                    Assert.That(result.Scale[0].Exp, Is.EqualTo(1));
                }
            });
        }

        [Test]
        public void IntegerCtor_StringScaleCm2()
        {
            // prepare
            // nothing to do.

            // execute
            Exception? e = null;
            ScaledType<int>? result = null;
            try { result = new ScaledType<int>("[cm^2]"); }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(e, Is.Null);
                if (result == null)
                {
                    Assert.Fail("result is null");
                }
                else
                {
                    Assert.That(result.Value, Is.EqualTo(0));
                    Assert.That(result.Scale.Count, Is.EqualTo(1));
                    Assert.That(result.Scale[0].Unit, Is.EqualTo(Unit.cm));
                    Assert.That(result.Scale[0].Exp, Is.EqualTo(2));
                }
            });
        }

        [Test]
        public void IntegerCtor_Value2UnitCmInt()
        {
            // prepare
            // nothing to do.

            // execute
            Exception? e = null;
            ScaledType<int>? result = null;
            try { result = new ScaledType<int>(2, Unit.cm); }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(e, Is.Null);
                if (result == null)
                {
                    Assert.Fail("result is null");
                }
                else
                {
                    Assert.That(result.Value, Is.EqualTo(2));
                    Assert.That(result.Scale.Count, Is.EqualTo(1));
                    Assert.That(result.Scale[0].Unit, Is.EqualTo(Unit.cm));
                    Assert.That(result.Scale[0].Exp, Is.EqualTo(1));
                }
            });
        }

        [Test]
        public void IntegerCtor_Value2UnitCmInt3()
        {
            // prepare
            // nothing to do.

            // execute
            Exception? e = null;
            ScaledType<int>? result = null;
            try { result = new ScaledType<int>(2, Unit.cm, 3); }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(e, Is.Null);
                if (result == null)
                {
                    Assert.Fail("result is null");
                }
                else
                {
                    Assert.That(result.Value, Is.EqualTo(2));
                    Assert.That(result.Scale.Count, Is.EqualTo(1));
                    Assert.That(result.Scale[0].Unit, Is.EqualTo(Unit.cm));
                    Assert.That(result.Scale[0].Exp, Is.EqualTo(3));
                }
            });
        }

        [Test]
        public void IntegerCtor_Value2StringScaleCm()
        {
            // prepare
            // nothing to do.

            // execute
            Exception? e = null;
            ScaledType<int>? result = null;
            try { result = new ScaledType<int>(2, "[cm]"); }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(e, Is.Null);
                if (result == null)
                {
                    Assert.Fail("result is null");
                }
                else
                {
                    Assert.That(result.Value, Is.EqualTo(2));
                    Assert.That(result.Scale.Count, Is.EqualTo(1));
                    Assert.That(result.Scale[0].Unit, Is.EqualTo(Unit.cm));
                    Assert.That(result.Scale[0].Exp, Is.EqualTo(1));
                }
            });
        }

        [Test]
        public void IntegerCtor_Value2ScaleCm()
        {
            // prepare
            Scale? scale = null;
            try
            {
                scale = [];
                scale.Append(Unit.cm);
            }
            catch
            {
                Assert.Fail("can not create scale");
            }

            // execute
            Exception? e = null;
            ScaledType<int>? result = null;
            try
            {
                if (scale == null) throw new Exception("scale is null");
                result = new ScaledType<int>(2, scale);
            }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(e, Is.Null);
                if (result == null)
                {
                    Assert.Fail("result is null");
                }
                else
                {
                    Assert.That(result.Value, Is.EqualTo(2));
                    Assert.That(result.Scale.Count, Is.EqualTo(1));
                    Assert.That(result.Scale[0].Unit, Is.EqualTo(Unit.cm));
                    Assert.That(result.Scale[0].Exp, Is.EqualTo(1));
                }
            });
        }

        [Test]
        public void IntegerCtor_Other()
        {
            // prepare
            var other = new ScaledType<int>();

            // execute
            Exception? e = null;
            ScaledType<int>? result = null;
            try { result = new ScaledType<int>(other); }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(e, Is.Null);
                if (result == null)
                {
                    Assert.Fail("result is null");
                }
                else
                {
                    Assert.That(result.Value, Is.EqualTo(other.Value));
                    Assert.That(result, Is.Not.SameAs(other));

                    Assert.That(result.Scale.Count, Is.EqualTo(other.Scale.Count));
                    Assert.That(result.Scale[0].Unit, Is.EqualTo(other.Scale[0].Unit));
                    Assert.That(result.Scale[0].Exp, Is.EqualTo(other.Scale[0].Exp));
                    Assert.That(result.Scale, Is.Not.SameAs(other.Scale));

                }
            });
        }

        [Test]
        public void LongCtor_Default()
        {
            // prepare
            // nothing to do.

            // execute
            Exception? e = null;
            ScaledType<long>? result = null;
            try { result = new ScaledType<long>(); }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(e, Is.Null);
                if (result == null)
                {
                    Assert.Fail("result is null");
                }
                else
                {
                    Assert.That(result.Value, Is.EqualTo(0));
                    Assert.That(result.Scale.Count, Is.EqualTo(1));
                    Assert.That(result.Scale[0].Unit, Is.EqualTo(Unit.c));
                    Assert.That(result.Scale[0].Exp, Is.EqualTo(1));
                }
            });
        }

        [Test]
        public void LongCtor_Value1()
        {
            // prepare
            // nothing to do.

            // execute
            Exception? e = null;
            ScaledType<long>? result = null;
            try { result = new ScaledType<long>(1); }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(e, Is.Null);
                if (result == null)
                {
                    Assert.Fail("result is null");
                }
                else
                {
                    Assert.That(result.Value, Is.EqualTo(1));
                    Assert.That(result.Scale.Count, Is.EqualTo(1));
                    Assert.That(result.Scale[0].Unit, Is.EqualTo(Unit.c));
                    Assert.That(result.Scale[0].Exp, Is.EqualTo(1));
                }
            });
        }

        [Test]
        public void FloatCtor_Default()
        {
            // prepare
            // nothing to do.

            // execute
            Exception? e = null;
            ScaledType<float>? result = null;
            try { result = new ScaledType<float>(); }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(e, Is.Null);
                if (result == null)
                {
                    Assert.Fail("result is null");
                }
                else
                {
                    Assert.That(result.Value, Is.EqualTo(0.0f));
                    Assert.That(result.Scale.Count, Is.EqualTo(1));
                    Assert.That(result.Scale[0].Unit, Is.EqualTo(Unit.c));
                    Assert.That(result.Scale[0].Exp, Is.EqualTo(1));
                }
            });
        }

        [Test]
        public void FloatCtor_Value1()
        {
            // prepare
            // nothing to do.

            // execute
            Exception? e = null;
            ScaledType<float>? result = null;
            try { result = new ScaledType<float>(1.0f); }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(e, Is.Null);
                if (result == null)
                {
                    Assert.Fail("result is null");
                }
                else
                {
                    Assert.That(result.Value, Is.EqualTo(1.0f));
                    Assert.That(result.Scale.Count, Is.EqualTo(1));
                    Assert.That(result.Scale[0].Unit, Is.EqualTo(Unit.c));
                    Assert.That(result.Scale[0].Exp, Is.EqualTo(1));
                }
            });
        }

        [Test]
        public void FloatCtor_Value0_5()
        {
            // prepare
            // nothing to do.

            // execute
            Exception? e = null;
            ScaledType<float>? result = null;
            try { result = new ScaledType<float>(0.5f); }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(e, Is.Null);
                if (result == null)
                {
                    Assert.Fail("result is null");
                }
                else
                {
                    Assert.That(result.Value, Is.EqualTo(0.5));
                    Assert.That(result.Scale.Count, Is.EqualTo(1));
                    Assert.That(result.Scale[0].Unit, Is.EqualTo(Unit.c));
                    Assert.That(result.Scale[0].Exp, Is.EqualTo(1));
                }
            });
        }

        [Test]
        public void DoubleCtor_Default()
        {
            // prepare
            // nothing to do.

            // execute
            Exception? e = null;
            ScaledType<double>? result = null;
            try { result = new ScaledType<double>(); }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(e, Is.Null);
                if (result == null)
                {
                    Assert.Fail("result is null");
                }
                else
                {
                    Assert.That(result.Value, Is.EqualTo(0.0));
                    Assert.That(result.Scale.Count, Is.EqualTo(1));
                    Assert.That(result.Scale[0].Unit, Is.EqualTo(Unit.c));
                    Assert.That(result.Scale[0].Exp, Is.EqualTo(1));
                }
            });
        }

        [Test]
        public void DoubleCtor_Value1()
        {
            // prepare
            // nothing to do.

            // execute
            Exception? e = null;
            ScaledType<double>? result = null;
            try { result = new ScaledType<double>(1.0); }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(e, Is.Null);
                if (result == null)
                {
                    Assert.Fail("result is null");
                }
                else
                {
                    Assert.That(result.Value, Is.EqualTo(1.0));
                    Assert.That(result.Scale.Count, Is.EqualTo(1));
                    Assert.That(result.Scale[0].Unit, Is.EqualTo(Unit.c));
                    Assert.That(result.Scale[0].Exp, Is.EqualTo(1));
                }
            });
        }

        [Test]
        public void DoubleCtor_Value0_5()
        {
            // prepare
            // nothing to do.

            // execute
            Exception? e = null;
            ScaledType<double>? result = null;
            try { result = new ScaledType<double>(0.5); }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(e, Is.Null);
                if (result == null)
                {
                    Assert.Fail("result is null");
                }
                else
                {
                    Assert.That(result.Value, Is.EqualTo(0.5));
                    Assert.That(result.Scale.Count, Is.EqualTo(1));
                    Assert.That(result.Scale[0].Unit, Is.EqualTo(Unit.c));
                    Assert.That(result.Scale[0].Exp, Is.EqualTo(1));
                }
            });
        }
        #endregion .ctor tests

        #region Operators
        static readonly object[] EQ =
        {
            // left, right -> left == right
            new object?[] { null, null, true },
            new object?[] { new ScaledType<double>(), null, false },
            new object?[] { null, new ScaledType<double>(), false },
            new object?[] { new ScaledType<double>(), new ScaledType<double>(), true },
            new object?[] { new ScaledType<double>(), new ScaledType<double>(Unit.cm), false },
            new object?[] { new ScaledType<double>(Unit.cm), new ScaledType<double>(), false },
            new object?[] { new ScaledType<double>(Unit.cm, 2), new ScaledType<double>(Unit.cm, -2), false },
            new object?[] { new ScaledType<double>(Unit.cm, 2), new ScaledType<double>(Unit.cm, 2), true },

            new object?[] { new ScaledType<double>(1), new ScaledType<double>(1), true },
            new object?[] { new ScaledType<double>(2), new ScaledType<double>(2, Unit.cm), false },
            new object?[] { new ScaledType<double>(3, Unit.cm), new ScaledType<double>(3), false },
            new object?[] { new ScaledType<double>(4, Unit.cm, 2), new ScaledType<double>(4, Unit.cm, -2), false },
            new object?[] { new ScaledType<double>(5, Unit.cm, 2), new ScaledType<double>(5, Unit.cm, 2), true },

            new object?[] { new ScaledType<double>(1), new ScaledType<double>(3), false },
            new object?[] { new ScaledType<double>(2, Unit.cm, 2), new ScaledType<double>(4, Unit.cm, 2), false },
        };

        static readonly object[] Arithmetic =
        {
            // left, right -> sum, dif, mul, div, exception, div_exception
            new object?[]
            {
                // scale indifference, divide by 0
                new ScaledType<double>(0, Unit.c), // left
                new ScaledType<double>(0, Unit.c), // right
                new ScaledType<double>(0, Unit.c), // left + right
                new ScaledType<double>(0, Unit.c), // left - right
                new ScaledType<double>(0, Unit.c), // left * right
                new ScaledType<double>(0, Unit.c), // left / right
                null,                              // incompattible scales for +, -
                new DivideByZeroException()        // left / 0
            },
            new object?[]
            {
                // divide by zero
                new ScaledType<double>(0, Unit.cm),    // left
                new ScaledType<double>(0, Unit.cm),    // right
                new ScaledType<double>(0, Unit.cm),    // left + right
                new ScaledType<double>(0, Unit.cm),    // left - right
                new ScaledType<double>(0, Unit.cm, 2), // left * right
                new ScaledType<double>(0, Unit.c),     // left / right
                null,                                  // incompattible scales for +, -
                new DivideByZeroException()            // left / 0
            },
            new object?[]
            {
                // generic
                new ScaledType<double>(1, Unit.cm),     // left
                new ScaledType<double>(1, Unit.cm),     // right
                new ScaledType<double>(2, Unit.cm),     // left + right
                new ScaledType<double>(0, Unit.cm),     // left - right
                new ScaledType<double>(1, Unit.cm, 2),  // left * right
                new ScaledType<double>(1, Unit.c),      // left / right
                null,                                   // incompattible scales  for +, -
                null                                    // left / 0
            },
            new object?[]
            {
                // generic
                new ScaledType<double>(1, Unit.cm),     // left
                new ScaledType<double>(3, Unit.cm),     // right
                new ScaledType<double>(4, Unit.cm),     // left + right
                new ScaledType<double>(-2, Unit.cm),    // left - right
                new ScaledType<double>(3, Unit.cm, 2),  // left * right
                new ScaledType<double>(1.0/3.0, Unit.c),// left / right
                null,                                   // incompattible scales  for +, -
                null                                    // left / 0
            },
            new object?[]
            {
                // incompattible scales, divide by 0
                new ScaledType<double>(0, Unit.c),     // left
                new ScaledType<double>(0, Unit.cm),    // right
                new ScaledType<double>(0, Unit.c),     // left + right
                new ScaledType<double>(0, Unit.c),     // left - right
                new ScaledType<double>(0, Unit.cm),    // left * right
                new ScaledType<double>(0, Unit.cm, -1), // left / right
                new ArgumentOutOfRangeException(),     // incompattible scales for +, -
                new DivideByZeroException()            // left / 0
            },
            new object?[]
            {
                // incompattible scales, divide by 0
                new ScaledType<double>(0, Unit.cm),    // left
                new ScaledType<double>(0, Unit.c),     // right
                new ScaledType<double>(0, Unit.cm),    // left + right
                new ScaledType<double>(0, Unit.cm),    // left - right
                new ScaledType<double>(0, Unit.cm),    // left * right
                new ScaledType<double>(0, Unit.c),     // left / right
                new ArgumentOutOfRangeException(),     // incompattible scales for +, -
                new DivideByZeroException()            // left / 0
            },
            new object?[]
            {
                // incompattible scales
                new ScaledType<double>(1, Unit.c),      // left
                new ScaledType<double>(1, Unit.cm),     // right
                new ScaledType<double>(2, Unit.c),      // left + right
                new ScaledType<double>(0, Unit.c),      // left - right
                new ScaledType<double>(1, Unit.cm),     // left * right
                new ScaledType<double>(1, Unit.cm, -1), // left / right
                new ArgumentOutOfRangeException(),      // incompattible scales  for +, -
                null                                    // left / 0
            },
            new object?[]
            {
                // incompattible scales
                new ScaledType<double>(1, Unit.cm),     // left
                new ScaledType<double>(1, Unit.c),      // right
                new ScaledType<double>(2, Unit.cm),     // left + right
                new ScaledType<double>(0, Unit.cm),     // left - right
                new ScaledType<double>(1, Unit.cm),     // left * right
                new ScaledType<double>(1, Unit.cm),     // left / right
                new ArgumentOutOfRangeException(),      // incompattible scales  for +, -
                null                                    // left / 0
            },
        };

        static readonly object[] ConstantAritmetic =
        {
            // value, constant -> mul, v_over_c, c_over_v, e_v_over_c_expected, e_c_over_v_expected
            new object?[]
            {
                // scale indifference, divide by 0
                new ScaledType<double>(0, Unit.c),  // value
                0.0,                                // constant
                new ScaledType<double>(0, Unit.c),  // value * constant = constant / value
                new ScaledType<double>(0, Unit.c),  // value / constant
                new ScaledType<double>(0, Unit.c),  // constant / value
                new DivideByZeroException(),        // value / 0
                new DivideByZeroException(),        // constant / 0
            },
            new object?[]
            {
                // divide by 0
                new ScaledType<double>(0, Unit.cm),  // value
                0.0,                                // constant
                new ScaledType<double>(0, Unit.cm),  // value * constant = constant / value
                new ScaledType<double>(0, Unit.cm),  // value / constant
                new ScaledType<double>(0, Unit.cm),  // constant / value
                new DivideByZeroException(),        // value / 0
                new DivideByZeroException(),        // constant / 0
            },
            new object?[]
            {
                // generic
                new ScaledType<double>(1, Unit.cm),     // value
                1.0,                                    // constant
                new ScaledType<double>(1, Unit.cm),     // value * constant = constant / value
                new ScaledType<double>(1, Unit.cm),     // value / constant
                new ScaledType<double>(1, Unit.cm, -1), // constant / value
                null,                                   // value / 0
                null,                                   // constant / 0
            },
            new object?[]
            {
                // generic
                new ScaledType<double>(1, Unit.cm),         // value
                3.0,                                        // constant
                new ScaledType<double>(3, Unit.cm),         // value * constant = constant / value
                new ScaledType<double>(1.0/3.0, Unit.cm),   // value / constant
                new ScaledType<double>(3, Unit.cm, -1),     // constant / value
                null,                                       // value / 0
                null,                                       // constant / 0
            },
            new object?[]
            {
                // generic
                new ScaledType<double>(3, Unit.cm),         // value
                1.0,                                        // constant
                new ScaledType<double>(3, Unit.cm),         // value * constant = constant / value
                new ScaledType<double>(3.0, Unit.cm),       // value / constant
                new ScaledType<double>(1.0/3, Unit.cm, -1), // constant / value
                null,                                       // value / 0
                null,                                       // constant / 0
            },
        };

        [TestCaseSource(nameof(EQ))]
        public void Equals_LeftRight(ScaledType<double>? left, ScaledType<double>? right, bool expected)
        {
            // prepare
            // nothing to do

            // execute
            Exception? e = null;
            bool result = !expected;
            try { result = Equals(left, right); }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(e, Is.Null);
                Assert.That(result, Is.EqualTo(expected));
            });
        }

        [TestCaseSource(nameof(EQ))]
        public void OpEq_LeftRight(ScaledType<double>? left, ScaledType<double>? right, bool expected)
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
        public void OpNe_LeftRight(ScaledType<double>? left, ScaledType<double>? right, bool expected)
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

        [TestCaseSource(nameof(Arithmetic))]
        public void Add_LeftRight(
            ScaledType<double> left,
            ScaledType<double> right,
            ScaledType<double> sum_expected,
            ScaledType<double> dif_expected,
            ScaledType<double> mul_expected,
            ScaledType<double> div_expected,
            Exception? e_expected,
            Exception? e_div_expected)
        {
            // prepare
            var expected = sum_expected;

            // execute
            Exception ? e = null;
            var result = left;
            try { result = ScaledType<double>.Add(left, right); }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                AssertExceptionType(e, e_expected);
                if (e_expected is null) Assert.That(result, Is.EqualTo(expected));
            });
        }

        [TestCaseSource(nameof(Arithmetic))]
        public void OpAdd_LeftRight(
            ScaledType<double> left,
            ScaledType<double> right,
            ScaledType<double> sum_expected,
            ScaledType<double> dif_expected,
            ScaledType<double> mul_expected,
            ScaledType<double> div_expected,
            Exception? e_expected,
            Exception? e_div_expected)
        {
            // prepare
            var expected = sum_expected;

            // execute
            Exception? e = null;
            var result = left;
            try { result = left + right; }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                AssertExceptionType(e, e_expected);
                if (e_expected is null) Assert.That(result, Is.EqualTo(expected));
            });
        }

        [TestCaseSource(nameof(Arithmetic))]
        public void OpAddIs_LeftRight(
            ScaledType<double> left,
            ScaledType<double> right,
            ScaledType<double> sum_expected,
            ScaledType<double> dif_expected,
            ScaledType<double> mul_expected,
            ScaledType<double> div_expected,
            Exception? e_expected,
            Exception? e_div_expected)
        {
            // prepare
            var expected = sum_expected;

            // execute
            Exception? e = null;
            var result = left;
            try { result += right; }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                AssertExceptionType(e, e_expected);
                if (e_expected is null) Assert.That(result, Is.EqualTo(expected));
            });
        }

        [TestCaseSource(nameof(Arithmetic))]
        public void Sub_LeftRight(
            ScaledType<double> left,
            ScaledType<double> right,
            ScaledType<double> sum_expected,
            ScaledType<double> dif_expected,
            ScaledType<double> mul_expected,
            ScaledType<double> div_expected,
            Exception? e_expected,
            Exception? e_div_expected)
        {
            // prepare
            var expected = dif_expected;

            // execute
            Exception? e = null;
            var result = left;
            try { result = ScaledType<double>.Sub(left, right); }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                AssertExceptionType(e, e_expected);
                if (e_expected is null) Assert.That(result, Is.EqualTo(expected));
            });
        }

        [TestCaseSource(nameof(Arithmetic))]
        public void OpSub_LeftRight(
            ScaledType<double> left,
            ScaledType<double> right,
            ScaledType<double> sum_expected,
            ScaledType<double> dif_expected,
            ScaledType<double> mul_expected,
            ScaledType<double> div_expected,
            Exception? e_expected,
            Exception? e_div_expected)
        {
            // prepare
            var expected = dif_expected;

            // execute
            Exception? e = null;
            var result = left;
            try { result = left - right; }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                AssertExceptionType(e, e_expected);
                if (e_expected is null) Assert.That(result, Is.EqualTo(expected));
            });
        }

        [TestCaseSource(nameof(Arithmetic))]
        public void OpSubIs_LeftRight(
            ScaledType<double> left,
            ScaledType<double> right,
            ScaledType<double> sum_expected,
            ScaledType<double> dif_expected,
            ScaledType<double> mul_expected,
            ScaledType<double> div_expected,
            Exception? e_expected,
            Exception? e_div_expected)
        {
            // prepare
            var expected = dif_expected;

            // execute
            Exception? e = null;
            var result = left;
            try { result -= right; }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                AssertExceptionType(e, e_expected);
                if (e_expected is null) Assert.That(result, Is.EqualTo(expected));
            });
        }

        [TestCaseSource(nameof(Arithmetic))]
        public void Mul_LeftRight(
            ScaledType<double> value,
            ScaledType<double> constant,
            ScaledType<double> sum_expected,
            ScaledType<double> dif_expected,
            ScaledType<double> mul_expected,
            ScaledType<double> div_expected,
            Exception? e_expected,
            Exception? e_div_expected)
        {
            // prepare
            var expected = mul_expected;

            // execute
            Exception? e = null;
            var result = value;
            try { result = ScaledType<double>.Mul(value, constant); }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                AssertExceptionType(e, null);
                Assert.That(result, Is.EqualTo(expected));
            });
        }

        [TestCaseSource(nameof(Arithmetic))]
        public void OpMul_LeftRight(
            ScaledType<double> left,
            ScaledType<double> right,
            ScaledType<double> sum_expected,
            ScaledType<double> dif_expected,
            ScaledType<double> mul_expected,
            ScaledType<double> div_expected,
            Exception? e_expected,
            Exception? e_div_expected)
        {
            // prepare
            var expected = mul_expected;

            // execute
            Exception? e = null;
            var result = left;
            try { result = left * right; }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                AssertExceptionType(e, null);
                Assert.That(result, Is.EqualTo(expected));
            });
        }

        [TestCaseSource(nameof(Arithmetic))]
        public void OpMulIs_LeftRight(
            ScaledType<double> left,
            ScaledType<double> right,
            ScaledType<double> sum_expected,
            ScaledType<double> dif_expected,
            ScaledType<double> mul_expected,
            ScaledType<double> div_expected,
            Exception? e_expected,
            Exception? e_div_expected)
        {
            // prepare
            var expected = mul_expected;

            // execute
            Exception? e = null;
            var result = left;
            try { result *= right; }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                AssertExceptionType(e, null);
                Assert.That(result, Is.EqualTo(expected));
            });
        }

        [TestCaseSource(nameof(Arithmetic))]
        public void Div_LeftRight(
            ScaledType<double> left,
            ScaledType<double> right,
            ScaledType<double> sum_expected,
            ScaledType<double> dif_expected,
            ScaledType<double> mul_expected,
            ScaledType<double> div_expected,
            Exception? e_expected,
            Exception? e_div_expected)
        {
            // prepare
            var expected = div_expected;

            // execute
            Exception? e = null;
            var result = left;
            try { result = ScaledType<double>.Div(left, right); }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                AssertExceptionType(e, e_div_expected);
                if (e_div_expected is null) Assert.That(result, Is.EqualTo(expected));
            });
        }

        [TestCaseSource(nameof(Arithmetic))]
        public void OpDiv_LeftRight(
            ScaledType<double> left,
            ScaledType<double> right,
            ScaledType<double> sum_expected,
            ScaledType<double> dif_expected,
            ScaledType<double> mul_expected,
            ScaledType<double> div_expected,
            Exception? e_expected,
            Exception? e_div_expected)
        {
            // prepare
            var expected = div_expected;

            // execute
            Exception? e = null;
            var result = left;
            try { result = left / right; }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                AssertExceptionType(e, e_div_expected);
                if (e_div_expected is null) Assert.That(result, Is.EqualTo(expected));
            });
        }

        [TestCaseSource(nameof(Arithmetic))]
        public void OpDivIs_LeftRight(
            ScaledType<double> left,
            ScaledType<double> right,
            ScaledType<double> sum_expected,
            ScaledType<double> dif_expected,
            ScaledType<double> mul_expected,
            ScaledType<double> div_expected,
            Exception? e_expected,
            Exception? e_div_expected)
        {
            // prepare
            var expected = div_expected;

            // execute
            Exception? e = null;
            var result = left;
            try { result /= right; }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                AssertExceptionType(e, e_div_expected);
                if (e_div_expected is null) Assert.That(result, Is.EqualTo(expected));
            });
        }

        [TestCaseSource(nameof(ConstantAritmetic))]
        public void OpMulC_LeftRight(
            ScaledType<double> value,
            double constant,
            ScaledType<double> mul_expected,
            ScaledType<double> v_over_c_expected,
            ScaledType<double> c_over_v_expected,
            Exception? e_v_over_c_expected,
            Exception? e_c_over_v_expected)
        {
            // prepare
            var expected = mul_expected;

            // execute
            Exception? e = null;
            var result = value;
            try { result = value * constant; }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                AssertExceptionType(e, null);
                Assert.That(result, Is.EqualTo(expected));
            });
        }

        [TestCaseSource(nameof(ConstantAritmetic))]
        public void OpMulCIs_LeftRight(
            ScaledType<double> value,
            double constant,
            ScaledType<double> mul_expected,
            ScaledType<double> v_over_c_expected,
            ScaledType<double> c_over_v_expected,
            Exception? e_v_over_c_expected,
            Exception? e_c_over_v_expected)
        {
            // prepare
            var expected = mul_expected;

            // execute
            Exception? e = null;
            var result = value;
            try { result *= constant; }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                AssertExceptionType(e, null);
                Assert.That(result, Is.EqualTo(expected));
            });
        }

        [TestCaseSource(nameof(ConstantAritmetic))]
        public void OpMulV_LeftRight(
            ScaledType<double> value,
            double constant,
            ScaledType<double> mul_expected,
            ScaledType<double> v_over_c_expected,
            ScaledType<double> c_over_v_expected,
            Exception? e_v_over_c_expected,
            Exception? e_c_over_v_expected)
        {
            // prepare
            var expected = mul_expected;

            // execute
            Exception? e = null;
            var result = value;
            try { result = constant * value; }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                AssertExceptionType(e, null);
                Assert.That(result, Is.EqualTo(expected));
            });
        }

        [TestCaseSource(nameof(ConstantAritmetic))]
        public void OpDivC_LeftRight(
            ScaledType<double> value,
            double constant,
            ScaledType<double> mul_expected,
            ScaledType<double> v_over_c_expected,
            ScaledType<double> c_over_v_expected,
            Exception? e_v_over_c_expected,
            Exception? e_c_over_v_expected)
        {
            // prepare
            var expected = v_over_c_expected;
            var exception_expected = e_v_over_c_expected;

            // execute
            Exception? e = null;
            var result = value;
            try { result = value / constant; }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                AssertExceptionType(e, exception_expected);
                if (exception_expected is null) Assert.That(result, Is.EqualTo(expected));
            });
        }

        [TestCaseSource(nameof(ConstantAritmetic))]
        public void OpDivCIs_LeftRight(
            ScaledType<double> value,
            double constant,
            ScaledType<double> mul_expected,
            ScaledType<double> v_over_c_expected,
            ScaledType<double> c_over_v_expected,
            Exception? e_v_over_c_expected,
            Exception? e_c_over_v_expected)
        {
            // prepare
            var expected = v_over_c_expected;
            var exception_expected = e_v_over_c_expected;

            // execute
            Exception? e = null;
            var result = value;
            try { result /= constant; }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                AssertExceptionType(e, exception_expected);
                if (exception_expected is null) Assert.That(result, Is.EqualTo(expected));
            });
        }

        [TestCaseSource(nameof(ConstantAritmetic))]
        public void OpDivV_LeftRight(
            ScaledType<double> value,
            double constant,
            ScaledType<double> mul_expected,
            ScaledType<double> v_over_c_expected,
            ScaledType<double> c_over_v_expected,
            Exception? e_v_over_c_expected,
            Exception? e_c_over_v_expected)
        {
            // prepare
            var expected = c_over_v_expected;
            var exception_expected = e_c_over_v_expected;

            // execute
            Exception? e = null;
            var result = value;
            try { result = constant / value; }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                AssertExceptionType(e, exception_expected);
                if (exception_expected is null) Assert.That(result, Is.EqualTo(expected));
            });
        }
        #endregion Operators

        #region Actions
        void AssertEvent(ScaledTypeEventArgs? result, ScaledTypeEventArgs? expected, string? change_kind = null)
        {
            if (expected is null)
            {
                Assert.That(result, Is.Null);
            }
            else if (result is null)
            {
                Assert.Fail((change_kind == null)
                    ? $"expected a {change_kind} changed event"
                    : "expected a changed event");
            }
            else
            {
                Assert.That(result.IsValueChanged, Is.EqualTo(expected.IsValueChanged));
                Assert.That(result.IsScaleChanged, Is.EqualTo(expected.IsScaleChanged));
            }
        }

        [Test]
        public void Clone()
        {
            // prepare
            var value = new ScaledType<double>();

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
            var value = new ScaledType<double>();

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

        [Test]
        public void OnValueChanged()
        {
            // prepare
            ScaledTypeEventArgs? e_on_value_changed_args_expected = new ScaledTypeEventArgs(value_changed: true);
            ScaledTypeEventArgs? e_on_scale_changed_args_expected = null;
            ScaledTypeEventArgs? e_on_changed_args_expected = e_on_value_changed_args_expected;

            ScaledTypeEventArgs? e_on_value_changed_args_result = null;
            ScaledTypeEventArgs? e_on_scale_changed_args_result = null;
            ScaledTypeEventArgs? e_on_changed_args_result = null;

            ScaledTypeEvent on_value_changed = (object caller, ScaledTypeEventArgs e) => { e_on_value_changed_args_result = e; };
            ScaledTypeEvent on_scale_changed = (object caller, ScaledTypeEventArgs e) => { e_on_scale_changed_args_result = e; };
            ScaledTypeEvent on_changed = (object caller, ScaledTypeEventArgs e) => { e_on_changed_args_result = e; };

            double value = 1.0;
            var expected = new ScaledType<double>(value);

            var result = new ScaledType<double>();
            result.OnValueChanged += on_value_changed;
            result.OnScaleChanged += on_scale_changed;
            result.OnChanged += on_changed;

            // execute
            Exception? e = null;
            try { result.Value = value; }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(e, Is.Null);
                Assert.That(result, Is.EqualTo(expected));
                AssertEvent(e_on_value_changed_args_result, e_on_value_changed_args_expected, "value");
                AssertEvent(e_on_scale_changed_args_result, e_on_scale_changed_args_expected, "scale");
                AssertEvent(e_on_changed_args_result, e_on_changed_args_expected);
            });
        }

        [Test]
        public void OnScaleChanged()
        {
            // prepare
            ScaledTypeEventArgs? e_on_value_changed_args_expected = null;
            ScaledTypeEventArgs? e_on_scale_changed_args_expected = new ScaledTypeEventArgs(scale_changed: true);
            ScaledTypeEventArgs? e_on_changed_args_expected = e_on_scale_changed_args_expected;

            ScaledTypeEventArgs? e_on_value_changed_args_result = null;
            ScaledTypeEventArgs? e_on_scale_changed_args_result = null;
            ScaledTypeEventArgs? e_on_changed_args_result = null;

            ScaledTypeEvent on_value_changed = (object caller, ScaledTypeEventArgs e) => { e_on_value_changed_args_result = e; };
            ScaledTypeEvent on_scale_changed = (object caller, ScaledTypeEventArgs e) => { e_on_scale_changed_args_result = e; };
            ScaledTypeEvent on_changed = (object caller, ScaledTypeEventArgs e) => { e_on_changed_args_result = e; };

            Scale scale_value = new Scale(Unit.cm);
            var expected = new ScaledType<double>(scale_value);

            var result = new ScaledType<double>();
            result.OnValueChanged += on_value_changed;
            result.OnScaleChanged += on_scale_changed;
            result.OnChanged += on_changed;

            // execute
            Exception? e = null;
            try { result.Scale = scale_value; }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(e, Is.Null);
                Assert.That(result, Is.EqualTo(expected));
                AssertEvent(e_on_value_changed_args_result, e_on_value_changed_args_expected, "value");
                AssertEvent(e_on_scale_changed_args_result, e_on_scale_changed_args_expected, "scale");
                AssertEvent(e_on_changed_args_result, e_on_changed_args_expected);
            });
        }
        #endregion Actions

        #region Value
        static readonly object[] Assign =
        {
            // root, other -> set_expeced, assign_expected, expected_exception
            new object?[]
            {
                new ScaledType<double>(0),
                new ScaledType<double>(0),
                new ScaledType<double>(0, Unit.c),
                new ScaledType<double>(0, Unit.c),
                null
            },
            new object?[]
            {
                new ScaledType<double>(0),
                new ScaledType<double>(1),
                new ScaledType<double>(1, Unit.c),
                new ScaledType<double>(1, Unit.c),
                null
            },
            new object?[]
            {
                new ScaledType<double>(0),
                new ScaledType<double>(0, Unit.cm),
                new ScaledType<double>(0, Unit.c),
                new ScaledType<double>(0, Unit.c),
                new ArgumentOutOfRangeException()
            },
            new object?[]
            {
                new ScaledType<double>(0),
                new ScaledType<double>(1, Unit.cm),
                new ScaledType<double>(1, Unit.c),
                new ScaledType<double>(0, Unit.c),
                new ArgumentOutOfRangeException()
            },
            new object?[]
            {
                new ScaledType<double>(0, Unit.cm),
                new ScaledType<double>(0, Unit.cm),
                new ScaledType<double>(0, Unit.cm),
                new ScaledType<double>(0, Unit.cm),
                null
            },
            new object?[]
            {
                new ScaledType<double>(0, Unit.cm),
                new ScaledType<double>(1, Unit.cm),
                new ScaledType<double>(1, Unit.cm),
                new ScaledType<double>(1, Unit.cm),
                null
            },
            new object?[]
            {
                new ScaledType<double>(0, Unit.cm),
                new ScaledType<double>(0),
                new ScaledType<double>(0, Unit.cm),
                new ScaledType<double>(0, Unit.cm),
                new ArgumentOutOfRangeException()
            },
            new object?[]
            {
                new ScaledType<double>(0, Unit.cm),
                new ScaledType<double>(1),
                new ScaledType<double>(1, Unit.cm),
                new ScaledType<double>(0, Unit.cm),
                new ArgumentOutOfRangeException()
            },
        };

        static readonly object[] AssignScaled =
        {
            // root, other -> expeced, expected_result
            new object?[]
            {
                new ScaledType<double>(0),
                new ScaledType<double>(0),
                new ScaledType<double>(0, Unit.c),
                true
            },
            new object?[]
            {
                new ScaledType<double>(0),
                new ScaledType<double>(0, Unit.cm),
                new ScaledType<double>(0, Unit.c),
                false
            },
            new object?[]
            {
                new ScaledType<double>(0),
                new ScaledType<double>(1, Unit.m),
                new ScaledType<double>(0, Unit.c),
                false
            },
            new object?[]
            {
                new ScaledType<double>(0, Unit.cm),
                new ScaledType<double>(0, Unit.cm),
                new ScaledType<double>(0, Unit.cm),
                true
            },
            new object?[]
            {
                new ScaledType<double>(0, Unit.cm),
                new ScaledType<double>(0),
                new ScaledType<double>(0, Unit.cm),
                false
            },
            new object?[]
            {
                new ScaledType<double>(0, Unit.cm),
                new ScaledType<double>(0, Unit.m),
                new ScaledType<double>(0, Unit.cm),
                true
            },
            new object?[]
            {
                new ScaledType<double>(0, Unit.cm),
                new ScaledType<double>(1, Unit.m),
                new ScaledType<double>(100, Unit.cm),
                true
            },
            new object?[]
            {
                new ScaledType<double>(0, Unit.m),
                new ScaledType<double>(1, Unit.cm),
                new ScaledType<double>(0.01, Unit.m),
                true
            },
        };

        static readonly object[] AssignStringDouble =
        {
            // root, other, culture -> expeced, expected_result
            new object?[]
            {
                new ScaledType<double>(0),
                "1",
                null,
                new ScaledType<double>(1.0, Unit.c),
                true
            },
             new object?[]
            {
                new ScaledType<double>(0),
                "1.0",
                null,
                new ScaledType<double>(1.0, Unit.c),
                true
            },
            new object?[]
            {
                new ScaledType<double>(0),
                "1,0",
                null,
                new ScaledType<double>(0, Unit.c),
                false
            },
            new object?[]
            {
                new ScaledType<double>(0),
                "NaN",
                null,
                new ScaledType<double>(double.NaN, Unit.c),
                true
            },
            new object?[]
            {
                new ScaledType<double>(0),
                "1.0",
                CultureInfo.InvariantCulture,
                new ScaledType<double>(1.0, Unit.c),
                true
            },
            new object?[]
            {
                new ScaledType<double>(0),
                "1,0",
                CultureInfo.InvariantCulture,
                new ScaledType<double>(0, Unit.c),
                false
            },
            new object?[]
            {
                new ScaledType<double>(0),
                "NaN",
                CultureInfo.InvariantCulture,
                new ScaledType<double>(double.NaN, Unit.c),
                true
            },
            new object?[]
            {
                new ScaledType<double>(0),
                "1.0",
                CultureInfo.GetCultureInfo("en-US"),
                new ScaledType<double>(1.0, Unit.c),
                true
            },
            new object?[]
            {
                new ScaledType<double>(0),
                "1,0",
                CultureInfo.GetCultureInfo("en-US"),
                new ScaledType<double>(0, Unit.c),
                false
            },
            new object?[]
            {
                new ScaledType<double>(0),
                "NaN",
                CultureInfo.GetCultureInfo("en-US"),
                new ScaledType<double>(double.NaN, Unit.c),
                true
            },
            new object?[]
            {
                new ScaledType<double>(0),
                "1.0",
                CultureInfo.GetCultureInfo("nl-NL"),
                new ScaledType<double>(1.0, Unit.c),
                false
            },
            new object?[]
            {
                new ScaledType<double>(0),
                "1,0",
                CultureInfo.GetCultureInfo("nl-NL"),
                new ScaledType<double>(1.0, Unit.c),
                true
            },
            new object?[]
            {
                new ScaledType<double>(0),
                "NaN",
                CultureInfo.GetCultureInfo("nl-NL"),
                new ScaledType<double>(double.NaN, Unit.c),
                true
            },
        };

        static readonly object[] AssignStringInt =
        {
            // root, other, culture -> expeced, expected_result
            new object?[]
            {
                new ScaledType<int>(0),
                "1",
                null,
                new ScaledType<int>(1, Unit.c),
                true
            },
            new object?[]
            {
                new ScaledType<int>(0),
                "1.",
                null,
                new ScaledType<int>(1, Unit.c),
                true
            },
            new object?[]
            {
                new ScaledType<int>(0),
                "1,",
                null,
                new ScaledType<int>(0, Unit.c),
                false
            },
        };

        [TestCaseSource(nameof(Assign))]
        public void Assign_Other(
            ScaledType<double> root,
            ScaledType<double> other,
            ScaledType<double> set_expected,
            ScaledType<double> assign_expected,
            Exception e_exp)
        {
            // prepare
            var result = root.Clone();
            var expected = assign_expected;
            Exception? e_expected = e_exp;

            // execute
            Exception? e = null;
            try { result.Assign(other); }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                AssertExceptionType(e, e_expected);
                Assert.That(result, Is.EqualTo(expected));
            });
        }

        [TestCaseSource(nameof(Assign))]
        public void SetValue_Other(
            ScaledType<double> root,
            ScaledType<double> other,
            ScaledType<double> set_expected,
            ScaledType<double> assign_expected,
            Exception e_exp)
        {
            // prepare
            var result = root.Clone();
            var expected = set_expected;
            Exception? e_expected = null;

            // execute
            Exception? e = null;
            try { result.SetValue(other); }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                AssertExceptionType(e, e_expected);
                Assert.That(result, Is.EqualTo(expected));
            });
        }

        [TestCaseSource(nameof(Assign))]
        public void IScaledType_SetValue_IScaledType(
            ScaledType<double> root,
            ScaledType<double> other,
            ScaledType<double> set_expected,
            ScaledType<double> assign_expected,
            Exception e_exp)
        {
            // prepare
            var result = root.Clone();
            var expected = set_expected;
            Exception? e_expected = null;

            // execute
            Exception? e = null;
            try { ((IScaledType<ScaledType<double>, double>)result).SetValue(other); }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                AssertExceptionType(e, e_expected);
                Assert.That(result, Is.EqualTo(expected));
            });
        }

        [TestCaseSource(nameof(AssignScaled))]
        public void SetValueScaled_Other(
            ScaledType<double> root,
            ScaledType<double> other,
            ScaledType<double> expected,
            bool expected_result)
        {
            // prepare
            var result = root.Clone();
            Exception? e_expected = null;

            // execute
            Exception? e = null;
            bool r = !expected_result;
            try { r = result.SetValueScaled(other); }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                AssertExceptionType(e, e_expected);
                Assert.That(r, Is.EqualTo(expected_result));
                Assert.That(result, Is.EqualTo(expected));
            });
        }

        [TestCaseSource(nameof(AssignScaled))]
        public void IScaledType_SetValueScaled_IScaledType(
            ScaledType<double> root,
            ScaledType<double> other,
            ScaledType<double> expected,
            bool expected_result)
        {
            // prepare
            var result = root.Clone();
            Exception? e_expected = null;

            // execute
            Exception? e = null;
            bool r = !expected_result;
            try { r = ((IScaledType<ScaledType<double>, double>)result).SetValueScaled(other); }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                AssertExceptionType(e, e_expected);
                Assert.That(r, Is.EqualTo(expected_result));
                Assert.That(result, Is.EqualTo(expected));
            });
        }

        [TestCaseSource(nameof(AssignStringDouble))]
        public void SetValue_StringDouble(
            ScaledType<double> root,
            string other,
            CultureInfo? culture,
            ScaledType<double> set_expected,
            bool set_result_expected)
        {
            // prepare
            var result = root.Clone();
            var expected = set_expected;
            Exception? e_expected = null;

            // execute
            Exception? e = null;
            var set_result = !set_result_expected;
            try
            {
                set_result = (culture is null)
                    ? result.SetValue(other)
                    : result.SetValue(other, culture);
            }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                AssertExceptionType(e, e_expected);
                Assert.That(set_result, Is.EqualTo(set_result_expected));
                if (set_result_expected)
                {
                    Assert.That(result.Value, Is.EqualTo(expected.Value));
                    Assert.That(result.Scale, Is.EqualTo(expected.Scale));
                }
                else
                {
                    Assert.That(result.Value, Is.EqualTo(root.Value));
                    Assert.That(result.Scale, Is.EqualTo(root.Scale));
                }
            });
        }

        [TestCaseSource(nameof(AssignStringInt))]
        public void SetValue_StringInt(
            ScaledType<int> root,
            string other,
            CultureInfo? culture,
            ScaledType<int> set_expected,
            bool set_result_expected)
        {
            // prepare
            var result = root.Clone();
            var expected = set_expected;
            Exception? e_expected = null;

            // execute
            Exception? e = null;
            var set_result = !set_result_expected;
            try
            {
                set_result = (culture is null)
                    ? result.SetValue(other)
                    : result.SetValue(other, culture);
            }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                AssertExceptionType(e, e_expected);
                Assert.That(set_result, Is.EqualTo(set_result_expected));
                if (set_result_expected)
                {
                    Assert.That(result.Value, Is.EqualTo(expected.Value));
                    Assert.That(result.Scale, Is.EqualTo(expected.Scale));
                }
                else
                {
                    Assert.That(result.Value, Is.EqualTo(root.Value));
                    Assert.That(result.Scale, Is.EqualTo(root.Scale));
                }
            });
        }
        #endregion Value

        #region Scale
        // Append_Other -> Scale.Append
        // Append_ScaleBool -> Scale.Append
        // Append_ScaledUnitBool -> Scale.Append
        // Append_ScaleIntBool
        // IScaledType_Append_IScaledTypeBool -> Scale.Append
        #endregion Scale
    }
}
