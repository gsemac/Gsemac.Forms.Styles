using Gsemac.Forms.Styles.Renderers2;
using Gsemac.Forms.Styles.StyleSheets;
using Gsemac.Forms.Styles.StyleSheets.Extensions;
using Gsemac.Win32;
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

        public void DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e) {

            IRuleset ruleset = GetColumnHeaderRuleset(e.Header);

            const int textPadding = 4;

            Rectangle headerRect = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Header.Width, e.Bounds.Height);
            Rectangle textRect = new Rectangle(headerRect.X + textPadding, headerRect.Y, headerRect.Width - textPadding * 2, headerRect.Height);

            TextFormatFlags textFormatFlags = ControlUtilities.GetTextFormatFlags(e.Header.TextAlign) | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix;

            styleRenderer.PaintBackground(e.Graphics, headerRect, ruleset);
            styleRenderer.PaintText(e.Graphics, textRect, ruleset, e.Header.Text, e.Font, textFormatFlags);
            styleRenderer.PaintBorder(e.Graphics, headerRect, ruleset);

            PaintHeaderControlRightPortion(e.Header.ListView);

        }
        public void DrawItem(object sender, DrawListViewItemEventArgs e) {

            /* There is a bug in the underlying Win32 ListView control that causes sub-items to be drawn over on the first mouse-over.
             * There is a workaround for the issue implemented here:
             * https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.listview.ownerdraw?view=netcore-3.1
             * 
             * The problem with this workaround is that it doesn't work with ListViews in virtual mode, where the tag is lost when a new item is created.
             * The following approach redraws all sub items when an item is invalidated.
             */

            PaintItem(e.Graphics, e.Item);

            // Paint all sub-items.

            foreach (ListViewItem.ListViewSubItem subItem in e.Item.SubItems)
                PaintSubItem(e.Graphics, e.Item, subItem);

            PaintHeaderControlRightPortion(e.Item.ListView);

        }
        public void DrawSubItem(object sender, DrawListViewSubItemEventArgs e) {
        }

        public override void PaintControl(ListView control, ControlPaintArgs args) {

            // Draw the background/border of the control.

            IRuleset ruleset = args.StyleSheet.GetRuleset(control);
            Rectangle borderRect = Renderers2.RenderUtilities.GetOuterBorderRectangle(control.ClientRectangle, ruleset);

            if (ruleset.BackgroundColor.HasValue() && ruleset.BackgroundColor.Value != control.BackColor)
                control.BackColor = ruleset.BackgroundColor.Value;

            args.PaintBackground(borderRect);
            args.PaintBorder(borderRect);

        }

        // Private members

        private readonly IStyleSheet styleSheet;
        private readonly IStyleRenderer styleRenderer;

        private UserNode GetColumnHeaderNode(ColumnHeader columnHeader) {

            Rectangle bounds = GetColumnHeaderBounds(columnHeader);

            UserNode node = new UserNode(bounds, columnHeader.ListView.PointToClient(Cursor.Position));

            node.SetParent(new ControlNode(columnHeader.ListView));

            node.AddClass("ColumnHeader");
            node.AddClass("Header");

            if (columnHeader.Index % 2 == 0)
                node.AddClass("Even");
            else
                node.AddClass("Odd");

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

        private void PaintItem(Graphics graphics, ListViewItem item) {

            Rectangle bounds = item.Bounds;
            UserNode node = new UserNode(bounds, item.ListView.PointToClient(Cursor.Position));

            node.AddClass("ListViewItem");
            node.AddClass("Item");
            node.SetParent(new ControlNode(item.ListView));

            if (item.Index % 2 == 0)
                node.AddClass("Even");
            else
                node.AddClass("Odd");

            if (item.Selected)
                node.AddState(NodeStates.Checked);

            IRuleset ruleset = styleSheet.GetRuleset(node);
            Rectangle rect = new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height);

            styleRenderer.PaintBackground(graphics, rect, ruleset);
            styleRenderer.PaintBorder(graphics, rect, ruleset);

        }
        private void PaintSubItem(Graphics graphics, ListViewItem item, ListViewItem.ListViewSubItem subItem) {

            ListView listView = item.ListView;
            ColumnHeader columnHeader = listView.Columns[item.SubItems.IndexOf(subItem)];

            Rectangle bounds = subItem.Bounds;
            UserNode node = new UserNode(bounds, listView.PointToClient(Cursor.Position));

            node.SetClass("ListViewItem");
            node.AddClass("Item");
            node.SetParent(new ControlNode(listView));

            if (item.Selected)
                node.AddState(NodeStates.Checked);

            int textPadding = 4;

            IRuleset ruleset = styleSheet.GetRuleset(node);

            Rectangle itemRect = new Rectangle(bounds.X, bounds.Y, columnHeader.Width, bounds.Height);
            Rectangle textRect = new Rectangle(itemRect.X + textPadding, itemRect.Y, itemRect.Width - textPadding * 2, itemRect.Height);

            TextFormatFlags textFormatFlags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix;

            styleRenderer.PaintText(graphics, textRect, ruleset, subItem.Text, subItem.Font, textFormatFlags);
            styleRenderer.PaintBorder(graphics, itemRect, ruleset);

        }

        private IntPtr GetHeaderControl(ListView listView) {

            const int LVM_GETHEADER = 0x1000 + 31;

            return User32.SendMessage(listView.Handle, LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero);

        }
        private void PaintHeaderControlRightPortion(ListView listView) {

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