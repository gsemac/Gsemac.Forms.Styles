using Gsemac.Forms.Styles.Extensions;
using Gsemac.Forms.Styles.StyleSheets;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public class ListBoxControlRenderer :
        ControlRendererBase<ListBox> {

        // Public members

        public override void PaintControl(ListBox control, ControlPaintArgs e) {

            e.PaintBackground();

            e.ClipToBorder();

            PaintItems(control, e);

            e.PaintBorder();

        }

        // Private members

        private void PaintItems(ListBox control, ControlPaintArgs e) {

            for (int i = 0; i < control.Items.Count; ++i)
                PaintItem(control, e, i);

        }
        private void PaintItem(ListBox control, ControlPaintArgs e, int itemindex) {

            Rectangle clientRect = control.ClientRectangle;
            Rectangle itemRect = control.GetItemRectangle(itemindex);

            itemRect = new Rectangle(itemRect.X + 1, itemRect.Y + 1, itemRect.Width - 2, itemRect.Height);

            object item = control.Items[itemindex];

            if (itemRect.IntersectsWith(clientRect)) {

                IRuleset itemRuleset = GetItemRuleset(control, e, itemindex);

                e.StyleRenderer.PaintBackground(e.Graphics, itemRect, itemRuleset);
                e.StyleRenderer.PaintBorder(e.Graphics, itemRect, itemRuleset);

                Rectangle textRect = new Rectangle(itemRect.X - 1, itemRect.Y, itemRect.Width + 1, itemRect.Height);

                e.StyleRenderer.PaintText(e.Graphics, textRect, itemRuleset, control.GetItemText(item), control.Font);

            }

        }
        private INode CreateItemNode(ListBox control, int itemindex) {

            object item = control.Items[itemindex];
            Rectangle itemRect = control.GetItemRectangle(itemindex);

            NodeStates states = NodeStates.None;

            if (control.SelectedItems.Contains(item))
                states |= NodeStates.Checked;

            if (RenderUtilities.MouseIntersectsWith(control, itemRect)) {

                if (Control.MouseButtons.HasFlag(MouseButtons.Left))
                    states |= NodeStates.Active;
                else
                    states |= NodeStates.Hover;

            }

            UserNode node = new UserNode(string.Empty, new[] { "ListBoxItem" });

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