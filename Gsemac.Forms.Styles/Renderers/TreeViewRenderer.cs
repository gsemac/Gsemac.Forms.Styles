using Gsemac.Forms.Styles.Extensions;
using Gsemac.Forms.Styles.StyleSheets;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public class TreeViewRenderer :
        ControlRendererBase<TreeView> {

        // Public members

        public override void InitializeControl(TreeView control) {

            control.BorderStyle = System.Windows.Forms.BorderStyle.None;

        }

        public override void PaintControl(TreeView control, ControlPaintArgs args) {

            PaintBackground(control, args);

            if(!args.ParentDraw) {

                foreach (TreeNode node in control.Nodes)
                    PaintNodeAndChildNodes(control, node, args);

            }

        }

        // Protected members

        protected virtual void PaintBackground(TreeView control, ControlPaintArgs args) {

            IRuleset ruleset = args.StyleSheet.GetRuleset(control);
            Rectangle borderRect = RenderUtilities.GetOuterBorderRectangle(control, ruleset);

            if (ruleset.BackgroundColor.HasValue() && ruleset.BackgroundColor.Value != control.BackColor)
                control.BackColor = ruleset.BackgroundColor.Value;

            args.PaintBackground(borderRect);
            args.PaintBorder(borderRect);

        }
        protected virtual void PaintNode(TreeView control, TreeNode node, ControlPaintArgs args) {

            args.StyleRenderer.PaintText(args.Graphics, node.Bounds, args.StyleSheet.GetRuleset(control), node.Text, control.Font, TextFormatFlags.Default);

        }

        // Private members

        private void PaintNodeAndChildNodes(TreeView control, TreeNode node, ControlPaintArgs args) {

            PaintNode(control, node, args);

            foreach (TreeNode childNode in node.Nodes)
                PaintNodeAndChildNodes(control, childNode, args);

        }

    }

}