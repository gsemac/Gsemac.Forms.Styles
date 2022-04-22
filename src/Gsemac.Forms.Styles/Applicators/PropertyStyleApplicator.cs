using Gsemac.Forms.Styles.Dom;
using Gsemac.Forms.Styles.StyleSheets;
using Gsemac.Forms.Styles.StyleSheets.Extensions;
using Gsemac.Forms.Styles.StyleSheets.Properties.Extensions;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators {

    public class PropertyStyleApplicator :
        StyleApplicatorBase {

        // Public members

        public PropertyStyleApplicator(IStyleSheet styleSheet, StyleApplicatorOptions options = StyleApplicatorOptions.Default) :
            base(styleSheet, options) {
        }

        // Protected members

        protected override void OnApplyStyles(Control control) {

            //IRuleset rules = StyleSheet.GetRuleset(new ControlNode2(control));

            //if (rules.BackgroundColor.HasValue())
            //    control.BackColor = rules.BackgroundColor.Value;

            //if (rules.Color.HasValue())
            //    control.ForeColor = rules.Color.Value;

            //switch (control) {

            //    case Button button:

            //        ApplyStyles(button, rules);

            //        break;

            //    case ListBox listBox:

            //        ApplyStyles(listBox, rules);

            //        break;

            //    case NumericUpDown numericUpDown:

            //        ApplyStyles(numericUpDown, rules);

            //        break;

            //    case TextBox textBox:

            //        if (!(textBox.Parent is NumericUpDown _))
            //            ApplyStyles(textBox, rules);

            //        break;

            //}

        }

        // Private members

        private void ApplyStyles(Button button, IRuleset rules) {

            //button.FlatStyle = FlatStyle.Flat;

            //double borderWidth = rules.Where(p => p.IsBorderWidthProperty())
            //     .Cast<MeasurementProperty>()
            //     .Select(p => p.Value)
            //     .LastOrDefault();

            //Color borderColor = rules.Where(p => p.IsBorderColorProperty())
            //     .Cast<ColorProperty>()
            //     .Select(p => p.Value)
            //     .LastOrDefault();

            //if (borderWidth > 0) {

            //    button.FlatAppearance.BorderColor = borderColor;
            //    button.FlatAppearance.BorderSize = (int)borderWidth;

            //}

        }
        private void ApplyStyles(ListBox listBox, IRuleset rules) {

            //double borderWidth = rules.Where(p => p.IsBorderWidthProperty())
            //     .Cast<MeasurementProperty>()
            //     .Select(p => p.Value)
            //     .LastOrDefault();

            //if (borderWidth <= 0)
            //    listBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            //else
            //    listBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

        }
        private void ApplyStyles(NumericUpDown numericUpDown, IRuleset rules) {

            //double borderWidth = rules.Where(p => p.IsBorderWidthProperty())
            //   .Cast<MeasurementProperty>()
            //   .Select(p => p.Value)
            //   .LastOrDefault();

            //if (borderWidth <= 0)
            //    numericUpDown.BorderStyle = System.Windows.Forms.BorderStyle.None;
            //else
            //    numericUpDown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

        }
        private void ApplyStyles(TextBox textBox, IRuleset rules) {

            //double borderWidth = rules.Where(p => p.IsBorderWidthProperty())
            //  .Cast<MeasurementProperty>()
            //  .Select(p => p.Value)
            //  .LastOrDefault();

            //if (borderWidth <= 0)
            //    textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            //else
            //    textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

        }

    }

}