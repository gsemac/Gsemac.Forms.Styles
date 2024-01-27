using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers.Extensions {

    public enum ImageSizeMode {
        Normal = 0,
        Stretch = 1,
        Center = 3,
        Zoom = 4
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

        public static GraphicsPath CreateTriangle(Rectangle bounds, ArrowDirection orientation) {

            GraphicsPath path = new GraphicsPath();

            int halfX = bounds.Width / 2;
            //int halfY = bounds.Height / 2;

            switch (orientation) {

                case ArrowDirection.Up:

                    path.AddLine(bounds.X, bounds.Y + bounds.Height, bounds.X + halfX, bounds.Y); // bottom-left to top
                    path.AddLine(bounds.X + halfX, bounds.Y, bounds.X + bounds.Width, bounds.Y + bounds.Height); // top to bottom-right

                    break;

                case ArrowDirection.Down:

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

        public static void DrawTriangle(this Graphics graphics, Pen pen, Rectangle bounds, ArrowDirection orientation) {

            if (graphics is null)
                throw new ArgumentNullException(nameof(graphics));

            if (pen is null)
                throw new ArgumentNullException(nameof(pen));

            using (GraphicsPath path = CreateTriangle(bounds, orientation))
                graphics.DrawPath(pen, path);

        }
        public static void FillTriangle(this Graphics graphics, Brush brush, Rectangle bounds, ArrowDirection orientation) {

            if (graphics is null)
                throw new ArgumentNullException(nameof(graphics));

            if (brush is null)
                throw new ArgumentNullException(nameof(brush));

            using (GraphicsPath path = CreateTriangle(bounds, orientation))
                graphics.FillPath(brush, path);

        }

        public static void DrawImage(this Graphics graphics, Image image, Rectangle bounds, ImageSizeMode sizeMode) {

            int x = bounds.X;
            int y = bounds.Y;
            int w = image.Width;
            int h = image.Height;

            switch (sizeMode) {

                case ImageSizeMode.Stretch:

                    w = bounds.Width;
                    h = bounds.Height;

                    break;

                case ImageSizeMode.Center:

                    x += (int)((float)bounds.Width / 2 - (float)w / 2);
                    y += (int)((float)bounds.Height / 2 - (float)h / 2);

                    break;

                case ImageSizeMode.Zoom:

                    float scaleFactor = Math.Min((float)bounds.Width / image.Width, (float)bounds.Height / image.Height);

                    w = (int)(w * scaleFactor);
                    h = (int)(h * scaleFactor);

                    x += (int)((float)bounds.Width / 2 - (float)w / 2);
                    y += (int)((float)bounds.Height / 2 - (float)h / 2);

                    break;

            }

            graphics.DrawImage(image, x, y, w, h);

        }
        public static void DrawImage(this Graphics graphics, Image image, Rectangle bounds, ContentAlignment alignment) {

            int x = bounds.X;
            int y = bounds.Y;

            if (alignment == ContentAlignment.TopRight || alignment == ContentAlignment.MiddleRight || alignment == ContentAlignment.BottomRight)
                x = bounds.X + bounds.Width - image.Width;
            else if (alignment == ContentAlignment.TopCenter || alignment == ContentAlignment.MiddleCenter || alignment == ContentAlignment.BottomCenter)
                x = bounds.X + (int)((float)bounds.Width / 2 - (float)image.Width / 2);

            if (alignment == ContentAlignment.BottomLeft || alignment == ContentAlignment.BottomCenter || alignment == ContentAlignment.BottomRight)
                y = bounds.Y + bounds.Height - image.Height;
            else if (alignment == ContentAlignment.MiddleLeft || alignment == ContentAlignment.MiddleCenter || alignment == ContentAlignment.MiddleRight)
                y = bounds.Y + (int)((float)bounds.Height / 2 - (float)image.Height / 2);

            graphics.DrawImage(image, x, y, image.Width, image.Height);
        }

        public static void AddPoint(this GraphicsPath path, Point point) {

            path.AddLine(point, point);

        }

    }

}