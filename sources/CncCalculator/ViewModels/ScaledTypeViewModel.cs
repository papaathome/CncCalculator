using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

using As.Tools.Data.Scales;

using Caliburn.Micro;

namespace As.Applications.ViewModels
{
    public class DoubleSTViewModel : ScaledTypeViewModel<double>
    {
        public DoubleSTViewModel(
            string name,
            ReadOnlyCollection<string> domain,
            ScaledType<double> data) :
            base(name, domain, data) { }
    }

    public class LongSTViewModel : ScaledTypeViewModel<long>
    {
        public LongSTViewModel(
            string name,
            ReadOnlyCollection<string> domain,
            ScaledType<long> data) :
            base(name, domain, data) { }
    }

    public class ScaledTypeViewModel<T> :
        Screen,
        IDataErrorInfo
        where T : INumber<T>
    {
        // The default constructor have unwanted side effects.
        // <summary>
        // .Ctor, the selected scale is, one of three posible values<br/>
        // On Domain then Data=(value, "[x]") the slected scale is one of { "[#]", "[x]", Domain[0] }<br/>
        // On Data=(value, "[x]") then Domain the slected scale is one of { "[#]", Domain[0] } and not "[x]" from (value, "[x]")
        // </summary>
        // <![CDATA[
        // Note on expected scale value when initialising Domain firs and then Data:
        // One of three scales becomes active, "[#]", Domain[0] or "[#]" from (value "[x]").
        //    Seqeunce of events:
        //    result = new ScaledTypeViewModel<T>() -> result.Domain = [], result.Data = (0 "[#]")
        //    result.Domain = value
        //       case 1: Domain is []
        //           Constraint Scale in Domain is applied.
        //          "[#]" is not in Domain -> reject scale.
        //          Domain is [] -> Domain[0] not available.
        //          Assign scale "[#]" (which is already the active scale)
        //       case 2: Domain is [...] and "[#]" is an element of Domain.
        //          Constraint Scale in Domain is applied.
        //          "[#]" is in Domain -> accept scale.
        //       case 3: Domain is [...] and "[#]" is not an element of Domain
        //          Constraint Scale in Domain is applied.
        //          "[#]" is not in Domain -> reject scale.
        //          Accept scale Domain[0]
        //    result.Data = (1 "[x]")
        //       case 1: Domain is []
        //           Constraint Scale in Domain is applied.
        //          "[#]" is not in Domain -> reject scale.
        //          Domain is [] -> Domain[0] not available.
        //          Assign scale "[#]"
        //       case 2: Domain is [...] and "[x]" is an element of Domain.
        //          Constraint Scale in Domain is applied.
        //          "[x]" is in Domain -> accept scale.
        //       case 3: Domain is [...] and "[x]" is not an element of Domain
        //          Constraint Scale in Domain is applied.
        //          "[#]" is not in Domain -> reject scale.
        //          Accept scale Domain[0]
        // Set of possible results { "[#]", "[x]", Domain[0] }
        // 
        // 
        // Note on expected scale value when initialising Data first and then Domain:
        // One of two scales becomes active, Domain[0] or "[#]" but not "[x]" from (value "[x]").
        //    Seqeunce of events:
        //    result = new ScaledTypeViewModel<T>() -> result.Domain = [], result.Data = (0 "[#]")
        //    result.Data = (value "[x]")
        //       Constraint Scale in Domain is applied.
        //       "[x]" is not in Domain -> reject new scale.
        //       Domain is [] -> Domain[0] not available.
        //       Assign scale "[#]".
        //    result.Domain = value
        //       case 1: Domain is []
        //           Constraint Scale in Domain is applied.
        //          "[#]" is not in Domain -> reject scale.
        //          Domain is [] -> Domain[0] not available.
        //          Assign scale "[#]" (which is already the active scale)
        //       case 2: Domain is [...] and "[#]" is an element of Domain.
        //          Constraint Scale in Domain is applied.
        //          "[#]" is in Domain -> accept scale.
        //       case 3: Domain is [...] and "[#]" is not an element of Domain
        //          Constraint Scale in Domain is applied.
        //          "[#]" is not in Domain -> reject scale.
        //          Accept scale Domain[0] (which may coincide with "[x]" by chance)
        // Set of possible results { "[#]", Domain[0] }
        // ]]>
        //public ScaledTypeViewModel() : base() { }

        /// <summary>
        /// .Ctor, the selected scale is, one of three posible values { "[#]", "[x]", Domain[0] }
        /// with "[x]" from data=(value, "[x]")
        /// </summary>
        public ScaledTypeViewModel(
            string name,
            ReadOnlyCollection<string> domain,
            ScaledType<T> data) :
            base()
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name), "expect non-white text");
            }

            Name = name.Trim();
            Domain = domain;
            Data = data;
        }

        #region Properties
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; private set; } = "";

        /// <summary>
        /// Units list
        /// </summary>
        public ReadOnlyCollection<string> Domain
        {
            get => _domain;
            set
            {
                _domain = value;
                NotifyOfPropertyChange();
                ValidateScale();
            }
        }
        ReadOnlyCollection<string> _domain = new([]);

        /// <summary>
        /// selected units
        /// </summary>
        // TODO: change string to type Scale
        public string Scale
        {
            get => $"{Data.Scale}";
            set
            {
                Data.TrySetScale(value, out string err);
                SetError(err);
            }
        }

        ScaledType<T> Data
        {
            get => _data;
            set
            {
                if (ReferenceEquals(_data, value)) return;
                UnlinkCallbacks(_data);
                _data = value;
                LinkCallbacks(_data);
                ValidateScale();
            }
        }
        ScaledType<T> _data = new ScaledType<T>();

        public T Value
        {
            get => Data.Value;
            set { if (!ValueIsReadOnly) SetValue(value); }
        }

        /// <summary>
        /// Value is read-only.
        /// </summary>
        public bool ValueIsReadOnly
        {
            get => _isReadOnly;
            set
            {
                if (_isReadOnly == value) return;
                _isReadOnly = value;
                NotifyOfPropertyChange();
            }
        }
        bool _isReadOnly = false;
        #endregion Properties

        #region Actions
        /// <summary>
        /// Set value, overriding ValueIsReadOnly. (Do not use in the View)
        /// </summary>
        /// <param name="Value"></param>
        public void SetValue(T value)
            => Data.Value = value;

        /// <summary>
        /// Constraint: Domain contains all acceptable scales for data.<br/>
        /// Scale is the Scale in data if valid, otherwise first in domain if available, otherwise empty<br/>
        /// </summary>
        void ValidateScale()
        {
            if (!Domain.Contains(Scale))
            {
                Scale = (0 < Domain.Count)
                    ? Domain[0]
                    : Tools.Data.Scales.Scale.EMPTY;
            }
        }

        protected void UnlinkCallbacks(IOnChanged? value)
        {
            if (value is null) return;
            value.OnValueChanged -= ValueChanged;
            value.OnScaleChanged -= ScaleChanged;
        }

        protected void LinkCallbacks(IOnChanged? value)
        {
            if (value is null) return;
            value.OnValueChanged += ValueChanged;
            value.OnScaleChanged += ScaleChanged;
        }

        protected void ValueChanged(object sender, ScaledTypeEventArgs e)
            => NotifyOfPropertyChange(nameof(Value));

        protected void ScaleChanged(object sender, ScaledTypeEventArgs e)
            => NotifyOfPropertyChange(nameof(Scale));
        #endregion Actions

        #region IDataErrorInfo
        readonly Dictionary<string, string> _named_errors = [];

        void SetError(string err, [CallerMemberName] string propertyName = "")
        {
            if ((_named_errors.TryGetValue(propertyName, out string? v) ? v : null) != err)
            {
                _named_errors[propertyName] = err;
            }
        }

        public string this[string propertyName]
        {
            get => _named_errors.TryGetValue(
                propertyName,
                out string? v)
                ? v
                : string.Empty;
            set => SetError(value, propertyName);
        }

        public string Error
        {
            get
            {
                var _sb = new StringBuilder();
                _sb.Clear();
                foreach (var kv in _named_errors)
                {
                    if (string.IsNullOrWhiteSpace(kv.Value)) continue;

                    if (0 < _sb.Length) _sb.Append("; ");
                    _sb.Append(kv.Key);
                    _sb.Append(":\"");
                    _sb.Append(kv.Value);
                    _sb.Append('"');
                }
                return (0 < _sb.Length) ? _sb.ToString() : "";
            }
        }
        #endregion IDataErrorInfo
    }
}
