using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using As.Tools.Data.Scales;

namespace As.Tools.Controls
{
    /// <summary>
    /// ScaledTypeControl: For on screen handling of ScaledType values.
    /// </summary>
    public partial class ScaledTypeControl : UserControl
    {
        /// <summary>
        /// .ctor: create an instance of the ScaledTypeControl
        /// </summary>
        public ScaledTypeControl()
        {
            InitializeComponent();
            InitialiseApplication();
        }

        /// <summary>
        /// InitialiseScale: Initialise application details.
        /// </summary>
        void InitialiseApplication()
        {
            InitialiseScale();
            ScaledValue = new DoubleST();
        }

        #region Container
        /// <summary>
        /// Public access to the (value, scale) data.
        /// </summary>
        /// <remarks>Returns a new copy of the value</remarks>
        public ScaledType ScaledValue
        {
            get { return scaled_value; }
            set { SetScaledValue(value); }
        }

        /// <summary>
        /// Container for ScaledValue.
        /// </summary>
        ScaledType scaled_value = null;

        /// <summary>
        /// SetScaledValue: Initialise a new container for this control
        /// </summary>
        /// <param name="value"></param>
        private void SetScaledValue(ScaledType value)
        {
            if (value == null) return;

            if (scaled_value != null)
            {
                scaled_value.OnScaleChanged -= Container_OnScaleChanged;
                scaled_value.OnValueChanged -= Container_OnValueChanged;
            }
            scaled_value = value;

            labelFloat.Visible = scaled_value.IsFloatingPoint;

            textBoxValue_Update($"{scaled_value.GetValue()}");
            SyncScales();

            scaled_value.OnValueChanged += Container_OnValueChanged;
            scaled_value.OnScaleChanged += Container_OnScaleChanged;
        }

        /// <summary>
        /// Container Scale changed event received.
        /// </summary>
        /// <param name="sender">Container with the changed scale</param>
        /// <param name="e">Change attributes.</param>
        private void Container_OnScaleChanged(object sender, EventArgs e)
        {
            SyncScales(container_locked: true);
            OnScaleChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Container Value changed evend received.
        /// </summary>
        /// <param name="sender">Container with the changed value</param>
        /// <param name="e">Change attributes</param>
        private void Container_OnValueChanged(object sender, EventArgs e)
        {
            textBoxValue_Update($"{scaled_value.GetValue()}");
            OnValueChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion // Container

        #region Label
        /// <summary>
        /// Label text on screen. (Will try to create space if required.)
        /// </summary>
        public string Label
        {
            get { return labelName.Text; }
            set { labelName.Text = value; }
        }
        #endregion // Label

        #region Value
        /// <summary>
        /// Event that fires on an accepted value change.
        /// </summary>
        public event EventHandler OnValueChanged;

        /// <summary>
        /// Value readonly or editable by user.
        /// </summary>
        public bool ValueReadonly
        {
            get { return textBoxValue.ReadOnly; }
            set { textBoxValue.ReadOnly = value; }
        }

        /// <summary>
        /// ValueConvert: Flag to activate value conversion on scale changes. (default: false)
        /// </summary>
        public bool ValueConvert { get; set; }

        /// <summary>
        /// Update value textbox with new data.
        /// </summary>
        /// <param name="value">New data to use.</param>
        private void textBoxValue_Update(string value)
        {
            if (textBoxValue.Text != value) textBoxValue.Text = value;
        }

        /// <summary>
        /// KeyUp: User releases a key, check for newline
        /// </summary>
        /// <param name="sender">Textbox with edited value</param>
        /// <param name="e">Change attributes</param>
        private void textBoxValue_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    textBoxValue.BackColor = (scaled_value.SetValue(textBoxValue.Text))
                        ? SystemColors.Window
                        : Color.PeachPuff;
                    break;
                default:
                    textBoxValue.BackColor = SystemColors.Window;
                    break;
            }
        }

        /// <summary>
        /// Validator for user changes on Value.
        /// </summary>
        /// <param name="sender">Textbox which holds the text version of the Value</param>
        /// <param name="e">Change event attricutes.</param>
        private void textBoxValue_Validating(object sender, CancelEventArgs e)
        {
            if (!scaled_value.SetValue(textBoxValue.Text))
            {
                textBoxValue_Update($"{scaled_value.GetValue()}");
            }
        }

        /// <summary>
        /// Selected value: current text version in de value text box.
        /// </summary>
        public string SelectedValue
        {
            get { return textBoxValue.Text; }
            set { if (value != null) textBoxValue.Text = value.Trim(); }
        }
        #endregion // Value

        #region Scale
        /// <summary>
        /// Initialise application with scale related data.
        /// </summary>
        void InitialiseScale()
        {
            var units = new List<string>();
            foreach (Unit u in Enum.GetValues(typeof(Unit)))
            {
                units.Add(u.ToString(append_brackets: true));
            }
            DataSource = units;
            DataSourceLocked = true;
        }

        /// <summary>
        /// Event that fires on an accepted scale change.
        /// </summary>
        public event EventHandler OnScaleChanged;

        /// <summary>
        /// Synchronise scale related data between the (value, scale) container and the scale selection box.
        /// </summary>
        /// <param name="container_locked">True if container data cannot change.</param>
        void SyncScales(bool container_locked = false)
        {
            if (scaled_value == null) return;

            var s1 = $"{scaled_value.GetScale()}";
            if (Items.Contains(s1))
            {
                // scale in container is available in datasource, select it.
                SelectedScale = s1;
                return;
            }

            if (container_locked)
            {
                // try selected scale from the datasourse
                var s2 = comboBoxScale_GetScale();
                if (s2 != null)
                {
                    // use current selected scale.
                    scaled_value.SetScale(s2);
                    return;
                }

                // find a valid scale from the datasourse
                foreach (string candidate in Items)
                {
                    if (string.IsNullOrWhiteSpace(candidate)) continue;
                    s2 = comboBoxScale_GetScale(candidate);
                    if (s2 != null)
                    {
                        s1 = candidate;
                        break;
                    }
                }
                if (s2 != null)
                {
                    // use first valid scale s1 from datasource
                    SelectedScale = s1;
                    scaled_value.SetScale(s2);
                    return;
                }
            }

            // edit container scale in combobox:
            comboBoxScale.Text = s1;

            // push container scale into datasource:
            //Items.Add(s1);
            //SelectedItem = s1;
        }

        /// <summary>
        /// Delegate for ScaleRejected events.
        /// </summary>
        /// <param name="sender">This instance</param>
        /// <param name="e">Attributes with the ScaleRejected event</param>
        public delegate void ScaleRejectedEventHandler(object sender, ScaleRejectedEventArgs e);

        /// <summary>
        /// OnScaleRejected: The (user changed) scale that is not accepted.
        /// </summary>
        public event ScaleRejectedEventHandler OnScaleRejected;

        /// <summary>
        /// Lock the ComboBox for Scales, the user is not able to edit scales if set true. (Default: false, the user can edit scales.)
        /// </summary>
        public bool DataSourceLocked
        {
            get { return (comboBoxScale.DropDownStyle == ComboBoxStyle.DropDownList); }
            set { comboBoxScale.DropDownStyle = (value) ? ComboBoxStyle.DropDownList : ComboBoxStyle.DropDown; }
        }

        /// <summary>
        /// Datasource for the Scales ComboBox.
        /// </summary>
        public object DataSource
        {
            get { return comboBoxScale.DataSource; }
            set
            {
                comboBoxScale.DataSource = value;
                SyncScales();
            }
        }

        /// <summary>
        /// DisplayMember for the Scales ComboBox.
        /// </summary>
        public string DisplayMember
        {
            get { return comboBoxScale.DisplayMember; }
            set { comboBoxScale.DisplayMember = value; }
        }

        /// <summary>
        /// ValueMember for the Scales ComboBox.
        /// </summary>
        public string ValueMember
        {
            get { return comboBoxScale.ValueMember; }
            set { comboBoxScale.ValueMember = value; }
        }

        /// <summary>
        /// Items in the Scales ComboBox.
        /// </summary>
        public ComboBox.ObjectCollection Items
        {
            get { return comboBoxScale.Items; }
        }

        /// <summary>
        /// SelectedIndex in the Scales ComboBox.
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public int SelectedIndex
        {
            get { return comboBoxScale.SelectedIndex; }
            set
            {
                if (comboBoxScale.SelectedIndex != value)
                {
                    comboBoxScale.SelectedIndex = value; // rejected when not in datasource.
                }
            }
        }

        /// <summary>
        /// SelectedItem in the Scales ComboBox.
        /// </summary>
        [System.ComponentModel.Bindable(true)]
        [System.ComponentModel.Browsable(false)]
        public object SelectedScale
        {
            get { return comboBoxScale.SelectedItem; }
            set
            {
                if (!comboBoxScale.SelectedItem.Equals(value))
                {
                    comboBoxScale.SelectedItem = value; // rejected when not in datasource.
                }
            }
        }

        /// <summary>
        /// Enable (or disable) the Scale combobox.
        /// </summary>
        public bool ScaleEnabled
        {
            get { return comboBoxScale.Enabled; }
            set { comboBoxScale.Enabled = value; }
        }

        /// <summary>
        /// Responce on a scale change, validate scale, optional convert value
        /// </summary>
        /// <param name="sender">The ComboBox for scales</param>
        /// <param name="e">Attributes with the scale change</param>
        private void comboBoxScale_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateScale();
        }

        /// <summary>
        /// Validate the current selected (and possibly changed by the user) for acceptance.
        /// </summary>
        /// <param name="sender">The ComboBox for scale values.</param>
        /// <param name="e">Attributes with the Validate event.</param>
        private void comboBoxScale_Validating(object sender, CancelEventArgs e)
        {
            var candidate = (DataSourceLocked)
                ? $"{comboBoxScale.SelectedItem}"
                : comboBoxScale.Text;
            if (!ValidateScale())
            {
                e.Cancel = true;
                OnScaleRejected?.Invoke(this, new ScaleRejectedEventArgs($"Scale rejected: '{candidate}'"));
            }
        }

        /// <summary>
        /// ValidateScale: Check if the new scale is acceptable, update internal state.
        /// </summary>
        /// <returns>True if the scale is accepted, false otherwise.</returns>
        private bool ValidateScale()
        {
            var scale = comboBoxScale_GetScale();
            if (scale == null) return false;

            if (scaled_value != null)
            {
                var v1 = (ValueConvert) ? scaled_value.Duplicate() : null;
                scaled_value.SetScale(scale);
                if (v1 != null) scaled_value.SetValueScaled(v1);
            }
            return true;
        }

        /// <summary>
        /// Convert a test representation to an actual scale instance.
        /// </summary>
        /// <param name="candidate">(Optional) text representation of a scale</param>
        /// <returns>Null is parsing fails, the scale instance otherwise.</returns>
        /// <remarks>
        /// If no candidate is given, the current selected item in the comboBox for Scales is used as candidate.
        /// </remarks>
        private Scale comboBoxScale_GetScale(string candidate = null)
        {
            if (candidate == null)
            {
                candidate = (DataSourceLocked)
                    ? $"{comboBoxScale.SelectedItem}"
                    : comboBoxScale.Text;
            }

            Scale scale;
            if (!Data.Scales.Scale.TryParse(candidate, out scale)) scale = null;
            return scale;
        }
        #endregion // Scale
    }

    public class ScaleRejectedEventArgs
    {
        public ScaleRejectedEventArgs(string text) { Text = text; }

        public string Text { get; }
    }
}
