using Gsemac.Forms.Styles.StyleSheets;
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

        public static GraphicsPath CreateRoundedRectangle(Rectangle bounds, int topLeft, int topRight, int bottomLeft, int bottomRight) {

            int topLeftDiameter = topLeft * 2;
            int topRightDiameter = topRight * 2;
            int bottomLeftDiameter = bottomLeft * 2;
            int bottomRightDiameter = bottomRight * 2;

            GraphicsPath path = new GraphicsPath();

            if (topLeftDiameter <= 0 && topRightDiameter <= 0 && bottomLeftDiameter <= 0 && bottomRightDiameter <= 0) {

                path.AddRectangle(bounds);

            }
            else {

                Size topLeftSize = new Size(topLeftDiameter, topLeftDiameter);
                Size topRightSize = new Size(topRightDiameter, topRightDiameter);
                Size bottomLeftSize = new Size(bottomLeftDiameter, bottomLeftDiameter);
                Size bottomRightSize = new Size(bottomRightDiameter, bottomRightDiameter);

                Rectangle topLeftArc = new Rectangle(bounds.Location, topLeftSize);
                Rectangle topRightArc = new Rectangle(new Point(bounds.Right - topRightDiameter, bounds.Location.Y), topRightSize);
                Rectangle bottomLeftArc = new Rectangle(new Point(bounds.Left, bounds.Bottom - bottomLeftDiameter), bottomLeftSize);
                Rectangle bottomRightArc = new Rectangle(new Point(bounds.Right - bottomRightDiameter, bounds.Bottom - bottomRightDiameter), bottomRightSize);

                if (topLeftDiameter > 0)
                    path.AddArc(topLeftArc, 180, 90);
                else
                    path.AddLine(bounds.Location, bounds.Location);

                if (topRightDiameter > 0)
                    path.AddArc(topRightArc, 270, 90);
                else
                    path.AddLine(new Point(bounds.Right, bounds.Top), new Point(bounds.Right, bounds.Top));

                if (bottomRightDiameter > 0)
                    path.AddArc(bottomRightArc, 0, 90);
                else
                    path.AddLine(new Point(bounds.Right, bounds.Bottom), new Point(bounds.Right, bounds.Bottom));

                if (bottomLeftDiameter > 0)
                    path.AddArc(bottomLeftArc, 90, 90);
                else
                    path.AddLine(new Point(bounds.Left, bounds.Bottom), new Point(bounds.Left, bounds.Bottom));

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

        public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, Rectangle bounds, int topLeft, int topRight, int bottomLeft, int bottomRight) {

            if (graphics is null)
                throw new ArgumentNullException(nameof(graphics));

            if (pen is null)
                throw new ArgumentNullException(nameof(pen));

            using (GraphicsPath path = CreateRoundedRectangle(bounds, topLeft, topRight, bottomLeft, bottomRight))
                graphics.DrawPath(pen, path);

        }
        public static void FillRoundedRectangle(this Graphics graphics, Brush brush, Rectangle bounds, int topLeft, int topRight, int bottomLeft, int bottomRight) {

            if (graphics is null)
                throw new ArgumentNullException(nameof(graphics));

            if (brush is null)
                throw new ArgumentNullException(nameof(brush));

            using (GraphicsPath path = CreateRoundedRectangle(bounds, topLeft, topRight, bottomLeft, bottomRight)) {

                graphics.FillPath(brush, path);

                using (Pen pen = new Pen(brush))
                    graphics.DrawPath(pen, path);

            }

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

        public static void AddPoint(this GraphicsPath path, Point point) {

            path.AddLine(point, point);

        }

    }

}