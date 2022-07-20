using Gsemac.Forms.Styles.StyleSheets.Properties;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers2 {

    internal static class ControlRenderUtilities {

        // Public members

        public static void ApplyColorProperties(Control control, IRuleset ruleset) {

            if (ruleset.ContainsKey(PropertyName.BackgroundColor) && ruleset.BackgroundColor != control.BackColor) {

                Color backColor = ruleset.BackgroundColor;

                if (backColor.A != byte.MaxValue && !ControlUtilities.GetStyle(control, ControlStyles.SupportsTransparentBackColor))
                    backColor = Color.FromArgb(backColor.R, backColor.G, backColor.B);

                control.BackColor = backColor;

            }

            if (ruleset.ContainsKey(PropertyName.Color) && ruleset.Color != control.ForeColor)
                control.ForeColor = ruleset.Color;

        }

    }

}