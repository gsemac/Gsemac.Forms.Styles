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