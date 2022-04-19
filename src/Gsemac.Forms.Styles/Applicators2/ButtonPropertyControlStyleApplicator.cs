using Gsemac.Forms.Styles.Dom;
using Gsemac.Forms.Styles.StyleSheets;
using Gsemac.Forms.Styles.StyleSheets.Extensions;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators2 {

    public class ButtonPropertyControlStyleApplicator :
         PropertyControlStyleApplicator<Button> {

        // Public members

        public override void ApplyStyle(Button button, IRuleset style) {

            base.ApplyStyle(button, style);

            button.FlatStyle = FlatStyle.Flat;

            double borderWidth = style.Where(p => p.IsBorderWidthProperty())
                 .Cast<NumberProperty>()
                 .Select(p => p.Value)
                 .LastOrDefault();

            Color borderColor = style.Where(p => p.IsBorderColorProperty())
                 .Cast<ColorProperty>()
                 .Select(p => p.Value)
                 .LastOrDefault();

            button.FlatAppearance.BorderColor = borderColor;
            button.FlatAppearance.BorderSize = (int)borderWidth;

        }

    }

}