using Gsemac.Forms.Styles.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators2.UserPaint
{

    internal sealed class ListBoxUserPaintStyleApplicator :
        WrapperControlUserPaintStyleApplicator<ListBox> {

        // Public members

        public ListBoxUserPaintStyleApplicator(bool customScrollBarsEnabled) :
            base(new WrapperControlOptions() {
                ForwardPaintEventsToChildControl = true,
                CustomScrollBarsEnabled = customScrollBarsEnabled,
            }) {
        }

        public override void InitializeStyle(ListBox listBox) {

            if (listBox is null)
                throw new ArgumentNullException(nameof(listBox));

            listBox.DrawMode = DrawMode.OwnerDrawFixed;

            AddEventHandlers(listBox);

            base.InitializeStyle(listBox);

        }
        public override void DeinitializeStyle(ListBox listBox) {

            if (listBox is null)
                throw new ArgumentNullException(nameof(listBox));

            RemoveEventHandlers(listBox);

            base.DeinitializeStyle(listBox);

        }

        // Protected members

        protected override WrapperControl WrapControl(ListBox listBox) {

            if (listBox is null)
                throw new ArgumentNullException(nameof(listBox));

            Size originalSize = listBox.Size;

            listBox.BorderStyle = BorderStyle.None;

            WrapperControl borderControl = base.WrapControl(listBox);

            borderControl.Padding = new Padding(2, 2, 2, 2);

            ControlUtilities2.Resize(borderControl, originalSize);

            return borderControl;

        }

        // Private members

        private bool isDragging = false;
        private int hotTrackedItemIndex = -1;

        private void MouseDownHandler(object sender, MouseEventArgs e) {

            if (e.Button == MouseButtons.Left)
                isDragging = true;

        }
        private void MouseUpHandler(object sender, MouseEventArgs e) {

            if (e.Button == MouseButtons.Left)
                isDragging = false;

        }
        private void MouseMoveHandler(object sender, MouseEventArgs e) {

            // ListBox controls in single selection mode can have their selection changed by dragging up and down.
            // However, the item is only highlighted, and the selected item isn't actually updated until mouse up.
            // By manually selecting the item here, the renderer can see it as selected and render it accordingly.

            if (sender is ListBox listBox && isDragging && (listBox.SelectionMode == SelectionMode.One || listBox.SelectionMode == SelectionMode.MultiExtended)) {

                int hoveredIndex = listBox.IndexFromPoint(e.Location);

                if (hoveredIndex != hotTrackedItemIndex) {

                    hotTrackedItemIndex = hoveredIndex;

                    if (hotTrackedItemIndex >= 0)
                        listBox.SelectedIndices.Add(hotTrackedItemIndex);

                    listBox.Invalidate();

                }

            }

        }

        private void AddEventHandlers(ListBox listBox) {

            if (listBox is null)
                throw new ArgumentNullException(nameof(listBox));

            listBox.MouseDown += InvalidateHandler;
            listBox.MouseDown += MouseDownHandler;
            listBox.MouseMove += MouseMoveHandler;
            listBox.MouseUp += MouseUpHandler;
            listBox.SelectedIndexChanged += InvalidateHandler;

        }
        private void RemoveEventHandlers(ListBox listBox) {

            if (listBox is null)
                throw new ArgumentNullException(nameof(listBox));

            listBox.MouseDown -= InvalidateHandler;
            listBox.MouseDown -= MouseDownHandler;
            listBox.MouseMove -= MouseMoveHandler;
            listBox.MouseUp -= MouseUpHandler;
            listBox.SelectedIndexChanged -= InvalidateHandler;

        }

        private static void InvalidateHandler(object sender, EventArgs e) {

            if (sender is Control control)
                control.Invalidate();

        }

    }

}