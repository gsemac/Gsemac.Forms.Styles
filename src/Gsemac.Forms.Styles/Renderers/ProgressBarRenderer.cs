using Gsemac.Forms.Styles.Dom;
using Gsemac.Forms.Styles.StyleSheets;
using Gsemac.Forms.Styles.StyleSheets.Extensions;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public class ProgressBarRenderer :
        ControlRendererBase<ProgressBar> {

        // Public members

        public override void PaintControl(ProgressBar control, ControlPaintArgs args) {

            args.PaintBackground();
            args.PaintBorder();

            args.ClipToBorder();

            PaintProgress(control, args);

        }

        // Private members

        private IRuleset GetProgressRuleset(ProgressBar control, ControlPaintArgs args) {

            return args.StyleSheet.GetRuleset(GetProgressNode(control));

        }
        private INode2 GetProgressNode(ProgressBar control) {

            UserNode node = new UserNode(string.Empty, new[] { "Progress" });

            node.SetParent(new ControlNode(control));

            return node;

        }

        private void PaintProgress(ProgressBar control, ControlPaintArgs args) {

            double progress = (double)control.Value / control.Maximum;

            IRuleset progressRuleset = GetProgressRuleset(control, args);

            Rectangle clientRect = control.ClientRectangle;
            Rectangle progressRect = new Rectangle(clientRect.X, clientRect.Y, (int)Math.Floor(clientRect.Width * progress), clientRect.Height);

            args.StyleRenderer.PaintBackground(args.Graphics, progressRect, progressRuleset);
            args.StyleRenderer.PaintBorder(args.Graphics, progressRect, progressRuleset);

        }
    }

}