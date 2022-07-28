using Gsemac.Forms.Styles.StyleSheets.Properties;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers2 {

    internal static class ControlRenderUtilities {

        // Public members

        public static void ApplyColorProperties(Control control, IRuleset style) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            if (style is null)
                throw new ArgumentNullException(nameof(style));

            if (style.ContainsKey(PropertyName.BackgroundColor) && style.BackgroundColor != control.BackColor) {

                Color backColor = style.BackgroundColor;

                if (backColor.A != byte.MaxValue && !ControlUtilities.GetStyle(control, ControlStyles.SupportsTransparentBackColor))
                    backColor = Color.FromArgb(backColor.R, backColor.G, backColor.B);

                control.BackColor = backColor;

            }

            if (style.ContainsKey(PropertyName.Color) && style.Color != control.ForeColor)
                control.ForeColor = style.Color;

        }

        public static void DrawFocusRectangle(Graphics graphics, Rectangle rectangle, IRuleset style) {

            if (graphics is null)
                throw new ArgumentNullException(nameof(graphics));

            if (style is null)
                throw new ArgumentNullException(nameof(style));

            using (Pen pen = new Pen(style.Color)) {

                graphics.SmoothingMode = SmoothingMode.Default;

                pen.DashPattern = new float[] { 1, 1 };

                graphics.DrawRectangle(pen, rectangle);

            }

        }

    }

}