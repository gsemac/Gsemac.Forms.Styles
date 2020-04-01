using Gsemac.Forms.Styles.Extensions;
using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Controls {

    public class ListBoxControlRenderer :
        ControlRendererBase {

        // Public members

        public ListBoxControlRenderer(IStyleSheet styleSheet) :
            base(styleSheet) {
        }

        public void RenderControl(Graphics graphics, ListBox control) {

            IRuleset ruleset = GetRuleset(control);

            NumericProperty borderRadius = ruleset.GetProperty(PropertyType.BorderRadius) as NumericProperty;

            PaintBackground(graphics, control);

            if (borderRadius?.Value > 0.0)
                graphics.SetClip(GraphicsExtensions.CreateRoundedRectangle(control.ClientRectangle, (int)borderRadius.Value));

            PaintItems(graphics, control);

        }

        // Private members

        private void PaintItems(Graphics graphics, ListBox control) {

            for (int i = 0; i < control.Items.Count; ++i)
                PaintItem(graphics, control, i);

        }
        private void PaintItem(Graphics graphics, ListBox control, int itemindex) {

            Rectangle clientRect = control.ClientRectangle;
            Rectangle itemRect = control.GetItemRectangle(itemindex);

            object item = control.Items[itemindex];

            if (itemRect.IntersectsWith(clientRect)) {

                IRuleset itemRuleset = GetRuleset(control, GetItemNode(control, itemindex));

                PaintBackground(graphics, itemRect, itemRuleset);

                PaintForeground(graphics, control.GetItemText(item), control.Font, itemRect, itemRuleset);

            }

        }
        private INode GetItemNode(ListBox control, int itemindex) {

            object item = control.Items[itemindex];
            Rectangle itemRect = control.GetItemRectangle(itemindex);

            NodeStates states = NodeStates.None;

            if (control.SelectedItems.Contains(item))
                states |= NodeStates.Checked;

            if (MouseIntersectsWith(control, itemRect)) {

                if (Control.MouseButtons.HasFlag(MouseButtons.Left))
                    states |= NodeStates.Active;
                else
                    states |= NodeStates.Hover;

            }

            return new Node("ListBoxItem", states: states);

        }

    }

}