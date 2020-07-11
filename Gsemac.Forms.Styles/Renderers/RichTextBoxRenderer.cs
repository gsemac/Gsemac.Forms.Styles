using Gsemac.Forms.Styles.StyleSheets;
using Gsemac.Forms.Utilities;
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

            ScrollBars visibleScrollbars = ControlUtilities.GetVisibleScrollBars(control);

            if (visibleScrollbars.HasFlag(ScrollBars.Vertical))
                borderRect = new Rectangle(borderRect.X, borderRect.Y, borderRect.Width + SystemInformation.VerticalScrollBarWidth, borderRect.Height);

            if (visibleScrollbars.HasFlag(ScrollBars.Horizontal))
                borderRect = new Rectangle(borderRect.X, borderRect.Y, borderRect.Width, borderRect.Height + SystemInformation.HorizontalScrollBarHeight);

            args.PaintBackground(borderRect);
            args.PaintBorder(borderRect);

        }

    }

}