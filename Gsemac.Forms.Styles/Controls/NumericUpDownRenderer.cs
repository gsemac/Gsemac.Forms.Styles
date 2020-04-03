using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Controls {

    public class NumericUpDownRenderer :
        ControlRendererBase<NumericUpDown> {

        // Public members

        public NumericUpDownRenderer(IStyleSheet styleSheet) :
            base(styleSheet) {
        }

        public override void RenderControl(Graphics graphics, NumericUpDown control) {

            IRuleset ruleset = GetRuleset(control);

            // Update the color of the NumericUpdateDown, which updates the color of the UpDownEdit (inheriting from TextBox).

            SetColorProperties(control, ruleset);

            // Like TextBoxes, NumericUpDowns are 23 pixels high.
            // Because the NumericUpDown has BorderStyle.None, we need to adjust it to look like a bordered control.

            Rectangle clientRect = control.ClientRectangle;

            int x = clientRect.X - 2;
            int y = clientRect.Y - 2;
            int w = clientRect.Width + 3;
            int h = clientRect.Height + 4;

            Rectangle drawRect = new Rectangle(x, y, w, h);

            PaintBackground(graphics, drawRect, ruleset);

        }

    }

}