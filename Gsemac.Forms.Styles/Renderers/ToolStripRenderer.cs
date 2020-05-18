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

        public ToolStripRenderer(IStyleSheetControlRenderer baseRenderer) {

            this.baseRenderer = baseRenderer;

        }

        // Protected members

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e) {

            IRuleset ruleset = baseRenderer.GetRuleset(e.ToolStrip);
            Rectangle clientRect = e.ToolStrip.ClientRectangle;

            baseRenderer.PaintBackground(e.Graphics, clientRect, ruleset);

        }
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e) {

            Rectangle backgroundRect = new Rectangle(2, 0, e.Item.Width - 3, e.Item.Height);

            baseRenderer.PaintBackground(e.Graphics, backgroundRect, GetToolStripItemRuleset(e.ToolStrip, e.Item));

        }
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e) {

            IRuleset textRuleset = GetToolStripItemRuleset(e.ToolStrip, e.Item);

            baseRenderer.PaintForeground(e.Graphics, e.Text, e.TextFont, e.TextRectangle, textRuleset, e.TextFormat);

        }
        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e) {

            // This method is called when rendering the border around the ToolStripDropDown.

            baseRenderer.PaintBackground(e.Graphics, e.ToolStrip.ClientRectangle, GetToolStripDropDownRuleset(e.ToolStrip));

        }
        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e) {

            Rectangle separatorRect = e.Vertical ?
                new Rectangle(e.Item.Width / 2, 4, 1, e.Item.Height - 8) :
                new Rectangle(0, e.Item.Height / 2, e.Item.Width, 1);

            baseRenderer.PaintBackground(e.Graphics, separatorRect, GetToolStripSeparatorRuleset(e.ToolStrip));

        }

        // Private members

        private readonly IStyleSheetControlRenderer baseRenderer;

        INode GetToolStripItemNode(ToolStripItem item) {

            UserNode node = new UserNode(string.Empty, new[] { "ToolStripItem" });

            if (item.Selected)
                node.AddState(NodeStates.Hover);

            if (item is ToolStripMenuItem toolStripMenuItem && toolStripMenuItem.DropDown.Visible)
                node.AddState(NodeStates.Active);

            return node;

        }
        INode GetToolStripDropDownNode() {

            UserNode node = new UserNode(string.Empty, new[] { "ToolStripDropDown" });

            return node;

        }
        INode getToolStripSeparatorNode() {

            UserNode node = new UserNode(string.Empty, new[] { "ToolStripSeparator" });

            return node;

        }
        IRuleset GetToolStripItemRuleset(ToolStrip parent, ToolStripItem item) {

            return baseRenderer.GetRuleset(parent, GetToolStripItemNode(item));

        }
        IRuleset GetToolStripDropDownRuleset(ToolStrip parent) {

            return baseRenderer.GetRuleset(parent, GetToolStripDropDownNode());

        }
        IRuleset GetToolStripSeparatorRuleset(ToolStrip parent) {

            return baseRenderer.GetRuleset(parent, getToolStripSeparatorNode());

        }

    }

}