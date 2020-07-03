using Gsemac.Forms.Styles.Extensions;
using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public class ToolStripRenderer :
        System.Windows.Forms.ToolStripRenderer {

        // Public members

        public ToolStripRenderer(IStyleSheet styleSheet, IStyleRenderer styleRenderer) {

            this.styleSheet = styleSheet;
            this.styleRenderer = styleRenderer;

        }

        // Protected members

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e) {

            IRuleset ruleset = styleSheet.GetRuleset(e.ToolStrip);
            Rectangle clientRect = e.ToolStrip.ClientRectangle;

            styleRenderer.PaintBackground(e.Graphics, clientRect, ruleset);
            styleRenderer.PaintBorder(e.Graphics, clientRect, ruleset);

        }
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e) {

            IRuleset ruleset = GetToolStripItemRuleset(e.ToolStrip, e.Item);
            Rectangle backgroundRect = new Rectangle(2, 0, e.Item.Width - 3, e.Item.Height);

            styleRenderer.PaintBackground(e.Graphics, backgroundRect, ruleset);
            styleRenderer.PaintBorder(e.Graphics, backgroundRect, ruleset);

        }
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e) {

            IRuleset textRuleset = GetToolStripItemRuleset(e.ToolStrip, e.Item);

            styleRenderer.PaintText(e.Graphics, e.TextRectangle, textRuleset, e.Text, e.TextFont, e.TextFormat);

        }
        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e) {

            // This method is called when rendering the border around the ToolStripDropDown.

            IRuleset ruleset = GetToolStripDropDownRuleset(e.ToolStrip);

            styleRenderer.PaintBackground(e.Graphics, e.ToolStrip.ClientRectangle, ruleset);
            styleRenderer.PaintBorder(e.Graphics, e.ToolStrip.ClientRectangle, ruleset);

        }
        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e) {

            IRuleset ruleset = GetToolStripSeparatorRuleset(e.ToolStrip);

            Rectangle separatorRect = e.Vertical ?
                new Rectangle(e.Item.Width / 2, 4, 1, e.Item.Height - 8) :
                new Rectangle(0, e.Item.Height / 2, e.Item.Width, 1);

            styleRenderer.PaintBackground(e.Graphics, separatorRect, ruleset);
            styleRenderer.PaintBorder(e.Graphics, separatorRect, ruleset);

        }

        // Private members

        private readonly IStyleSheet styleSheet;
        private readonly IStyleRenderer styleRenderer;

        private UserNode GetToolStripItemNode(ToolStripItem item) {

            UserNode node = new UserNode(string.Empty, new[] { "ToolStripItem", "Item" });

            if (item.Selected)
                node.AddState(NodeStates.Hover);

            if (item is ToolStripMenuItem toolStripMenuItem && toolStripMenuItem.DropDown.Visible)
                node.AddState(NodeStates.Active);

            return node;

        }
        private UserNode GetToolStripDropDownNode() {

            UserNode node = new UserNode(string.Empty, new[] { "ToolStripDropDown" });

            return node;

        }
        private UserNode getToolStripSeparatorNode() {

            UserNode node = new UserNode(string.Empty, new[] { "ToolStripSeparator", "ToolStripItem", "Item" });

            return node;

        }
        private IRuleset GetToolStripItemRuleset(ToolStrip parent, ToolStripItem item) {

            UserNode node = GetToolStripItemNode(item);

            node.SetParent(new ControlNode(parent));

            return styleSheet.GetRuleset(node);

        }
        private IRuleset GetToolStripDropDownRuleset(ToolStrip parent) {

            UserNode node = GetToolStripDropDownNode();

            node.SetParent(new ControlNode(parent));

            return styleSheet.GetRuleset(node);

        }
        private IRuleset GetToolStripSeparatorRuleset(ToolStrip parent) {

            UserNode node = getToolStripSeparatorNode();

            node.SetParent(new ControlNode(parent));

            return styleSheet.GetRuleset(node);

        }

    }

}