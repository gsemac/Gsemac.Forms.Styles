using Gsemac.Drawing;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers2 {

    internal class RadioButtonRenderer :
        CheckBoxRendererBase<RadioButton> {

        // Protected members

        protected override void DrawCheck(RadioButton control, IRenderContext context) {

            // Draw the background.

            Rectangle checkBoxRect = new Rectangle(
                context.ClientRectangle.X,
                context.ClientRectangle.Y + (int)(context.ClientRectangle.Height / 2.0f - CheckBoxWidth / 2.0f),
                CheckBoxWidth,
                CheckBoxWidth
            );

            Color baseColor = context.Style.AccentColor;
            double perceivedLightness = ColorUtilities.ComputePerceivedLightness(baseColor);

            Color backgroundColor = baseColor;
            Color outlineColor = ColorUtilities.Shade(baseColor, 0.5f);
            Color checkColor = perceivedLightness > 0.5 ? Color.Black : Color.White;

            context.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            //context.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using (Brush brush = new SolidBrush(backgroundColor))
                context.Graphics.FillEllipse(brush, checkBoxRect);

            using (Pen pen = new Pen(outlineColor))
                context.Graphics.DrawEllipse(pen, checkBoxRect);

            // Draw the check.

            if (control.Checked) {

                using (Brush brush = new SolidBrush(checkColor)) {

                    checkBoxRect.Inflate(-3, -3);

                    context.Graphics.FillEllipse(brush, checkBoxRect);

                }

            }

        }

        protected override Rectangle GetTextRectangle(RadioButton control, IRenderContext context) {

            Rectangle textRect = base.GetTextRectangle(control, context);

            return new Rectangle(textRect.X, textRect.Y + 1, textRect.Width, textRect.Height);

        }

    }

}