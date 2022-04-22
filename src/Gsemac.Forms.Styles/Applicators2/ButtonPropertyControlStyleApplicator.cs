using Gsemac.Forms.Styles.StyleSheets.Properties;
using Gsemac.Forms.Styles.StyleSheets.Properties.Extensions;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
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

            double borderWidth = style.Where(p => PropertyUtilities.IsBorderWidthProperty(p))
                 .Select(p => p.GetValueAs<double>())
                 .LastOrDefault();

            Color borderColor = style.Where(p => PropertyUtilities.IsBorderColorProperty(p))
                 .Select(p => p.GetValueAs<Color>())
                 .LastOrDefault();

            button.FlatAppearance.BorderColor = borderColor;
            button.FlatAppearance.BorderSize = (int)borderWidth;

        }

    }

}