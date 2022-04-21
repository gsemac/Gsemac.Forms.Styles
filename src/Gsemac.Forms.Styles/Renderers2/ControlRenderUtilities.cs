using Gsemac.Forms.Styles.StyleSheets.Extensions;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers2 {

    internal static class ControlRenderUtilities {

        // Public members

        public static void ApplyColorProperties(Control control, IRuleset ruleset) {

            if (ruleset.BackgroundColor.HasValue() && ruleset.BackgroundColor.Value != control.BackColor) {

                Color backColor = ruleset.BackgroundColor.Value;

                if (backColor.A != byte.MaxValue && !ControlUtilities.GetStyle(control, ControlStyles.SupportsTransparentBackColor))
                    backColor = Color.FromArgb(backColor.R, backColor.G, backColor.B);

                control.BackColor = backColor;

            }

            if (ruleset.Color.HasValue() && ruleset.Color.Value != control.ForeColor)
                control.ForeColor = ruleset.Color.Value;

        }

    }

}