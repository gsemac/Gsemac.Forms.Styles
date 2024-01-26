using Gsemac.Drawing;
using Gsemac.Forms.Styles.Dom;
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
        public int Value {
            get => value;
            set => SetValue(value);
        }
        public int SmallChange { get; set; } = 1;
        public int LargeChange { get; set; } = 10;

        public WrapperControlScrollBar() {

            // Enable user paint so that the control can be custom painted.

            DoubleBuffered = true;

        }

        // Protected members

        protected override void OnPaint(PaintEventArgs e) {

            base.OnPaint(e);

            using (SolidBrush brush = new SolidBrush(ForeColor)) {

                // Paint the scroll arrows.

                e.Graphics.FillRectangle(brush, GetUpperScrollArrowBounds());
                e.Graphics.FillRectangle(brush, GetLowerScrollArrowBounds());

                // Paint the thumb.

                PaintThumb(e.Graphics);

            }

        }

        protected override void OnMouseMove(MouseEventArgs e) {

            base.OnMouseMove(e);

            bool newIsMouseOnThumb = isDragging ||
                GetThumbBounds().Contains(e.Location);

            if (newIsMouseOnThumb != isMouseOnThumb) {

                isMouseOnThumb = newIsMouseOnThumb;

                Invalidate();

            }

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

        }
        protected override void OnMouseUp(MouseEventArgs e) {

            isDragging = false;

        }

        // Private members

        private bool isMouseOnThumb = false;
        private bool isDragging = false;
        private Point draggingMouseOrigin = new Point(0, 0);
        private int draggingThumbOrigin = 0;
        private int value = 50;

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

            if (Orientation == Orientation.Vertical) {

                int trackLength = Height - (scrollArrowSize.Height * 2);

                return new Rectangle(0, scrollArrowSize.Height, Width, trackLength);

            }
            else {

                int trackLength = Width - (scrollArrowSize.Width * 2);

                return new Rectangle(scrollArrowSize.Width, 0, trackLength, Height);

            }

        }
        private Rectangle GetThumbBounds() {

            Size minimumThumbSize = GetMinimumThumbSize();
            Rectangle trackRect = GetTrackBounds();

            if (Orientation == Orientation.Vertical) {

                int trackLength = trackRect.Height;
                int thumbLength = MathUtilities.Clamp((int)(trackLength * (LargeChange / (double)Maximum)), minimumThumbSize.Height, trackLength);

                return new Rectangle(trackRect.X, trackRect.Y + (int)((trackLength - thumbLength) * (value / (double)Maximum)), Width, thumbLength);

            }
            else {

                int trackLength = trackRect.Width;
                int thumbLength = MathUtilities.Clamp((int)(trackLength * (LargeChange / (double)Maximum)), minimumThumbSize.Width, trackLength);

                return new Rectangle(trackRect.X + (int)((trackLength - thumbLength) * (value / (double)Maximum)), trackRect.Y, thumbLength, Height);

            }

        }

        private int GetTrackLength() {

            Rectangle bounds = GetTrackBounds();

            return Orientation == Orientation.Vertical ?
                bounds.Height :
                bounds.Width;

        }
        private int GetThumbLength() {

            Rectangle bounds = GetThumbBounds();

            return Orientation == Orientation.Vertical ?
                bounds.Height :
                bounds.Width;

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

            Rectangle trackBounds = GetTrackBounds();
            Rectangle bounds = GetThumbBounds();

            return Orientation == Orientation.Vertical ?
                bounds.Y - trackBounds.Y :
                bounds.X - trackBounds.X;

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