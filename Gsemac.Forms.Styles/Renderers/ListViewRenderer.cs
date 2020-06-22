using Gsemac.Forms.Styles.Extensions;
using Gsemac.Forms.Styles.StyleSheets;
using Gsemac.Forms.Utilities;
using Gsemac.Native;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public class ListViewRenderer :
        ControlRendererBase<ListView>,
        IListViewRenderer {

        // Public members

        public ListViewRenderer(IStyleSheet styleSheet, IStyleRenderer styleRenderer) {

            this.styleSheet = styleSheet;
            this.styleRenderer = styleRenderer;

        }

        public override void InitializeControl(ListView listView) {

            INode listViewNode = new ControlNode(listView);
            IRuleset ruleset = styleSheet.GetRuleset(listViewNode);

            if (ruleset.BackgroundColor.HasValue())
                listView.BackColor = ruleset.BackgroundColor.Value;

        }

        public void DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e) {

            IRuleset ruleset = GetColumnHeaderRuleset(e.Header);

            int textPadding = 4;

            Rectangle headerRect = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Header.Width, e.Bounds.Height);
            Rectangle textRect = new Rectangle(headerRect.X + textPadding, headerRect.Y, headerRect.Width - textPadding * 2, headerRect.Height);

            TextFormatFlags textFormatFlags = ControlUtilities.GetTextFormatFlags(e.Header.TextAlign) | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis;

            styleRenderer.PaintBackground(e.Graphics, headerRect, ruleset);
            styleRenderer.PaintText(e.Graphics, textRect, ruleset, e.Header.Text, e.Font, textFormatFlags);
            styleRenderer.PaintBorder(e.Graphics, headerRect, ruleset);

            DrawHeaderRightPortion(e.Header.ListView);

        }
        public void DrawItem(object sender, DrawListViewItemEventArgs e) {

            UserNode node = new UserNode(e.Bounds, e.Item.ListView.PointToClient(Cursor.Position));

            node.AddClass("ListViewItem");
            node.AddClass("Item");
            node.SetParent(new ControlNode(e.Item.ListView));

            if (e.Item.Selected)
                node.AddState(NodeStates.Checked);

            IRuleset ruleset = styleSheet.GetRuleset(node);
            Rectangle rect = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);

            styleRenderer.PaintBackground(e.Graphics, rect, ruleset);
            styleRenderer.PaintBorder(e.Graphics, rect, ruleset);

            DrawHeaderRightPortion(e.Item.ListView);

        }
        public void DrawSubItem(object sender, DrawListViewSubItemEventArgs e) {

            UserNode node = new UserNode(e.Bounds, e.Item.ListView.PointToClient(Cursor.Position));

            node.SetClass("ListViewItem");
            node.AddClass("Item");
            node.SetParent(new ControlNode(e.Item.ListView));

            if (e.Item.Selected)
                node.AddState(NodeStates.Checked);

            int textPadding = 4;

            IRuleset ruleset = styleSheet.GetRuleset(node);

            Rectangle itemRect = new Rectangle(e.SubItem.Bounds.X, e.SubItem.Bounds.Y, e.Header.Width, e.SubItem.Bounds.Height);
            Rectangle textRect = new Rectangle(itemRect.X + textPadding, itemRect.Y, itemRect.Width - textPadding * 2, itemRect.Height);

            TextFormatFlags textFormatFlags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis;

            styleRenderer.PaintText(e.Graphics, textRect, ruleset, e.SubItem.Text, e.Item.Font, textFormatFlags);
            styleRenderer.PaintBorder(e.Graphics, itemRect, ruleset);

            DrawHeaderRightPortion(e.Item.ListView);

        }

        public override void PaintControl(ListView control, ControlPaintArgs args) {

            // Draw the background/border of the control.

            IRuleset ruleset = args.StyleSheet.GetRuleset(control);
            Rectangle borderRect = RenderUtilities.GetOuterBorderRectangle(control, ruleset);

            args.PaintBackground(borderRect);
            args.PaintBorder(borderRect);

        }

        // Private members

        private readonly IStyleSheet styleSheet;
        private readonly IStyleRenderer styleRenderer;

        private UserNode GetColumnHeaderNode(ColumnHeader columnHeader) {

            Rectangle bounds = GetColumnHeaderBounds(columnHeader);

            UserNode node = new UserNode(bounds, columnHeader.ListView.PointToClient(Cursor.Position));

            node.SetClass("ColumnHeader");
            node.SetParent(new ControlNode(columnHeader.ListView));

            return node;

        }

        private IRuleset GetColumnHeaderRuleset(ColumnHeader columnHeader) {

            return styleSheet.GetRuleset(GetColumnHeaderNode(columnHeader));

        }

        private Rectangle GetColumnHeaderBounds(ColumnHeader columnHeader) {

            int columnHeaderIndex = columnHeader.DisplayIndex;

            int columnX = columnHeader.ListView.ClientRectangle.X;
            int columnY = columnHeader.ListView.ClientRectangle.Y;
            int columnWidth = columnHeader.Width;
            int columnHeight = columnHeader.ListView.TopItem?.Bounds.Top ?? 0;

            for (int i = 0; i < columnHeaderIndex && i < columnHeader.ListView.Columns.Count; ++i)
                columnX += columnHeader.ListView.Columns[i].Width;

            return new Rectangle(columnX, columnY, columnWidth, columnHeight);

        }

        private IntPtr GetHeaderControl(ListView listView) {

            const int LVM_GETHEADER = 0x1000 + 31;

            return User32.SendMessage(listView.Handle, LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero);

        }
        private void DrawHeaderRightPortion(ListView listView) {

            bool hasColumns = listView.View == View.Details &&
                listView.HeaderStyle != ColumnHeaderStyle.None &&
                listView.Columns.Count > 0;

            if (hasColumns) {

                int columnsWidth = listView.Columns.Cast<ColumnHeader>().Sum(header => header.Width);
                int x = columnsWidth;
                int width = listView.Width - columnsWidth;

                Rectangle rect = new Rectangle(x, 0, width, listView.TopItem?.Bounds.Top ?? 0);

                IntPtr headerControl = GetHeaderControl(listView);
                IntPtr hdc = User32.GetDC(headerControl);

                IRuleset ruleset = GetColumnHeaderRuleset(listView.Columns[0]);

                using (Graphics graphics = Graphics.FromHdc(hdc))
                    styleRenderer.PaintBackground(graphics, rect, ruleset);

                User32.ReleaseDC(headerControl, hdc);

            }

        }

    }

}