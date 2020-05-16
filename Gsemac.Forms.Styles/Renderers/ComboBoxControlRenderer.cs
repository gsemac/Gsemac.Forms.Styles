using Gsemac.Forms.Styles.Extensions;
using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public class ComboBoxControlRenderer :
        ControlRendererBase<ComboBox> {

        // Public members

        public ComboBoxControlRenderer(IStyleSheetControlRenderer baseRenderer) {

            this.baseRenderer = baseRenderer;

        }

        public override void RenderControl(Graphics graphics, ComboBox control) {

            IRuleset ruleset = baseRenderer.GetRuleset(control);

            RenderUtilities.ApplyColorProperties(control, ruleset);

            Rectangle clientRect = control.ClientRectangle;

            baseRenderer.PaintBackground(graphics, control);

            // Match the foreground bounds of the default control.
            // The text is cut off behind the drop-down arrow.

            baseRenderer.PaintForeground(graphics, control, new Rectangle(clientRect.X + 1, clientRect.Y, clientRect.Width - 21, clientRect.Height));

            PaintDropDownArrow(graphics, control);

        }

        // Private members

        private readonly IStyleSheetControlRenderer baseRenderer;

        private void PaintDropDownArrow(Graphics graphics, ComboBox control) {

            INode controlNode = new ControlNode(control);
            IRuleset ruleset = baseRenderer.GetRuleset(new UserNode(string.Empty, "DropDownArrow", parent: controlNode, states: controlNode.States));

            // Create the arrow rectangle to match the bounds of the default control.

            Rectangle clientRect = control.ClientRectangle;
            Rectangle arrowRect = new Rectangle(clientRect.Right - 12, clientRect.Y + 9, 7, 6);

            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (Pen pen = new Pen(ruleset.Color?.Value ?? SystemColors.ControlText)) {

                pen.Width = 2.0f;
                pen.Alignment = PenAlignment.Center;
                pen.StartCap = LineCap.Flat;
                pen.EndCap = LineCap.Flat;

                PointF bottomMidpoint = new PointF(arrowRect.Left + arrowRect.Width / 2.0f, arrowRect.Bottom - 1);

                graphics.DrawLine(pen, new PointF(arrowRect.Left, arrowRect.Top), bottomMidpoint);
                graphics.DrawLine(pen, new PointF(arrowRect.Right, arrowRect.Top), bottomMidpoint);

            }

        }

    }

}