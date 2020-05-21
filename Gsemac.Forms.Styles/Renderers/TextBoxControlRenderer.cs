using Gsemac.Forms.Styles.Extensions;
using Gsemac.Forms.Styles.StyleSheets;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public class TextBoxControlRenderer :
        ControlRendererBase<TextBox> {

        // Public members

        public override void PaintControl(TextBox control, ControlPaintArgs e) {

            IRuleset ruleset = e.StyleSheet.GetRuleset(control);

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

            e.StyleRenderer.PaintBackground(e.Graphics, drawRect, ruleset);
            e.StyleRenderer.PaintBorder(e.Graphics, drawRect, ruleset);

            //graphics.Restore(graphicsState);

        }

    }

}