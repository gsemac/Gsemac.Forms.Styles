using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.Extensions {

    internal enum TriangleOrientation {
        Up,
        Left,
        Right,
        Down
    }

    internal static class GraphicsExtensions {

        // RoundedRectangle code adapted from https://stackoverflow.com/a/33853557/5383169

        public static GraphicsPath CreateRoundedRectangle(Rectangle bounds, int radius) {

            int diameter = radius * 2;

            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);

            GraphicsPath path = new GraphicsPath();

            if (radius == 0) {

                path.AddRectangle(bounds);

            }
            else {

                // top left arc  

                path.AddArc(arc, 180, 90);

                // top right arc  

                arc.X = bounds.Right - diameter;

                path.AddArc(arc, 270, 90);

                // bottom right arc  

                arc.Y = bounds.Bottom - diameter;

                path.AddArc(arc, 0, 90);

                // bottom left arc 

                arc.X = bounds.Left;

                path.AddArc(arc, 90, 90);

                path.CloseFigure();

            }

            return path;
        }
        public static GraphicsPath CreateTriangle(Rectangle bounds, TriangleOrientation orientation) {

            GraphicsPath path = new GraphicsPath();

            int halfX = bounds.Width / 2;
            int halfY = bounds.Height / 2;

            switch (orientation) {

                case TriangleOrientation.Up:

                    path.AddLine(bounds.X, bounds.Y + bounds.Height, bounds.X + halfX, bounds.Y); // bottom-left to top
                    path.AddLine(bounds.X + halfX, bounds.Y, bounds.X + bounds.Width, bounds.Y + bounds.Height); // top to bottom-right

                    break;

                case TriangleOrientation.Down:

                    path.AddLine(bounds.X, bounds.Y, bounds.X + halfX, bounds.Y + bounds.Height); // top-left to bottom
                    path.AddLine(bounds.X + halfX, bounds.Y + bounds.Height, bounds.X + bounds.Width, bounds.Y); // bottom to top-right

                    break;

            }

            path.CloseFigure();

            return path;

        }

        public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, Rectangle bounds, int cornerRadius) {

            if (graphics is null)
                throw new ArgumentNullException(nameof(graphics));

            if (pen is null)
                throw new ArgumentNullException(nameof(pen));

            using (GraphicsPath path = CreateRoundedRectangle(bounds, cornerRadius))
                graphics.DrawPath(pen, path);

        }
        public static void FillRoundedRectangle(this Graphics graphics, Brush brush, Rectangle bounds, int cornerRadius) {

            if (graphics is null)
                throw new ArgumentNullException(nameof(graphics));

            if (brush is null)
                throw new ArgumentNullException(nameof(brush));

            using (GraphicsPath path = CreateRoundedRectangle(bounds, cornerRadius))
                graphics.FillPath(brush, path);

        }

        public static void DrawTriangle(this Graphics graphics, Pen pen, Rectangle bounds, TriangleOrientation orientation) {

            if (graphics is null)
                throw new ArgumentNullException(nameof(graphics));

            if (pen is null)
                throw new ArgumentNullException(nameof(pen));

            using (GraphicsPath path = CreateTriangle(bounds, orientation))
                graphics.DrawPath(pen, path);

        }
        public static void FillTriangle(this Graphics graphics, Brush brush, Rectangle bounds, TriangleOrientation orientation) {

            if (graphics is null)
                throw new ArgumentNullException(nameof(graphics));

            if (brush is null)
                throw new ArgumentNullException(nameof(brush));

            using (GraphicsPath path = CreateTriangle(bounds, orientation))
                graphics.FillPath(brush, path);

        }

    }

}