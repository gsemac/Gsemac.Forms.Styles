using Gsemac.Forms.Styles.Extensions;
using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public class ListBoxControlRenderer :
        ControlRendererBase<ListBox> {

        // Public members

        public ListBoxControlRenderer(IStyleSheetControlRenderer baseRenderer) {

            this.baseRenderer = baseRenderer;

        }

        public override void RenderControl(Graphics graphics, ListBox control) {

            IRuleset ruleset = baseRenderer.GetRuleset(control);

            baseRenderer.PaintBackground(graphics, control);

            if (ruleset.Any(p => p.IsBorderRadiusProperty())) {

                graphics.SetClip(GraphicsExtensions.CreateRoundedRectangle(control.ClientRectangle,
                    (int)(ruleset.BorderTopLeftRadius?.Value ?? 0),
                    (int)(ruleset.BorderTopRightRadius?.Value ?? 0),
                    (int)(ruleset.BorderBottomLeftRadius?.Value ?? 0),
                    (int)(ruleset.BorderBottomRightRadius?.Value ?? 0)));

            }

            PaintItems(graphics, control);

        }

        // Private members

        private readonly IStyleSheetControlRenderer baseRenderer;

        private void PaintItems(Graphics graphics, ListBox control) {

            for (int i = 0; i < control.Items.Count; ++i)
                PaintItem(graphics, control, i);

        }
        private void PaintItem(Graphics graphics, ListBox control, int itemindex) {

            Rectangle clientRect = control.ClientRectangle;
            Rectangle itemRect = control.GetItemRectangle(itemindex);

            itemRect = new Rectangle(itemRect.X + 1, itemRect.Y + 1, itemRect.Width - 2, itemRect.Height);

            object item = control.Items[itemindex];

            if (itemRect.IntersectsWith(clientRect)) {

                IRuleset itemRuleset = GetItemRuleset(control, itemindex);

                baseRenderer.PaintBackground(graphics, itemRect, itemRuleset);

                Rectangle textRect = new Rectangle(itemRect.X - 1, itemRect.Y, itemRect.Width + 1, itemRect.Height);

                baseRenderer.PaintForeground(graphics, control.GetItemText(item), control.Font, textRect, itemRuleset);

            }

        }
        private INode GetItemNode(ListBox control, int itemindex) {

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
        private IRuleset GetItemRuleset(ListBox control, int itemindex) {

            return baseRenderer.GetRuleset(control, GetItemNode(control, itemindex));

        }

    }

}