using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers2 {

    internal sealed class ListBoxRenderer :
        StyleRendererBase<ListBox> {

        // Public members

        public override void Render(ListBox listBox, IRenderContext context) {

            if (listBox is null)
                throw new ArgumentNullException(nameof(listBox));

            if (context is null)
                throw new ArgumentNullException(nameof(context));

            context.Clear();

            context.DrawBackground();

            if (!context.IsRenderingBackground)
                DrawItems(listBox, context);

            if (context.IsRenderingBackground)
                context.DrawBorder();

        }

        // Private members

        private void DrawItems(ListBox listBox, IRenderContext context) {

            if (listBox is null)
                throw new ArgumentNullException(nameof(listBox));

            if (context is null)
                throw new ArgumentNullException(nameof(context));

            for (int itemIndex = 0; itemIndex < listBox.Items.Count; ++itemIndex)
                DrawItem(listBox, context, itemIndex);

        }
        private void DrawItem(ListBox listBox, IRenderContext context, int itemIndex) {

            if (listBox is null)
                throw new ArgumentNullException(nameof(listBox));

            if (context is null)
                throw new ArgumentNullException(nameof(context));

            if (itemIndex < 0 || itemIndex >= listBox.Items.Count)
                throw new ArgumentOutOfRangeException(nameof(itemIndex));

            Rectangle parentRect = listBox.ClientRectangle;
            Rectangle itemRect = listBox.GetItemRectangle(itemIndex);

            object item = listBox.Items[itemIndex];
            bool isSelected = listBox.SelectedIndices.Contains(itemIndex);

            // Only bother rendering items that are currently visible.

            if (itemRect.IntersectsWith(parentRect)) {

                if (isSelected) {

                    using (SolidBrush backgroundBrush = new SolidBrush(context.Style.AccentColor))
                        context.Graphics.FillRectangle(backgroundBrush, itemRect);

                }

                context.DrawText(itemRect, listBox.GetItemText(item), listBox.Font);

            }

        }

    }

}