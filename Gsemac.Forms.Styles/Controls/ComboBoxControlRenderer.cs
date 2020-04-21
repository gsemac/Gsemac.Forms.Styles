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

    public class ComboBoxControlRenderer :
        ControlRendererBase<ComboBox> {

        // Public members

        public ComboBoxControlRenderer(IStyleSheet styleSheet) :
            base(styleSheet) { }

        public override void RenderControl(Graphics graphics, ComboBox control) {

            IRuleset ruleset = GetRuleset(control);

            SetColorProperties(control, ruleset);

            Rectangle clientRect = control.ClientRectangle;

            PaintBackground(graphics, control);

            // Match the foreground bounds of the default control.
            // The text is cut off behind the drop-down arrow.

            PaintForeground(graphics, control, new Rectangle(clientRect.X + 1, clientRect.Y, clientRect.Width - 21, clientRect.Height));

            PaintDropDownArrow(graphics, control);

        }

        // Private members

        private void PaintDropDownArrow(Graphics graphics, ComboBox control) {

            INode controlNode = new ControlNode(control);
            IRuleset ruleset = GetRuleset(new Node("DropDownArrow", parent: controlNode, states: controlNode.States));

            // Create the arrow rectangle to match the bounds of the default control.

            Rectangle clientRect = control.ClientRectangle;
            Rectangle arrowRect = new Rectangle(clientRect.Right - 12, clientRect.Y + 9, 7, 6);

            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (Pen pen = new Pen(ruleset.Color?.Value ?? SystemColors.ControlText)) {

                pen.Width = 2.0f;
                pen.Alignment = PenAlignment.Center;
                pen.StartCap = LineCap.Flat;
                pen.EndCap = LineCap.Flat;

                PointF bottomMidpoint = new PointF(arrowRect.Left + (arrowRect.Width / 2.0f), arrowRect.Bottom - 1);

                graphics.DrawLine(pen, new PointF(arrowRect.Left, arrowRect.Top), bottomMidpoint);
                graphics.DrawLine(pen, new PointF(arrowRect.Right, arrowRect.Top), bottomMidpoint);

            }

        }

    }

}