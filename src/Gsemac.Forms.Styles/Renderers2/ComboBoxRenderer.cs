using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Remoting.Contexts;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers2 {

    internal class ComboBoxRenderer :
        StyleRendererBase<ComboBox> {

        // Public members

        public override void Render(ComboBox control, IRenderContext context) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            if (context is null)
                throw new ArgumentNullException(nameof(context));

            RenderUtilities.ApplyColorProperties(control, context.Style);

            Rectangle clientRect = control.ClientRectangle;

            context.DrawBackground();

            // Match the foreground bounds of the default control.
            // The text is cut off behind the drop-down arrow.

            Rectangle textRect = new Rectangle(clientRect.X + 1, clientRect.Y, clientRect.Width - 21, clientRect.Height);

            context.DrawText(textRect, control.Text, control.Font);

            PaintDropDownArrow(control, context);

            context.DrawBorder();

        }

        // Private members

        private void PaintDropDownArrow(ComboBox control, IRenderContext context) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            if (context is null)
                throw new ArgumentNullException(nameof(context));

            // Create the arrow rectangle to match the bounds of the default control.

            Rectangle clientRect = control.ClientRectangle;
            Rectangle arrowRect = new Rectangle(clientRect.Right - 12, clientRect.Y + 9, 7, 6);

            context.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (Pen pen = new Pen(context.Style.AccentColor)) {

                pen.Width = 2.0f;
                pen.Alignment = PenAlignment.Center;
                pen.StartCap = LineCap.Flat;
                pen.EndCap = LineCap.Flat;

                PointF bottomMidpoint = new PointF(arrowRect.Left + arrowRect.Width / 2.0f, arrowRect.Bottom - 1);

                context.Graphics.DrawLine(pen, new PointF(arrowRect.Left, arrowRect.Top), bottomMidpoint);
                context.Graphics.DrawLine(pen, new PointF(arrowRect.Right, arrowRect.Top), bottomMidpoint);

            }

        }

    }

}