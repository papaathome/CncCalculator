using System.Collections.ObjectModel;

using As.Tools.Data.Scales;

namespace As.Applications.Models
{
    /// <summary>
    /// Two sets of three variables, one for lengths and one for speeds.<br/>
    /// If one value changes then the other two in the set will adjust.<br/>
    /// If one scale changes then the other two values in the set will adjust (as if there was a value change).
    /// </summary>
    public class Converter
    {
        const int N = 3;

        public Converter()
        {
            XScales = Domain.Lengths;
            YScales = Domain.Speeds;

            XValues = new ScaledType<double>[N];
            YValues = new ScaledType<double>[N];
            for (var i = 0; i < N; i++)
            {
                XValues[i] = new ScaledType<double>(0, XScales[0]);
                XValues[i].OnValueChanged += OnXValueChanged;
                XValues[i].OnScaleChanged += OnXScaleChanged;

                YValues[i] = new ScaledType<double>(0, YScales[0]);
                YValues[i].OnValueChanged += OnYValueChanged;
                YValues[i].OnScaleChanged += OnYScaleChanged;
            }
        }

        #region Properties and Fields
        public ReadOnlyCollection<string> XScales { get; private set; }
        readonly ScaledType<double>[] XValues;

        public ReadOnlyCollection<string> YScales { get; private set; }
        readonly ScaledType<double>[] YValues;

        public ScaledType<double> X1
        {
            get => XValues[0];
            set => SetValue(0, value, XValues, XScales);
        }
        public ScaledType<double> X2
        {
            get => XValues[1];
            set => SetValue(1, value, XValues, XScales);
        }
        public ScaledType<double> X3
        {
            get => XValues[2];
            set => SetValue(2, value, XValues, XScales);
        }

        public ScaledType<double> Y1
        {
            get => YValues[0];
            set => SetValue(0, value, YValues, YScales);
        }
        public ScaledType<double> Y2
        {
            get => YValues[1];
            set => SetValue(1, value, YValues, YScales);
        }
        public ScaledType<double> Y3
        {
            get => YValues[2];
            set => SetValue(2, value, YValues, YScales);
        }

        bool Calculating
        {
            get { lock (calculating_key) { return _calculating; } }
            set { lock (calculating_key) { _calculating = value; } }
        }
        bool _calculating = false;
        readonly object calculating_key = new();
        #endregion Properties and Fields

        #region Actions
        void SetValue(
            int n,
            ScaledType<double> value,
            ScaledType<double>[] set,
            ReadOnlyCollection<string> scales_set)
        {
            if (scales_set.FirstOrDefault(c => (c == $"{value.Scale}")) == null)
            {
                return;
            }
            var equals = set[n].Equals(value);
            set[n].OnValueChanged -= OnXValueChanged;
            set[n].OnScaleChanged -= OnXScaleChanged;
            set[n] = value;
            set[n].OnValueChanged += OnXValueChanged;
            set[n].OnScaleChanged += OnXScaleChanged;
            if (!equals) OnValueChanged(set[n], set);
        }

        bool TrySetCalculating(bool value = true)
        {
            bool result;
            lock (calculating_key)
            {
                result = !_calculating;
                _calculating = value;
            }
            return result;
        }

        void ResetCalcuating() => TrySetCalculating(false);

        static bool TryFindIndex(
            object caller,
            ScaledType<double>[] set,
            out int index)
        {
            index = -1;
            for (var i = 0; i < N; i++)
            {
                if (!caller.Equals(set[i])) continue;
                index = i;
                break;
            }
            return (-1 < index);
        }

        static void SetValuesScaled(ScaledType<double>[] set, int n)
        {
            for (var i = 0; i < N; i++)
            {
                if (i == n) continue;
                set[i].SetValueScaled(set[n]);
            }
        }

        private void OnValueChanged(object caller, ScaledType<double>[] set)
        {
            bool have_set_calculating = false;
            try
            {
                have_set_calculating = TrySetCalculating();
                if (!have_set_calculating) return;
                if (!TryFindIndex(caller, set, out int n)) return;
                SetValuesScaled(set, n);
            }
            finally
            {
                if (have_set_calculating) ResetCalcuating();
            }
        }

        private void OnScaleChanged(
            object caller,
            ScaledType<double>[] set,
            ReadOnlyCollection<string> scales,
            ScaledTypeEventArgs e)
        {
            if (
                (e.NewScale is null) ||
                (scales.FirstOrDefault(c => (c == $"{e.NewScale}")) == null)
               )
            {
                var c = caller as ScaledType<double>;
                if ((c is not null) && (e.OldScale is not null))
                {
                    c.Scale = e.OldScale;
                }
                return;
            }
            OnValueChanged(caller, set);
        }

        private void OnXValueChanged(object caller, ScaledTypeEventArgs e)
            => OnValueChanged(caller, XValues);

        private void OnYValueChanged(object caller, ScaledTypeEventArgs e)
            => OnValueChanged(caller, YValues);

        private void OnXScaleChanged(object caller, ScaledTypeEventArgs e)
            => OnScaleChanged(caller, XValues, XScales, e);

        private void OnYScaleChanged(object caller, ScaledTypeEventArgs e)
            => OnScaleChanged(caller, YValues, YScales, e);
        #endregion Actions
    }
}
