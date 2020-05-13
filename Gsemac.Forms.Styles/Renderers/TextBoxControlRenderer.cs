using Gsemac.Forms.Styles.Extensions;
using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Collections.Generic;
using System.Drawing;
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

            StyleUtilities.ApplyColorProperties(control, ruleset);

            // Draw the background the TextBox.
            // The height of regular TextBoxes is 23 pixels, with 3 pixels of horizontal padding.

            Rectangle clientRect = control.ClientRectangle;

            int x = clientRect.X - 3;
            int y = clientRect.Y - 3;
            int w = clientRect.Width + 6;
            int h = clientRect.Height + 7;

            if (control.ScrollBars.HasFlag(ScrollBars.Vertical))
                w += 17;

            Rectangle drawRect = new Rectangle(x, y, w, h);

            baseRenderer.PaintBackground(graphics, drawRect, ruleset);

        }

        // Private members

        private readonly IStyleSheetControlRenderer baseRenderer;

    }

}