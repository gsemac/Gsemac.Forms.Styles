using System;
using System.Drawing;
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

        private const int CheckBoxWidth = 13;

        private void DrawCheckBox(CheckBox checkBox, IRenderContext context) {
         
            Color baseColor = context.Style.AccentColor;

            Rectangle clientRect = context.ClientRectangle;

            Rectangle checkRect = new Rectangle(
                clientRect.X,
                clientRect.Y + (int)(clientRect.Height / 2.0f - CheckBoxWidth / 2.0f) - 1,
                CheckBoxWidth,
                CheckBoxWidth
                );

            //e.StyleRenderer.PaintBackground(e.Graphics, checkRect, ruleset);
            //e.StyleRenderer.PaintBorder(e.Graphics, checkRect, ruleset);

            //// Draw the checkmark.

            //if (control.Checked) {

            //    using (Pen pen = new Pen(ruleset.Color?.Value ?? SystemColors.ControlText)) {

            //        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            //        pen.Alignment = PenAlignment.Center;
            //        pen.Width = 2.0f;
            //        pen.StartCap = LineCap.Square;
            //        pen.EndCap = LineCap.Square;

            //        e.Graphics.DrawLine(pen, checkRect.X + 3, checkRect.Y + checkRect.Height / 2.0f, checkRect.X + checkRect.Width / 2.0f - 1, checkRect.Y + checkRect.Height - 5);
            //        e.Graphics.DrawLine(pen, checkRect.X + checkRect.Width / 2.0f - 1, checkRect.Y + checkRect.Height - 5, checkRect.X + checkRect.Width - 4, checkRect.Y + 3);

            //    }

            //}

        }
        private void DrawText(CheckBox checkBox, IRenderContext context) {

            Rectangle clientRect = context.ClientRectangle;

            Rectangle textRect = new Rectangle(
                clientRect.X + CheckBoxWidth + 3,
                clientRect.Y - 1,
                clientRect.Width,
                clientRect.Height
                );

            context.DrawText(textRect, checkBox.Text, checkBox.Font, ControlUtilities.GetTextFormatFlags(checkBox.TextAlign));

        }

    }

}