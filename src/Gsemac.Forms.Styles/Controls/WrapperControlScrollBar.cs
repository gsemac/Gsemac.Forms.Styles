using Gsemac.Drawing;
using Gsemac.Forms.Styles.Dom;
using Gsemac.Forms.Styles.Renderers.Extensions;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Controls {

    [DomHidden]
    internal class WrapperControlScrollBar :
        UserControl {

        // Public members

        public event EventHandler ValueChanged;

        public override bool AutoSize {
            get => base.AutoSize;
            set => base.AutoSize = value;
        }
        public Orientation Orientation { get; set; } = Orientation.Vertical;
        public int Minimum { get; set; } = 0;
        public int Maximum { get; set; } = 100;
        public int SmallChange {
            get => smallChange;
            set {

                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));

                smallChange = value;

            }
        }
        public int LargeChange {
            get => largeChange;
            set {

                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));

                largeChange = value;

            }
        }
        public int Value {
            get => value;
            set {

                // Updating the value by property will only work if we're not currently dragging.
                // This is so controls bound to the scroll bar that update this value when scrolled don't update it while the user is scolling.

                if (isDragging)
                    return;

                SetValue(value);

            }
        }

        public WrapperControlScrollBar() {

            // Enable user paint so that the control can be custom painted.

            DoubleBuffered = true;

        }

        // Protected members

        protected override void OnPaint(PaintEventArgs e) {

            base.OnPaint(e);

            using (SolidBrush brush = new SolidBrush(ForeColor)) {

                // Paint the scroll arrows.

                PaintScrollArrow(e.Graphics, GetUpperScrollArrowBounds(), Orientation == Orientation.Vertical ? ArrowDirection.Up : ArrowDirection.Left);
                PaintScrollArrow(e.Graphics, GetLowerScrollArrowBounds(), Orientation == Orientation.Vertical ? ArrowDirection.Down : ArrowDirection.Right);

                // Paint the thumb.

                PaintThumb(e.Graphics);

            }

        }

        protected override void OnMouseMove(MouseEventArgs e) {

            base.OnMouseMove(e);

            bool newIsMouseOnThumb = isDragging ||
                GetThumbBounds().Contains(e.Location);

            bool newIsMouseOnUpperArrow = !isDragging &&
                GetUpperScrollArrowBounds().Contains(e.Location);

            bool newIsMouseOnLowerArrow = !isDragging &&
                GetLowerScrollArrowBounds().Contains(e.Location);

            if (newIsMouseOnThumb != isMouseOnThumb || newIsMouseOnUpperArrow != isMouseOnUpperArrow || newIsMouseOnLowerArrow != isMouseOnLowerArrow)
                Invalidate();

            isMouseOnThumb = newIsMouseOnThumb;
            isMouseOnUpperArrow = newIsMouseOnUpperArrow;
            isMouseOnLowerArrow = newIsMouseOnLowerArrow;

            if (isDragging) {

                int dragDistance = Orientation == Orientation.Vertical ?
                    e.Y - draggingMouseOrigin.Y :
                    e.X - draggingMouseOrigin.X;

                SetThumbOffset(draggingThumbOrigin + dragDistance);

            }

        }
        protected override void OnMouseLeave(EventArgs e) {

            base.OnMouseLeave(e);

            if (isMouseOnThumb) {

                isMouseOnThumb = false;
                isMouseOnUpperArrow = false;
                isMouseOnLowerArrow = false;

                Invalidate();

            }

        }
        protected override void OnMouseDown(MouseEventArgs e) {

            base.OnMouseDown(e);

            if (isMouseOnThumb) {

                isDragging = true;
                draggingMouseOrigin = e.Location;
                draggingThumbOrigin = GetThumbOffset();

            }
            else if (GetUpperScrollArrowBounds().Contains(e.Location)) {

                Value -= SmallChange;

            }
            else if (GetLowerScrollArrowBounds().Contains(e.Location)) {

                Value += SmallChange;

            }
            else if (GetTrackBounds().Contains(e.Location)) {

                Rectangle thumbBounds = GetThumbBounds();

                int thumbOffset = Orientation == Orientation.Vertical ?
                    thumbBounds.Y :
                    thumbBounds.X;

                int clickOffset = Orientation == Orientation.Vertical ?
                    e.Location.Y :
                    e.Location.X;

                if (clickOffset < thumbOffset) {

                    Value -= LargeChange;

                }
                else if (clickOffset > thumbOffset) {

                    Value += LargeChange;

                }

            }

        }
        protected override void OnMouseUp(MouseEventArgs e) {

            isDragging = false;

        }

        // Private members

        private int smallChange = 1;
        private int largeChange = 10;
        private int value = 0;

        private bool isMouseOnThumb = false;
        private bool isMouseOnUpperArrow = false;
        private bool isMouseOnLowerArrow = false;
        private bool isDragging = false;
        private Point draggingMouseOrigin = new Point(0, 0);
        private int draggingThumbOrigin = 0;

        private int GetTrackLength() {

            Size scrollArrowSize = GetScrollArrowSize();

            return Orientation == Orientation.Vertical ?
                Height - (scrollArrowSize.Height * 2) :
                Width - (scrollArrowSize.Width * 2);

        }
        private int GetScrollableTrackLength() {

            return GetTrackLength() - GetThumbLength();

        }
        private int GetThumbLength() {

            int minimumThumbLength = GetMinimumThumbLength();
            int trackLength = GetTrackLength();
            int viewportLength = GetViewportLength();
            int contentLength = GetContentLength();

            return Maximum > 0 ?
                MathUtilities.Clamp((int)(trackLength * (viewportLength / (double)contentLength)), minimumThumbLength, trackLength) :
                0;

        }
        private int GetMinimumThumbLength() {

            Size minimumThumbSize = GetMinimumThumbSize();

            return Orientation == Orientation.Vertical ?
                minimumThumbSize.Height :
                minimumThumbSize.Width;

        }
        private int GetViewportLength() {

            // Assume the length of the scrollbar matches the viewport.

            return Orientation == Orientation.Vertical ?
                Height :
                Width;

        }
        private int GetContentLength() {

            // Assume Maximum is set to the content size minus the viewport size.
            // https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.scrollbar.largechange

            return Maximum + GetViewportLength();

        }
        private void SetValue(int value) {

            value = MathUtilities.Clamp(value, Minimum, Maximum);

            if (this.value != value) {

                this.value = value;

                ValueChanged?.Invoke(this, EventArgs.Empty);

                Invalidate();

            }

        }
        private void SetThumbOffset(int value) {

            // Set the value according to the thumb offset we want.

            int trackLength = GetTrackLength();
            int thumbLength = GetThumbLength();
            int maxOffset = trackLength - thumbLength;

            value = MathUtilities.Clamp(value, 0, maxOffset);

            int newScrollValue = (int)(Minimum + (Maximum - Minimum) * (value / (double)maxOffset));

            SetValue(newScrollValue);

        }
        private int GetThumbOffset() {

            return Maximum > 0 ?
                (int)(GetScrollableTrackLength() * (value - Minimum) / ((double)Maximum - Minimum)) :
                0;

        }

        private Rectangle GetUpperScrollArrowBounds() {

            Size scrollArrowSize = GetScrollArrowSize();

            if (Orientation == Orientation.Vertical)
                return new Rectangle(0, 0, Width, scrollArrowSize.Height);

            return new Rectangle(0, 0, scrollArrowSize.Width, Height);

        }
        private Rectangle GetLowerScrollArrowBounds() {

            Size scrollArrowSize = GetScrollArrowSize();

            if (Orientation == Orientation.Vertical)
                return new Rectangle(0, Height - scrollArrowSize.Height, Width, scrollArrowSize.Height);

            return new Rectangle(Width - scrollArrowSize.Width, 0, scrollArrowSize.Width, Height);

        }
        private Rectangle GetTrackBounds() {

            Size scrollArrowSize = GetScrollArrowSize();

            return Orientation == Orientation.Vertical ?
                new Rectangle(0, scrollArrowSize.Height, Width, GetTrackLength()) :
                new Rectangle(scrollArrowSize.Width, 0, GetTrackLength(), Height);

        }
        private Rectangle GetThumbBounds() {

            Rectangle trackRect = GetTrackBounds();
            int thumbLength = GetThumbLength();
            int thumbOffset = GetThumbOffset();

            return Orientation == Orientation.Vertical ?
                new Rectangle(trackRect.X, trackRect.Y + thumbOffset, Width, thumbLength) :
                new Rectangle(trackRect.X + thumbOffset, trackRect.Y, thumbLength, Height);

        }

        private void PaintThumb(Graphics graphics) {

            if (graphics is null)
                throw new ArgumentNullException(nameof(graphics));

            Color thumbColor = ForeColor;

            if (isMouseOnThumb)
                thumbColor = ColorUtilities.Shade(thumbColor, 0.2f);

            using (SolidBrush brush = new SolidBrush(thumbColor))
                graphics.FillRectangle(brush, GetThumbBounds());

        }
        private void PaintScrollArrow(Graphics graphics, Rectangle bounds, ArrowDirection direction) {

            if (graphics is null)
                throw new ArgumentNullException(nameof(graphics));

            // Paint the background.

            if (isMouseOnUpperArrow && (direction == ArrowDirection.Up || direction == ArrowDirection.Left) ||
                isMouseOnLowerArrow && (direction == ArrowDirection.Down || direction == ArrowDirection.Right)) {

                Color backgroundColor = ColorUtilities.Shade(BackColor, 0.2f);

                using (SolidBrush brush = new SolidBrush(backgroundColor))
                    graphics.FillRectangle(brush, bounds);

            }

            // Paint the button triangle.

            int arrowBoundsWidth = bounds.Width / 2;
            int arrowBoundsHeight = bounds.Height / 3;

            Rectangle arrowBounds = new Rectangle(
                bounds.X + (bounds.Width / 2) - (arrowBoundsWidth / 2),
                bounds.Y + (bounds.Height / 2) - (arrowBoundsHeight / 2),
                arrowBoundsWidth,
                arrowBoundsHeight
                );

            Color arrowColor = ColorUtilities.Tint(ForeColor, 0.5f);

            graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            using (Brush brush = new SolidBrush(arrowColor))
                graphics.FillTriangle(brush, arrowBounds, direction);

        }

        private static Size GetScrollArrowSize() {

            return new Size(15, 17);

        }
        private static Size GetMinimumThumbSize() {

            // https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.primitives.track.thumb

            return new Size(
                (int)Math.Ceiling(SystemInformation.HorizontalScrollBarThumbWidth / 2.0),
                (int)Math.Ceiling(SystemInformation.VerticalScrollBarThumbHeight / 2.0)
                );

        }

    }

}