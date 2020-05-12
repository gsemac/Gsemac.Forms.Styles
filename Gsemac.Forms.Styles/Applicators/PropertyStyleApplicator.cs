using Gsemac.Forms.Styles.Extensions;
using Gsemac.Forms.Styles.StyleSheets;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators {

    public class PropertyStyleApplicator :
        StyleSheetStyleApplicatorBase {

        // Public members

        public PropertyStyleApplicator(IStyleSheet styleSheet) :
            base(styleSheet) {
        }

        // Protected members

        protected override void OnApplyStyles(Control control) {

            IRuleset rules = StyleSheet.GetRuleset(new ControlNode(control));

            if (rules.BackgroundColor.HasValue())
                control.BackColor = rules.BackgroundColor.Value;

            if (rules.Color.HasValue())
                control.ForeColor = rules.Color.Value;

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

        private void ApplyStyles(Button button, IRuleset rules) {

            button.FlatStyle = FlatStyle.Flat;

            if (rules.BorderColor.HasValue())
                button.FlatAppearance.BorderColor = rules.BorderColor.Value;

            if (rules.BorderWidth.HasValue())
                button.FlatAppearance.BorderSize = (int)rules.BorderWidth.Value;

        }
        private void ApplyStyles(ListBox listBox, IRuleset rules) {

            if (rules.BorderWidth.HasValue()) {

                if (rules.BorderWidth.Value <= 0)
                    listBox.BorderStyle = BorderStyle.None;
                else
                    listBox.BorderStyle = BorderStyle.FixedSingle;

            }

        }
        private void ApplyStyles(NumericUpDown numericUpDown, IRuleset rules) {

            if (rules.BorderWidth.HasValue()) {

                if (rules.BorderWidth.Value <= 0)
                    numericUpDown.BorderStyle = BorderStyle.None;
                else
                    numericUpDown.BorderStyle = BorderStyle.FixedSingle;

            }

        }
        private void ApplyStyles(TextBox textBox, IRuleset rules) {

            if (rules.BorderWidth.HasValue()) {

                if (rules.BorderWidth.Value <= 0)
                    textBox.BorderStyle = BorderStyle.None;
                else
                    textBox.BorderStyle = BorderStyle.FixedSingle;

            }

        }

    }

}