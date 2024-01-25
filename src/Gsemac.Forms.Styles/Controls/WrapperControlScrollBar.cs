using Gsemac.Forms.Styles.Dom;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Controls {

    [DomHidden]
    internal class WrapperControlScrollBar :
        UserControl {

        // Public members

        public override bool AutoSize {
            get => base.AutoSize;
            set => base.AutoSize = value;
        }
        public Orientation Orientation { get; set; } = Orientation.Vertical;
        public int Minimum { get; set; } = 0;
        public int Maximum { get; set; } = 100;
        public int Value { get; set; } = 0;

        public WrapperControlScrollBar() {

            // Enable user paint so that the control can be custom painted.

            DoubleBuffered = true;

        }

        // Protected members

        protected override void OnPaint(PaintEventArgs e) {

            base.OnPaint(e);

            Size scrollArrowSize = GetScrollArrowSize();
            Size minimumThumbSize = GetMinimumThumbSize();

            if (Orientation == Orientation.Vertical) {

                using (SolidBrush brush = new SolidBrush(ForeColor)) {

                    // Paint the scroll arrows.

                    Rectangle topScrollArrowRect = new Rectangle(0, 0, Width, scrollArrowSize.Height);
                    Rectangle bottomScrollArrowRect = new Rectangle(0, Height - scrollArrowSize.Height, Width, scrollArrowSize.Height);

                    e.Graphics.FillRectangle(brush, topScrollArrowRect);
                    e.Graphics.FillRectangle(brush, bottomScrollArrowRect);

                    // Paint the thumb.

                    int trackLength = Height - (scrollArrowSize.Height * 2);
                    int thumbLength = Math.Max(Math.Min(trackLength * (trackLength / Maximum), trackLength), minimumThumbSize.Height);
                    Rectangle trackRect = new Rectangle(0, scrollArrowSize.Height, Width, trackLength);
                    Rectangle thumbRect = new Rectangle(trackRect.X, trackRect.Y, Width, trackRect.Y + thumbLength);

                    e.Graphics.FillRectangle(brush, thumbRect);

                }

            }

        }

        protected override void OnMouseDown(MouseEventArgs e) {

            base.OnMouseDown(e);

        }

        // Private members

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