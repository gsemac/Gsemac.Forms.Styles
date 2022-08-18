using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BorderStyle = Gsemac.Forms.Styles.StyleSheets.Properties.BorderStyle;

namespace Gsemac.Forms.Styles.Applicators2 {

    internal class ButtonPropertyStyleApplicator :
         ControlPropertyStyleApplicator<Button> {

        // Public members

        public override void InitializeStyle(Button button) {

            // Flat buttons are slightly larger than regular buttons.

            ControlUtilities2.Inflate(button, -1, -1);

        }
        public override void ApplyStyle(Button button, IRuleset style) {

            base.ApplyStyle(button, style);

            button.FlatStyle = FlatStyle.Flat;

            BorderStyle borderStyle = style.BorderStyle.Where(s => s != BorderStyle.None).LastOrDefault();
            double borderWidth = borderStyle == BorderStyle.None ? 0 : style.BorderWidth.Max(width => width.Value);
            Color borderColor = style.BorderColor.Where(color => color != style.Color).LastOrDefault();

            button.FlatAppearance.BorderColor = borderColor;
            button.FlatAppearance.BorderSize = (int)borderWidth;

        }

    }

}