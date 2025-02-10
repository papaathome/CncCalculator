using System.Collections.ObjectModel;
using System.ComponentModel;

using As.Applications.ViewModels;
using As.Tools.Data.Scales;

namespace As.Applications.Test.ViewModels
{
    public class DoubleSTViewModelTest : BaseTest
    {
        // Note: DoubleSTModelView does not implement IEqualable<T> -> Assert.That(x, IsEqualTo(y)) is checking reference equality.

        const string FIRST = "[mm]";
        const string SECOND = "[cm]";

        [SetUp]
        public void Setup() { }

        static readonly ReadOnlyCollection<string> Lengths = new([FIRST, SECOND, "[in]", "[ft]"]);

        #region .ctor tests
        [Test]
        public void Ctor_InvalidName()
        {
            // prepare
            const string name_value = " ";
            var domain_value = Lengths;
            double value_value = 1.0;
            var scale_value = SECOND;
            var data_value = new ScaledType<double>(value_value, scale_value);

            var e_expected = new ArgumentOutOfRangeException();

            // execute
            Exception? e = null;
            DoubleSTViewModel? result = null;
            try
            {
                result = new DoubleSTViewModel(
                    name: name_value,
                    domain: domain_value,
                    data: data_value);
            }
            catch (Exception x) { e = x; }

            // assert
            AssertExceptionType(e, e_expected);
        }

        [Test]
        public void Ctor_InvalidData()
        {
            // prepare
            const string name_value = "name";
            var domain_value = Lengths;
            var value_value = 1.0;
            var scale_value = Tools.Data.Scales.Scale.EMPTY;
            var data_value = new ScaledType<double>(value_value, scale_value);

            const string name_expected = name_value;
            var domain_expected = Lengths;
            double value_expected = value_value;
            var scale_expected = FIRST;

            // execute
            Exception? e = null;
            DoubleSTViewModel? result = null;
            try
            {
                result = new DoubleSTViewModel(
                    name: name_value,
                    domain: domain_value,
                    data: data_value);
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
                    Assert.That(result.Name, Is.EqualTo(name_expected));
                    Assert.That(result.Domain, Is.EqualTo(domain_expected));
                    Assert.That(result.Value, Is.EqualTo(value_expected));
                    Assert.That(result.Scale, Is.EqualTo(scale_expected));
                }
            });
        }

        [Test]
        public void Ctor_ValidData()
        {
            // prepare
            const string name_value = "name";
            var domain_value = Lengths;
            var value_value = 1.0;
            var scale_value = SECOND;
            var data_value = new ScaledType<double>(value_value, scale_value);

            const string name_expected = name_value;
            var domain_expected = Lengths;
            var value_expected = value_value;
            var scale_expected = SECOND;

            // execute
            Exception? e = null;
            DoubleSTViewModel? result = null;
            try
            {
                result = new DoubleSTViewModel(
                    name: name_value,
                    domain: domain_value,
                    data: data_value); ;
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
                    Assert.That(result.Name, Is.EqualTo(name_expected));
                    Assert.That(result.Domain, Is.EqualTo(domain_expected));
                    Assert.That(result.Value, Is.EqualTo(value_expected));
                    Assert.That(result.Scale, Is.EqualTo(scale_expected));
                }
            });
        }
        #endregion .ctor tests

        #region Properties
        // Domain -> .ctor tests

        [Test]
        public void Scale()
        {
            // prepare
            var on_property_changed_events = 0;
            PropertyChangedEventHandler on_property_changed = (
                object? sender,
                PropertyChangedEventArgs e) =>
            {
                on_property_changed_events++;
            };

            var result = new DoubleSTViewModel(
                name: "name",
                domain: Lengths,
                data: new ScaledType<double>(0.0, FIRST))
            {
                ValueIsReadOnly = false
            };
            result.PropertyChanged += on_property_changed;

            var on_value_changed_events_expected = 1;
            var expected = new DoubleSTViewModel(
                name: "name",
                domain: Lengths,
                data: new ScaledType<double>(0.0, SECOND))
            {
                ValueIsReadOnly = false
            };

            // execute
            on_property_changed_events = 0;
            result.Scale = SECOND;

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Value, Is.EqualTo(expected.Value));
                Assert.That(result.ValueIsReadOnly, Is.EqualTo(expected.ValueIsReadOnly));
                Assert.That(result.Scale, Is.EqualTo(expected.Scale));
                Assert.That(on_property_changed_events, Is.EqualTo(on_value_changed_events_expected));
            });
        }

        [Test]
        public void ValueNotReadOnly()
        {
            // prepare
            var on_property_changed_events = 0;
            PropertyChangedEventHandler on_propertye_changed = (
                object? sender,
                PropertyChangedEventArgs e) =>
            {
                on_property_changed_events++;
            };

            var result = new DoubleSTViewModel(
                name: "name",
                domain: Lengths,
                data: new ScaledType<double>(0.0, SECOND))
            {
                ValueIsReadOnly = false
            };
            result.PropertyChanged += on_propertye_changed;

            var on_value_changed_events_expected = 1;
            var expected = new DoubleSTViewModel(
                name: "name",
                domain: Lengths,
                data: new ScaledType<double>(1.0, SECOND))
            {
                ValueIsReadOnly = false
            };

            // execute
            on_property_changed_events = 0;
            result.Value = 1.0;

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Value, Is.EqualTo(expected.Value));
                Assert.That(result.ValueIsReadOnly, Is.EqualTo(expected.ValueIsReadOnly));
                Assert.That(on_property_changed_events, Is.EqualTo(on_value_changed_events_expected));
            });
        }

        [Test]
        public void ValueReadOnly()
        {
            // prepare
            var on_property_changed_events = 0;
            PropertyChangedEventHandler on_property_changed = (object? sender, PropertyChangedEventArgs e) => { on_property_changed_events++; };

            var result = new DoubleSTViewModel(
                name: "name",
                domain: Lengths,
                data: new ScaledType<double>(0.0, SECOND))
            {
                ValueIsReadOnly = true
            };
            result.PropertyChanged += on_property_changed;

            var on_value_changed_events_expected = 0;
            var expected = new DoubleSTViewModel(
                name: "name",
                domain: Lengths,
                data: new ScaledType<double>(0.0, SECOND))
            {
                ValueIsReadOnly = true
            };

            // execute
            result.Value = 1.0;

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Value, Is.EqualTo(expected.Value));
                Assert.That(result.ValueIsReadOnly, Is.EqualTo(expected.ValueIsReadOnly));
                Assert.That(on_property_changed_events, Is.EqualTo(on_value_changed_events_expected));
            });
        }
        #endregion Properties

        #region Actions
        [Test]
        public void SetValueNotReadOnly()
        {
            var on_property_changed_events = 0;
            PropertyChangedEventHandler on_property_changed = (object? sender, PropertyChangedEventArgs e) => { on_property_changed_events++; };

            // prepare
            var result = new DoubleSTViewModel(
                name: "name",
                domain: Lengths,
                data: new ScaledType<double>(0.0, SECOND))
            {
                ValueIsReadOnly = false
            };
            result.PropertyChanged += on_property_changed;

            var on_value_changed_events_expected = 1;
            var expected = new DoubleSTViewModel(
                name: "name",
                domain: Lengths,
                data: new ScaledType<double>(1.0, SECOND))
            {
                ValueIsReadOnly = false
            };

            // execute
            result.SetValue(1.0);

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Value, Is.EqualTo(expected.Value));
                Assert.That(result.ValueIsReadOnly, Is.EqualTo(expected.ValueIsReadOnly));
                Assert.That(on_property_changed_events, Is.EqualTo(on_value_changed_events_expected));
            });
        }

        [Test]
        public void SetValueReadOnly()
        {
            // prepare
            var on_property_changed_events = 0;
            PropertyChangedEventHandler on_property_changed = (object? sender, PropertyChangedEventArgs e) => { on_property_changed_events++; };

            var result = new DoubleSTViewModel(
                name: "name",
                domain: Lengths,
                data: new ScaledType<double>(0.0, SECOND))
            {
                ValueIsReadOnly = true
            };
            result.PropertyChanged += on_property_changed;

            var on_value_changed_events_expected = 1;
            var expected = new DoubleSTViewModel(
                name: "name",
                domain: Lengths,
                data: new ScaledType<double>(1.0, SECOND))
            {
                ValueIsReadOnly = true
            };

            // execute
            result.SetValue(1.0);

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Value, Is.EqualTo(expected.Value));
                Assert.That(result.ValueIsReadOnly, Is.EqualTo(expected.ValueIsReadOnly));
                Assert.That(on_property_changed_events, Is.EqualTo(on_value_changed_events_expected));
            });
        }
        #endregion Actions
    }
}