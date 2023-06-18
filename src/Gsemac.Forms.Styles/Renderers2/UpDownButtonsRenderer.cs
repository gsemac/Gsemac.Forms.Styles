using Gsemac.Forms.Styles.Renderers.Extensions;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers2 {

    internal class UpDownButtonsRenderer :
        StyleRendererBase<Control> {

        // Public members

        public override void Render(Control upDownButtons, IRenderContext context) {

            if (upDownButtons is null)
                throw new ArgumentNullException(nameof(upDownButtons));

            if (context is null)
                throw new ArgumentNullException(nameof(context));

            Rectangle clientRect = context.ClientRectangle;

            clientRect.Offset(-1, 1);

            Rectangle topButtonRect = new Rectangle(clientRect.X + 3, clientRect.Y + 1, clientRect.Width - 2, clientRect.Height / 2);
            Rectangle bottomButtonRect = new Rectangle(topButtonRect.X, clientRect.Y + clientRect.Height / 2, topButtonRect.Width, topButtonRect.Height);

            context.Clear();

            context.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            PaintButton(context, topButtonRect, TriangleOrientation.Up);
            PaintButton(context, bottomButtonRect, TriangleOrientation.Down);

        }

        // Private members

        private void PaintButton(IRenderContext context, Rectangle buttonRect, TriangleOrientation arrowOrientation) {

            if (context is null)
                throw new ArgumentNullException(nameof(context));

            Rectangle backgroundRect = new Rectangle(buttonRect.X, buttonRect.Y, buttonRect.Width - 1, buttonRect.Height - 1);

            context.DrawBackground(backgroundRect);
            context.DrawBorder(buttonRect);

            Rectangle triangleRect = backgroundRect;

            triangleRect.Inflate(-3, -2);

            using (Brush brush = new SolidBrush(context.Style.AccentColor))
                context.Graphics.FillTriangle(brush, triangleRect, arrowOrientation);

        }

    }

}