using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BorderStyle = Gsemac.Forms.Styles.StyleSheets.Properties.BorderStyle;

namespace Gsemac.Forms.Styles.Applicators2 {

    public class ButtonStyleApplicator :
         ControlStyleApplicator<Button> {

        // Public members

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