using Gsemac.Forms.Styles.Extensions;
using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Controls {

    public class UpDownButtonsControlRenderer :
        ControlRendererBase {

        // Public members

        public UpDownButtonsControlRenderer(IStyleSheet styleSheet) :
                base(styleSheet) {
        }

        public override void RenderControl(Graphics graphics, Control control) {

            Rectangle clientRect = control.ClientRectangle;

            clientRect.Offset(-1, 1);

            Rectangle topButtonRect = new Rectangle(clientRect.X, clientRect.Y, clientRect.Width, clientRect.Height / 2);
            Rectangle bottomButtonRect = new Rectangle(topButtonRect.X, clientRect.Y + clientRect.Height / 2, topButtonRect.Width, topButtonRect.Height);

            Rectangle topButtonClickRect = topButtonRect;
            Rectangle bottomButtonClickRect = bottomButtonRect;

            topButtonClickRect.Offset(0, -1);
            bottomButtonClickRect.Offset(0, -1);

            ClearBackground(graphics, control);

            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            PaintButton(graphics, topButtonRect, TriangleOrientation.Up, GetRuleset(new ControlNode(control, topButtonClickRect)));
            PaintButton(graphics, bottomButtonRect, TriangleOrientation.Down, GetRuleset(new ControlNode(control, bottomButtonClickRect)));

        }

        // Private members

        private void PaintButton(Graphics graphics, Rectangle clientRect, TriangleOrientation arrowOrientation, IRuleset ruleset) {

            PaintBackground(graphics, clientRect, ruleset);

            clientRect.Inflate(new Size(-5, -3));
            clientRect.Offset(1, 0);

            ColorProperty color = ruleset.GetProperty(PropertyType.Color) as ColorProperty;

            using (Brush brush = new SolidBrush(color?.Value ?? Color.Black))
                graphics.FillTriangle(brush, clientRect, arrowOrientation);

        }

    }

}