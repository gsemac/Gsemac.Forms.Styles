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

    public class TextBoxControlRenderer :
        ControlRendererBase<TextBox> {

        // Public members

        public TextBoxControlRenderer(IStyleSheetControlRenderer baseRenderer) {

            this.baseRenderer = baseRenderer;

        }

        public override void RenderControl(Graphics graphics, TextBox control) {

            IRuleset ruleset = baseRenderer.GetRuleset(control);

            // Update the color of the TextBox itself.

            RenderUtilities.ApplyColorProperties(control, ruleset);

            // Draw the background the TextBox.
            // The height of regular TextBoxes is 23 pixels, with 3 pixels of horizontal padding.

            Rectangle clientRect = control.ClientRectangle;

            int x = clientRect.X - 3;
            int y = clientRect.Y - 4;
            int w = clientRect.Width + 6;
            int h = clientRect.Height + 7;

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

            if (control.ScrollBars.HasFlag(ScrollBars.Vertical))
                w += 17;

            //GraphicsState graphicsState = graphics.Save();

            Rectangle drawRect = new Rectangle(x, y, w, h);

            //graphics.SetClip(drawRect);

            baseRenderer.PaintBackground(graphics, drawRect, ruleset);

            //graphics.Restore(graphicsState);

        }

        // Private members

        private readonly IStyleSheetControlRenderer baseRenderer;

    }

}