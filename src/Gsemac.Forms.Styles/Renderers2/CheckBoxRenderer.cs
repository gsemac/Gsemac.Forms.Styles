using Gsemac.Drawing;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers2 {

    public class CheckBoxRenderer :
        CheckBoxRendererBase<CheckBox> {

        // Protected members

        protected override void DrawCheck(CheckBox checkBox, IRenderContext context) {

            // Draw the background.

            Rectangle checkBoxRect = new Rectangle(
                context.ClientRectangle.X,
                context.ClientRectangle.Y + (int)(context.ClientRectangle.Height / 2.0f - CheckBoxWidth / 2.0f) - 1,
                CheckBoxWidth,
                CheckBoxWidth
            );

            Color baseColor = context.Style.AccentColor;
            double perceivedLightness = ColorUtilities.ComputePerceivedLightness(baseColor);

            Color backgroundColor = baseColor;
            Color outlineColor = ColorUtilities.Shade(baseColor, 0.5f);
            Color checkColor = perceivedLightness > 0.5 ? Color.Black : Color.White;

            using (Brush brush = new SolidBrush(backgroundColor))
                context.Graphics.FillRectangle(brush, checkBoxRect);

            using (Pen pen = new Pen(outlineColor))
                context.Graphics.DrawRectangle(pen, checkBoxRect);

            // Draw the checkmark.

            if (checkBox.Checked) {

                using (Pen pen = new Pen(checkColor)) {

                    context.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                    pen.Alignment = PenAlignment.Center;
                    pen.Width = 2.0f;
                    pen.StartCap = LineCap.Square;
                    pen.EndCap = LineCap.Square;

                    context.Graphics.DrawLine(pen, checkBoxRect.X + 3, checkBoxRect.Y + checkBoxRect.Height / 2.0f, checkBoxRect.X + checkBoxRect.Width / 2.0f, checkBoxRect.Y + checkBoxRect.Height - 4);
                    context.Graphics.DrawLine(pen, checkBoxRect.X + checkBoxRect.Width / 2.0f, checkBoxRect.Y + checkBoxRect.Height - 4, checkBoxRect.X + checkBoxRect.Width - 3, checkBoxRect.Y + 3);

                }

            }

        }

    }

}