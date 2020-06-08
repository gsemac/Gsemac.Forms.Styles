using Gsemac.Forms.Styles.Extensions;
using Gsemac.Forms.Styles.Native;
using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public class ListViewRenderer :
        IListViewRenderer {

        // Public members

        public ListViewRenderer(IStyleSheet styleSheet, IStyleRenderer styleRenderer) {

            this.styleSheet = styleSheet;
            this.styleRenderer = styleRenderer;

        }

        public void Initialize(ListView listView) {

            INode listViewNode = new ControlNode(listView);
            IRuleset ruleset = styleSheet.GetRuleset(listViewNode);

            if (ruleset.BackgroundColor.HasValue())
                listView.BackColor = ruleset.BackgroundColor.Value;

        }

        public void DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e) {

            //DrawHeaderRightPortion(e.Header.ListView);

            IRuleset ruleset = GetColumnHeaderRuleset(e.Header);
            Rectangle rect = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Header.Width, e.Bounds.Height);
            TextFormatFlags textFormatFlags = RenderUtilities.GetTextFormatFlags(e.Header.TextAlign) | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis;

            styleRenderer.PaintBackground(e.Graphics, rect, ruleset);
            styleRenderer.PaintText(e.Graphics, rect, ruleset, e.Header.Text, e.Font, textFormatFlags);
            styleRenderer.PaintBorder(e.Graphics, rect, ruleset);

        }
        public void DrawItem(object sender, DrawListViewItemEventArgs e) {

            //DrawHeaderRightPortion(e.Item.ListView);

            UserNode node = new UserNode(e.Bounds, e.Item.ListView.PointToClient(Cursor.Position));

            node.SetClass("ListViewItem");
            node.SetParent(new ControlNode(e.Item.ListView));

            if (e.Item.Selected)
                node.AddState(NodeStates.Checked);

            IRuleset ruleset = styleSheet.GetRuleset(node);
            Rectangle rect = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);

            styleRenderer.PaintBackground(e.Graphics, rect, ruleset);

        }
        public void DrawSubItem(object sender, DrawListViewSubItemEventArgs e) {

            //DrawHeaderRightPortion(e.Item.ListView);

            UserNode node = new UserNode(e.Bounds, e.Item.ListView.PointToClient(Cursor.Position));

            node.SetClass("ListViewItem");
            node.SetParent(new ControlNode(e.Item.ListView));

            if (e.Item.Selected)
                node.AddState(NodeStates.Checked);

            IRuleset ruleset = styleSheet.GetRuleset(node);
            Rectangle rect = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            TextFormatFlags textFormatFlags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis;

            styleRenderer.PaintText(e.Graphics, rect, ruleset, e.SubItem.Text, e.Item.Font, textFormatFlags);
            styleRenderer.PaintBorder(e.Graphics, rect, ruleset);

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