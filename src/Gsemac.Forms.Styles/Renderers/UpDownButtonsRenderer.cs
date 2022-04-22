using Gsemac.Forms.Styles.Dom;
using Gsemac.Forms.Styles.Renderers.Extensions;
using Gsemac.Forms.Styles.StyleSheets.Extensions;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public class UpDownButtonsRenderer :
        ControlRendererBase {

        // Public members

        public override void PaintControl(Control control, ControlPaintArgs e) {

            Rectangle clientRect = control.ClientRectangle;

            clientRect.Offset(-1, 1);

            Rectangle topButtonRect = new Rectangle(clientRect.X, clientRect.Y, clientRect.Width, clientRect.Height / 2);
            Rectangle bottomButtonRect = new Rectangle(topButtonRect.X, clientRect.Y + clientRect.Height / 2, topButtonRect.Width, topButtonRect.Height);

            Rectangle topButtonClickRect = topButtonRect;
            Rectangle bottomButtonClickRect = bottomButtonRect;

            topButtonClickRect.Offset(0, -1);
            bottomButtonClickRect.Offset(0, -1);

            e.Clear();

            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

          //  PaintButton(e, topButtonRect, TriangleOrientation.Up, e.StyleSheet.GetRuleset(new ControlNode2(control, topButtonClickRect)));
          //  PaintButton(e, bottomButtonRect, TriangleOrientation.Down, e.StyleSheet.GetRuleset(new ControlNode2(control, bottomButtonClickRect)));

        }

        // Private members

        private void PaintButton(ControlPaintArgs e, Rectangle clientRect, TriangleOrientation arrowOrientation, IRuleset ruleset) {

            //e.StyleRenderer.PaintBackground(e.Graphics, clientRect, ruleset);
            //e.StyleRenderer.PaintBorder(e.Graphics, clientRect, ruleset);

            //clientRect.Inflate(new Size(-5, -3));
            //clientRect.Offset(1, 0);

            //using (Brush brush = new SolidBrush(ruleset.Color?.Value ?? Color.Black))
            //    e.Graphics.FillTriangle(brush, clientRect, arrowOrientation);

        }

    }

}