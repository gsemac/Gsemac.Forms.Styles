using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System.Linq;
using System.Windows.Forms;
using BorderStyle = Gsemac.Forms.Styles.StyleSheets.Properties.BorderStyle;

namespace Gsemac.Forms.Styles.Applicators2 {

    public class TextBoxStyleApplicator :
         ControlStyleApplicator<TextBox> {

        // Public members

        public override void ApplyStyle(TextBox textBox, IRuleset style) {

            // Do not attempt to style TextBox instances nested inside of NumericUpDown controls.

            if (textBox.Parent is NumericUpDown)
                return;

            base.ApplyStyle(textBox, style);

            BorderStyle borderStyle = style.BorderStyle.Where(s => s != BorderStyle.None).LastOrDefault();
            double borderWidth = borderStyle == BorderStyle.None ? 0 : style.BorderWidth.Max(width => width.Value);

            if (borderWidth <= 0 || borderStyle == BorderStyle.None)
                textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            else
                textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

        }

    }

}