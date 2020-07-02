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

            if (!args.ParentDraw) {

                PaintLines(control, control.Nodes, args);

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
        protected virtual void PaintNodeContent(TreeView control, TreeNode node, ControlPaintArgs args) {

            UserNode treeNodeNode = new UserNode(string.Empty, new[] { "Node", "TreeViewNode" });

            treeNodeNode.SetParent(new ControlNode(control));

            if (node.IsSelected)
                treeNodeNode.AddState(NodeStates.Checked);

            IRuleset treeNodeRuleset = args.StyleSheet.GetRuleset(treeNodeNode);

            Rectangle nodeRect = node.Bounds;
            Rectangle drawRect = new Rectangle(nodeRect.X + 2, nodeRect.Y + 1, nodeRect.Width, nodeRect.Height - 1);
            Rectangle textRect = new Rectangle(nodeRect.X + 1, nodeRect.Y + 3, nodeRect.Width - 1, nodeRect.Height - 3);

            args.StyleRenderer.PaintBackground(args.Graphics, drawRect, treeNodeRuleset);
            args.StyleRenderer.PaintText(args.Graphics, textRect, treeNodeRuleset, node.Text, control.Font, TextFormatFlags.Default);
            args.StyleRenderer.PaintBorder(args.Graphics, drawRect, treeNodeRuleset);

        }
        protected virtual void PaintNodeButton(TreeView control, TreeNode node, ControlPaintArgs args) {

            const int buttonWidth = 9;
            const int detailPadding = 2;

            UserNode nodeButtonNode = new UserNode(string.Empty, new[] { "Button" });

            nodeButtonNode.SetParent(new ControlNode(control));

            if (node.IsSelected)
                nodeButtonNode.AddState(NodeStates.Checked);

            IRuleset nodeButtonRuleset = args.StyleSheet.GetRuleset(nodeButtonNode);

            Rectangle nodeRect = node.Bounds;
            Rectangle buttonRect = new Rectangle(nodeRect.X - buttonWidth - 3, nodeRect.Y + nodeRect.Height - buttonWidth - 1, buttonWidth, buttonWidth);

            args.StyleRenderer.PaintBackground(args.Graphics, buttonRect, nodeButtonRuleset);
            args.StyleRenderer.PaintBorder(args.Graphics, buttonRect, nodeButtonRuleset);

            Color detailColor = nodeButtonRuleset.Color?.Value ?? Color.Black;

            buttonRect.Inflate(-detailPadding, -detailPadding);

            int x = buttonRect.X;
            int y = buttonRect.Y;
            int x2 = x + buttonRect.Width - 1;
            int y2 = y + buttonRect.Height - 1;
            int midX = x + (buttonRect.Width / 2);
            int midY = y + (buttonRect.Height / 2);

            using (Pen detailPen = new Pen(detailColor)) {

                args.Graphics.DrawLine(detailPen, new Point(x, midY), new Point(x2, midY));

                if (!node.IsExpanded)
                    args.Graphics.DrawLine(detailPen, new Point(midX, y), new Point(midX, y2));

            }

        }

        // Private members

        private void PaintNode(TreeView control, TreeNode node, ControlPaintArgs args) {

            // Check that the node bounds size is valid to avoid drawing in the top-left corner.
            // https://stackoverflow.com/questions/50815725/treeview-owner-draw-anomaly

            if (node.Bounds.Width > 1 && node.Bounds.Height > 1) {

                // Paint the node content.

                PaintNodeContent(control, node, args);

                // Paint the expand/collapse button to the left of the node.

                if (node.Nodes.Count > 0)
                    PaintNodeButton(control, node, args);

            }

        }
        private void PaintNodeAndChildNodes(TreeView control, TreeNode node, ControlPaintArgs args) {

            PaintNode(control, node, args);

            foreach (TreeNode childNode in node.Nodes)
                PaintNodeAndChildNodes(control, childNode, args);

        }
        private void PaintLines(TreeView control, TreeNodeCollection nodes, ControlPaintArgs args) {

            const int buttonWidth = 9;

            if (control.ShowLines && control.Nodes.Count > 0) {

                if (control.ShowRootLines || nodes[0].Level != 0) {

                    UserNode lineNode = new UserNode(string.Empty, new[] { "Lines" });

                    lineNode.SetParent(new ControlNode(control));

                    IRuleset lineRuleset = args.StyleSheet.GetRuleset(lineNode);

                    Color lineColor = lineRuleset.Color?.Value ?? Color.Black;

                    using (Pen linePen = new Pen(lineColor)) {

                        linePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

                        // Draw a line from the first node down to the last node.

                        TreeNode firstNode = nodes[0];
                        TreeNode lastNode = nodes[nodes.Count - 1];

                        int x1 = firstNode.Bounds.X - 8;
                        int x2 = x1;
                        int y1 = firstNode.Bounds.Y + (firstNode.Nodes.Count > 0 ? buttonWidth - 1 : 2);
                        int y2 = lastNode.Bounds.Y + (firstNode.Nodes.Count > 0 ? buttonWidth - 1 : 13);

                        args.Graphics.DrawLine(linePen, new Point(x1, y1), new Point(x2, y2));

                        // Draw horizontal lines connecting the vertical line to each of the nodes.

                        foreach (TreeNode childNode in nodes) {

                            x1 = firstNode.Bounds.X - buttonWidth + 1;
                            x2 = childNode.Bounds.X + 2;
                            y1 = childNode.Bounds.Y + 12;
                            y2 = y1;

                            args.Graphics.DrawLine(linePen, new Point(x1, y1), new Point(x2, y2));

                        }

                    }

                }

                // Draw lines for the child nodes.

                foreach (TreeNode node in nodes) {

                    if (node.IsExpanded)
                        PaintLines(control, node.Nodes, args);

                }

            }

        }

    }

}