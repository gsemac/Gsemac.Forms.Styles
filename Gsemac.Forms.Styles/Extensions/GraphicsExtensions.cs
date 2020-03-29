using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.Extensions {

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

        public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, Rectangle bounds, int cornerRadius) {

            if (graphics == null)
                throw new ArgumentNullException(nameof(graphics));

            if (pen == null)
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

    }

}