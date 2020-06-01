using Gsemac.Forms.Styles.StyleSheets;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public class RichTextBoxRenderer :
        ControlRendererBase<RichTextBox> {

        public override void PaintControl(RichTextBox control, ControlPaintArgs args) {

            INode controlNode = new ControlNode(control);
            IRuleset ruleset = args.StyleSheet.GetRuleset(controlNode);

            RenderUtilities.ApplyColorProperties(control, ruleset);

            Rectangle clientRect = control.ClientRectangle;
            Rectangle borderRect = new Rectangle(clientRect.X - 3, clientRect.Y - 3, clientRect.Width + 6, clientRect.Height + 6);

            if (RenderUtilities.GetVisibleScrollbars(control).HasFlag(ScrollBars.Vertical))
                borderRect = new Rectangle(borderRect.X, borderRect.Y, borderRect.Width + SystemInformation.VerticalScrollBarWidth, borderRect.Height);

            args.PaintBackground(borderRect);
            args.PaintBorder(borderRect);

        }

    }

}