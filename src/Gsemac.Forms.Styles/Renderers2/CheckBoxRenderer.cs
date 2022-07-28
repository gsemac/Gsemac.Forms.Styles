using Gsemac.Drawing;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers2 {

    public class CheckBoxRenderer :
        StyleRendererBase<CheckBox> {

        // Public members

        public override void Render(CheckBox checkBox, IRenderContext context) {

            if (checkBox is null)
                throw new ArgumentNullException(nameof(checkBox));

            if (context is null)
                throw new ArgumentNullException(nameof(context));

            context.Clear();

            context.DrawBackground();

            DrawCheckBox(checkBox, context);

            DrawText(checkBox, context);

            context.DrawBorder();

        }

        // Private members

        private const int CheckBoxWidth = 12;

        private void DrawCheckBox(CheckBox checkBox, IRenderContext context) {

            // Draw the background.

            Rectangle checkBoxRect = new Rectangle(
                context.ClientRectangle.X,
                context.ClientRectangle.Y + (int)(context.ClientRectangle.Height / 2.0f - CheckBoxWidth / 2.0f) - 1,
                CheckBoxWidth,
                CheckBoxWidth
            );

            Color baseColor = context.Style.AccentColor;

            Color backgroundColor = baseColor;
            Color outlineColor = ColorUtilities.Shade(backgroundColor, 0.5f);
            Color checkColor = ColorUtilities.Shade(backgroundColor, 0.5f);

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
        private void DrawText(CheckBox checkBox, IRenderContext context) {

            int textXOffset = 4;
            int textYOffset = -1;

            Rectangle textRect = new Rectangle(
                context.ClientRectangle.X + CheckBoxWidth + textXOffset,
                context.ClientRectangle.Y + textYOffset,
                context.ClientRectangle.Width,
                context.ClientRectangle.Height
            );

            context.DrawText(textRect, checkBox.Text, checkBox.Font, ControlUtilities.GetTextFormatFlags(checkBox.TextAlign));

        }

    }

}