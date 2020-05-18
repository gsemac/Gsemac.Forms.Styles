using Gsemac.Forms.Styles.Extensions;
using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public class NumericUpDownControlRenderer :
        ControlRendererBase<NumericUpDown> {

        // Public members

        public NumericUpDownControlRenderer(IStyleSheetControlRenderer baseRenderer) {

            this.baseRenderer = baseRenderer;

        }

        public override void RenderControl(Graphics graphics, NumericUpDown control) {

            IRuleset ruleset = baseRenderer.GetRuleset(control);

            // Update the color of the NumericUpdateDown, which updates the color of the UpDownEdit (inheriting from TextBox).

            RenderUtilities.ApplyColorProperties(control, ruleset);

            // Like TextBoxes, NumericUpDowns are 23 pixels high.
            // Because the NumericUpDown has BorderStyle.None, we need to adjust it to look like a bordered control.

            Rectangle clientRect = control.ClientRectangle;

            int x = clientRect.X - 2;
            int y = clientRect.Y - 2;
            int w = clientRect.Width + 3;
            int h = clientRect.Height + 4;

            double topWidth = ruleset.BorderTopWidth?.Value ?? 0;
            double rightWidth = ruleset.BorderRightWidth?.Value ?? 0;
            double bottomWidth = ruleset.BorderBottomWidth?.Value ?? 0;
            double leftWidth = ruleset.BorderLeftWidth?.Value ?? 0;

            if (topWidth <= 1)
                topWidth = 0;

            if (rightWidth <= 1)
                rightWidth = 0;

            if (bottomWidth <= 1)
                bottomWidth = 0;

            if (leftWidth <= 1)
                leftWidth = 0;

            x -= (int)leftWidth;
            y -= (int)topWidth;
            w += (int)leftWidth + (int)rightWidth;
            h += (int)topWidth + (int)bottomWidth;

            Rectangle drawRect = new Rectangle(x, y, w, h);

            baseRenderer.PaintBackground(graphics, drawRect, ruleset);

        }

        // Private members

        private readonly IStyleSheetControlRenderer baseRenderer;

    }

}