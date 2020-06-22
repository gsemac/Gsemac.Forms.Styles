using Gsemac.Forms.Styles.Extensions;
using Gsemac.Forms.Styles.StyleSheets;
using Gsemac.Forms.Utilities;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public class ListBoxRenderer :
        ControlRendererBase<ListBox> {

        // Public members

        public override void PaintControl(ListBox control, ControlPaintArgs e) {

            Rectangle borderRect = RenderUtilities.GetOuterBorderRectangle(control, e.StyleSheet.GetRuleset(control));

            if (ControlUtilities.GetVisibleScrollbars(control).HasFlag(ScrollBars.Vertical))
                borderRect = new Rectangle(borderRect.X, borderRect.Y, borderRect.Width + SystemInformation.VerticalScrollBarWidth, borderRect.Height);

            e.PaintBackground();

            if (!e.ParentDraw) {

                GraphicsState graphicsState = e.Graphics.Save();

                e.ClipToBorder(borderRect);

                PaintItems(control, e);

                e.Graphics.Restore(graphicsState);

            }

            e.PaintBorder(borderRect);

        }

        // Private members

        private void PaintItems(ListBox control, ControlPaintArgs e) {

            for (int i = 0; i < control.Items.Count; ++i)
                PaintItem(control, e, i);

        }
        private void PaintItem(ListBox control, ControlPaintArgs e, int itemindex) {

            Rectangle clientRect = control.ClientRectangle;
            Rectangle itemRect = control.GetItemRectangle(itemindex);

            object item = control.Items[itemindex];

            if (itemRect.IntersectsWith(clientRect)) {

                IRuleset itemRuleset = GetItemRuleset(control, e, itemindex);

                e.StyleRenderer.PaintBackground(e.Graphics, itemRect, itemRuleset);
                e.StyleRenderer.PaintBorder(e.Graphics, itemRect, itemRuleset);
                e.StyleRenderer.PaintText(e.Graphics, itemRect, itemRuleset, control.GetItemText(item), control.Font);

            }

        }
        private INode CreateItemNode(ListBox control, int itemindex) {

            object item = control.Items[itemindex];
            Rectangle itemRect = control.GetItemRectangle(itemindex);

            NodeStates states = NodeStates.None;

            if (control.SelectedItems.Contains(item))
                states |= NodeStates.Checked;

            if (ControlUtilities.MouseIntersectsWith(control, itemRect)) {

                if (Control.MouseButtons.HasFlag(MouseButtons.Left))
                    states |= NodeStates.Active;
                else
                    states |= NodeStates.Hover;

            }

            UserNode node = new UserNode(string.Empty, new[] { "ListBoxItem", "Item" });

            node.SetParent(new ControlNode(control));
            node.SetStates(states);

            if (itemindex % 2 == 0)
                node.AddClass("even");
            else
                node.AddClass("odd");

            return node;

        }
        private IRuleset GetItemRuleset(ListBox control, ControlPaintArgs e, int itemindex) {

            return e.StyleSheet.GetRuleset(CreateItemNode(control, itemindex), control);

        }

    }

}