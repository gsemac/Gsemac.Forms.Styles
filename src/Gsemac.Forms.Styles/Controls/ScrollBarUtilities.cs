using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Controls {

    internal static class ScrollBarUtilities {

        // Public members

        public static void BindToControl(Control control, WrapperControlScrollBar scrollBar) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            if (scrollBar is null)
                throw new ArgumentNullException(nameof(scrollBar));

            Rectangle viewportRectangle = GetViewportRectangle(control);
            Rectangle contentRectangle = GetContentRectangle(control);

            // Initialize the scroll parameters.

            int viewportLength = scrollBar.Orientation == Orientation.Vertical ?
                viewportRectangle.Height :
                viewportRectangle.Width;

            int contentLength = scrollBar.Orientation == Orientation.Vertical ?
                contentRectangle.Height :
                contentRectangle.Width;

            scrollBar.Minimum = 0;
            scrollBar.SmallChange = viewportLength / 20;
            scrollBar.LargeChange = viewportLength / 10;

            scrollBar.Maximum = contentLength - viewportLength;

            // Set the scrollbar position.

            if (scrollBar.Orientation == Orientation.Vertical) {

                scrollBar.Height = control.Height;
                scrollBar.Width = SystemInformation.VerticalScrollBarWidth;
                scrollBar.Location = new Point(control.Width - scrollBar.Width, 0);
                scrollBar.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;

            }
            else {

                scrollBar.Height = SystemInformation.HorizontalScrollBarHeight;
                scrollBar.Width = control.Width;
                scrollBar.Location = new Point(0, control.Height - scrollBar.Height);
                scrollBar.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left;

            }

            // Initialize the scrollbar appearance.

            RefreshScrollBar(control, scrollBar);

            // Add event handlers.

            scrollBar.ValueChanged += (sender, e) => ControlUtilities.SetScrollPosition(control, scrollBar.Value);

            void controlPaintHandler(object sender, EventArgs e) => RefreshScrollBar(control, scrollBar);

            control.Disposed += (sender, e) => scrollBar.Dispose();
            control.Paint += controlPaintHandler;

            // Make sure to remove event handlers when the scrollbar is disposed to prevent them from accumulating.

            scrollBar.Disposed += (sender, e) => control.Paint -= controlPaintHandler;

        }

        // Private members

        private const int DefaultListBoxItemHeight = 15;

        private static int GetListBoxItemHeight(ListBox listBox) {

            if (listBox is null)
                throw new ArgumentNullException(nameof(listBox));

            return listBox.Items.Count > 0 ?
                listBox.GetItemHeight(0) :
                DefaultListBoxItemHeight;

        }

        private static Rectangle GetViewportRectangle(Control control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            if (control is ListBox listBox) {

                // ListBox controls scroll by the item rather than the display rectangle.

                int visibleItemCount = (int)(control.ClientRectangle.Height / (double)GetListBoxItemHeight(listBox));

                return new Rectangle(control.ClientRectangle.X, control.ClientRectangle.Y, control.ClientRectangle.Width, visibleItemCount);

            }
            else {

                return control.ClientRectangle;

            }

        }
        private static Rectangle GetContentRectangle(Control control) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            if (control is ListBox listBox) {

                // ListBox controls scroll by the item rather than the display rectangle.

                return new Rectangle(control.ClientRectangle.X, control.ClientRectangle.Y, control.ClientRectangle.Width, listBox.Items.Count);

            }
            else {

                return control.DisplayRectangle;

            }

        }

        private static void RefreshScrollBar(Control control, WrapperControlScrollBar scrollBar) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            if (scrollBar is null)
                throw new ArgumentNullException(nameof(scrollBar));

            ScrollBars scrollBarOrientation = scrollBar.Orientation == Orientation.Vertical ?
                ScrollBars.Vertical :
                ScrollBars.Horizontal;

            scrollBar.Value = ControlUtilities.GetScrollPosition(control);
            scrollBar.Visible = ControlUtilities2.GetVisibleScrollBars(control).HasFlag(scrollBarOrientation);

        }

    }

}