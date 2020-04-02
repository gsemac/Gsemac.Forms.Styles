using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Controls {

    public class TextBoxControlRenderer :
        ControlRendererBase {

        // Public members

        public TextBoxControlRenderer(IStyleSheet styleSheet) :
            base(styleSheet) {
        }

        public void RenderControl(Graphics graphics, TextBox control) {

            Rectangle clientRect = control.ClientRectangle;

            // The height of regular TextBoxes is 23 pixels, with 3 pixels of horizontal padding.

            int x = clientRect.X - 3;
            int y = clientRect.Y - 3;
            int w = clientRect.Width + 6;
            int h = clientRect.Height + 7;

            if (control.ScrollBars.HasFlag(ScrollBars.Vertical))
                w += 17;

            Rectangle drawRect = new Rectangle(x, y, w, h);

            PaintBackground(graphics, drawRect, GetRuleset(control));
            //PaintForeground(graphics, control);

        }

    }

}