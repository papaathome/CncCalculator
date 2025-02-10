using As.Tools.Data.Scales;

namespace As.Tools.Test.Data.Scales
{
    public class ScaledUnitTest : BaseTest
    {
        // TODO: ScaledUnit.TryParse_StringUnit (requires parser testing)
        // TODO: ScaledUnit.Factor (requires conversion data testing)

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
            var result = new ScaledUnit();

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Unit, Is.EqualTo(Unit.c));
                Assert.That(result.Exp, Is.EqualTo(1));
            });
        }

        [Test]
        public void Ctor_UnitCmExp()
        {
            // prepare
            // nothing to do.

            // execute
            var result = new ScaledUnit(Unit.cm);

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Unit, Is.EqualTo(Unit.cm));
                Assert.That(result.Exp, Is.EqualTo(1));
            });
        }

        [Test]
        public void Ctor_UnitCmExp2()
        {
            // prepare
            // nothing to do.

            // execute
            var result = new ScaledUnit(Unit.cm, 2);

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Unit, Is.EqualTo(Unit.cm));
                Assert.That(result.Exp, Is.EqualTo(2));
            });
        }

        [Test]
        public void Ctor_Other()
        {
            // prepare
            var other = new ScaledUnit(Unit.cm, 2);

            // execute
            var result = new ScaledUnit(other);

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Unit, Is.EqualTo(other.Unit));
                Assert.That(result.Exp, Is.EqualTo(other.Exp));
                Assert.That(result, Is.Not.SameAs(other));
            });
        }
        #endregion .ctor tests

        #region Operator tests
        static readonly object[] EQ =
        {
            // left, right -> expected
            new object?[] { null, null, true },
            new object?[] { new ScaledUnit(), null, false },
            new object?[] { null, new ScaledUnit(), false },
            new object?[] { new ScaledUnit(), new ScaledUnit(), true },
            new object?[] { new ScaledUnit(), new ScaledUnit(Unit.cm), false },
            new object?[] { new ScaledUnit(Unit.cm), new ScaledUnit(), false },
            new object?[] { new ScaledUnit(Unit.cm), new ScaledUnit(Unit.cm), true },
        };

        [TestCaseSource(nameof(EQ))]
        public void OpEq_LeftRight(ScaledUnit? left, ScaledUnit? right, bool expected)
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
        public void OpNe_LeftRight(ScaledUnit? left, ScaledUnit? right, bool expected)
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

        [TestCaseSource(nameof(EQ))]
        public void Equals_Obj(ScaledUnit? left, ScaledUnit? right, bool expected)
        {
            // prepare
            // nothing to do

            // execute
            Exception? e = null;
            bool result = expected;
            try
            {
                result =
                      (left is not null) ? left.Equals(right)
                    : (right is not null) ? right.Equals(left)
                    : true;
            }
            catch (Exception x) { e = x; }

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(e, Is.Null);
                Assert.That(result, Is.EqualTo(expected));
            });
        }
        #endregion Operator tests

        #region Action tests
        static readonly object[] Exp =
        {
            // root, value, -> add_expected, set_expected
            new object?[] { new ScaledUnit(Unit.c, 1), -1, 0, -1 },
            new object?[] { new ScaledUnit(Unit.c, 1),  0, 1,  1 },
            new object?[] { new ScaledUnit(Unit.c, 1),  1, 2,  1 },

            new object?[] { new ScaledUnit(Unit.c, 2), -1, 1, -1 },
            new object?[] { new ScaledUnit(Unit.c, 2),  0, 2,  2 },
            new object?[] { new ScaledUnit(Unit.c, 2),  1, 3,  1 },
        };

        [Test]
        public void Clone()
        {
            // prepare
            var value = new ScaledUnit();

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
            var value = new ScaledUnit();

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

        [TestCaseSource(nameof(Exp))]
        public void ExpAdd_Int(ScaledUnit root, int value, int add_expected, int set_expected)
        {
            // prepare
            var result = root;
            var expected = add_expected;

            // execute
            result.ExpAdd(value);

            // assert
            Assert.That(result.Exp, Is.EqualTo(expected));
        }

        [TestCaseSource(nameof(Exp))]
        public void ExpSet_Int(ScaledUnit root, int value, int add_expected, int set_expected)
        {
            // prepare
            var result = root;
            var expected = set_expected;

            // execute
            result.ExpSet(value);

            // assert
            Assert.That(result.Exp, Is.EqualTo(expected));
        }

        [Test]
        public void ExpReset()
        {
            // prepare
            var result = new ScaledUnit(Unit.c, 2);
            var expected = 1;

            // execute
            result.ExpReset();

            // assert
            Assert.That(result.Exp, Is.EqualTo(expected));
        }
        #endregion Action tests
    }
}
