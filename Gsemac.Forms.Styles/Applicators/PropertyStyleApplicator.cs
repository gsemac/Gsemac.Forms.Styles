using Gsemac.Forms.Styles.Controls;
using Gsemac.Forms.Styles.StyleSheets;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators {

    public class PropertyStyleApplicator :
        StyleApplicatorBase {

        // Public members

        public PropertyStyleApplicator(IStyleSheet styleSheet) {

            this.styleSheet = styleSheet;

        }

        // Protected members

        protected override bool HasStyles(Control control) {

            return styleSheet.GetRuleset(new ControlNode(control)).Any();

        }
        protected override void OnApplyStyles(Control control) {

            IRuleset rules = styleSheet.GetRuleset(new ControlNode(control));

            if (rules.GetProperty(PropertyType.BackgroundColor) is ColorProperty backgroundColor)
                control.BackColor = backgroundColor.Value;

            if (rules.GetProperty(PropertyType.Color) is ColorProperty color)
                control.ForeColor = color.Value;

            switch (control) {

                case Button button:

                    ApplyStyles(button, rules);

                    break;

                case ListBox listBox:

                    ApplyStyles(listBox, rules);

                    break;

                case NumericUpDown numericUpDown:

                    ApplyStyles(numericUpDown, rules);

                    break;

                case TextBox textBox:

                    if (!(textBox.Parent is NumericUpDown _))
                        ApplyStyles(textBox, rules);

                    break;

            }

        }
        protected override void OnClearStyles(Control control) {

            control.BackColor = SystemColors.Control;
            control.ForeColor = SystemColors.ControlText;

            switch (control) {

                case Button button:

                    button.FlatStyle = FlatStyle.Standard;

                    break;

                case CheckBox checkBox:

                    checkBox.BackColor = Color.Transparent;

                    break;

                case ComboBox comboBox:

                    comboBox.BackColor = SystemColors.Window;

                    break;

                case Label label:

                    label.BackColor = Color.Transparent;

                    break;

                case ListBox listBox:

                    listBox.BackColor = SystemColors.Window;

                    break;

                case NumericUpDown numericUpDown:

                    numericUpDown.BackColor = SystemColors.Window;
                    numericUpDown.BorderStyle = BorderStyle.Fixed3D;

                    break;

                case RadioButton radioButton:

                    radioButton.BackColor = Color.Transparent;

                    break;

                case TabPage tabPage:

                    tabPage.BackColor = SystemColors.ControlLightLight;

                    break;

                case TextBox textBox:

                    textBox.BackColor = SystemColors.Window;

                    if (!(textBox.Parent is NumericUpDown _))
                        textBox.BorderStyle = BorderStyle.Fixed3D;

                    break;

            }

        }

        // Private members

        private readonly IStyleSheet styleSheet;

        private void ApplyStyles(Button button, IRuleset rules) {

            button.FlatStyle = FlatStyle.Flat;

            if (rules.GetProperty(PropertyType.BorderColor) is ColorProperty borderColor)
                button.FlatAppearance.BorderColor = borderColor.Value;

            if (rules.GetProperty(PropertyType.BorderWidth) is NumericProperty borderWidth)
                button.FlatAppearance.BorderSize = (int)borderWidth.Value;

        }
        private void ApplyStyles(ListBox listBox, IRuleset rules) {

            if (rules.GetProperty(PropertyType.BorderWidth) is NumericProperty borderWidth) {

                if (borderWidth.Value <= 0)
                    listBox.BorderStyle = BorderStyle.None;
                else
                    listBox.BorderStyle = BorderStyle.FixedSingle;

            }

        }
        private void ApplyStyles(NumericUpDown numericUpDown, IRuleset rules) {

            if (rules.GetProperty(PropertyType.BorderWidth) is NumericProperty borderWidth) {

                if (borderWidth.Value <= 0)
                    numericUpDown.BorderStyle = BorderStyle.None;
                else
                    numericUpDown.BorderStyle = BorderStyle.FixedSingle;

            }

        }
        private void ApplyStyles(TextBox textBox, IRuleset rules) {

            if (rules.GetProperty(PropertyType.BorderWidth) is NumericProperty borderWidth) {

                if (borderWidth.Value <= 0)
                    textBox.BorderStyle = BorderStyle.None;
                else
                    textBox.BorderStyle = BorderStyle.FixedSingle;

            }

        }

    }

}