using Gsemac.Forms.Styles.Dom;
using Gsemac.Forms.Styles.Renderers2;
using Gsemac.Forms.Styles.StyleSheets.Extensions;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public class ComboBoxRenderer :
        ControlRendererBase<ComboBox> {

        // Public members

        public override void PaintControl(ComboBox control, ControlPaintArgs e) {

            IRuleset ruleset = e.StyleSheet.GetRuleset(control);

            RenderUtilities.ApplyColorProperties(control, ruleset);

            Rectangle clientRect = control.ClientRectangle;

            e.PaintBackground();

            // Match the foreground bounds of the default control.
            // The text is cut off behind the drop-down arrow.

            Rectangle textRect = new Rectangle(clientRect.X + 1, clientRect.Y, clientRect.Width - 21, clientRect.Height);

            e.PaintText(textRect);

            PaintDropDownArrow(control, e);

            e.PaintBorder();

        }

        // Private members

        private void PaintDropDownArrow(ComboBox control, ControlPaintArgs e) {

            //INode controlNode = new ControlNode(control);
            //UserNode dropDownArrowNode = new UserNode(string.Empty, new[] { "DropDownArrow" });

            //dropDownArrowNode.SetParent(controlNode);
            //dropDownArrowNode.SetStates(controlNode.States);

            //IRuleset ruleset = e.StyleSheet.GetRuleset(dropDownArrowNode);

            //// Create the arrow rectangle to match the bounds of the default control.

            //Rectangle clientRect = control.ClientRectangle;
            //Rectangle arrowRect = new Rectangle(clientRect.Right - 12, clientRect.Y + 9, 7, 6);

            //e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            //using (Pen pen = new Pen(ruleset.Color?.Value ?? SystemColors.ControlText)) {

            //    pen.Width = 2.0f;
            //    pen.Alignment = PenAlignment.Center;
            //    pen.StartCap = LineCap.Flat;
            //    pen.EndCap = LineCap.Flat;

            //    PointF bottomMidpoint = new PointF(arrowRect.Left + arrowRect.Width / 2.0f, arrowRect.Bottom - 1);

            //    e.Graphics.DrawLine(pen, new PointF(arrowRect.Left, arrowRect.Top), bottomMidpoint);
            //    e.Graphics.DrawLine(pen, new PointF(arrowRect.Right, arrowRect.Top), bottomMidpoint);

            //}

        }

    }

}