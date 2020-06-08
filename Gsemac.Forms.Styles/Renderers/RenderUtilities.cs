using Gsemac.Forms.Styles.Applicators;
using Gsemac.Forms.Styles.Extensions;
using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    internal enum BorderPathType {
        Top,
        Right,
        Bottom,
        Left,
        Full
    }

    internal static class RenderUtilities {

        // Public members

        public static bool MouseIntersectsWith(Control control) {

            return MouseIntersectsWith(control, control.ClientRectangle);

        }
        public static bool MouseIntersectsWith(Control control, Rectangle rect) {

            Point mousePos = control.PointToClient(Cursor.Position);
            Rectangle mouseRect = new Rectangle(mousePos.X, mousePos.Y, 1, 1);

            return rect.IntersectsWith(mouseRect);

        }

        public static TextFormatFlags GetTextFormatFlags(ContentAlignment contentAlignment) {

            TextFormatFlags flags = TextFormatFlags.Default;

            if ((contentAlignment & (ContentAlignment.TopLeft | ContentAlignment.TopCenter | ContentAlignment.TopRight)) != 0)
                flags |= TextFormatFlags.Top;

            if ((contentAlignment & (ContentAlignment.TopLeft | ContentAlignment.MiddleLeft | ContentAlignment.BottomLeft)) != 0)
                flags |= TextFormatFlags.Left;

            if ((contentAlignment & (ContentAlignment.TopRight | ContentAlignment.MiddleRight | ContentAlignment.BottomRight)) != 0)
                flags |= TextFormatFlags.Right;

            if ((contentAlignment & (ContentAlignment.BottomRight | ContentAlignment.BottomCenter | ContentAlignment.BottomLeft)) != 0)
                flags |= TextFormatFlags.Bottom;

            if ((contentAlignment & (ContentAlignment.MiddleRight | ContentAlignment.MiddleCenter | ContentAlignment.MiddleLeft)) != 0)
                flags |= TextFormatFlags.VerticalCenter;

            if ((contentAlignment & (ContentAlignment.TopCenter | ContentAlignment.MiddleCenter | ContentAlignment.BottomCenter)) != 0)
                flags |= TextFormatFlags.HorizontalCenter;

            return flags;

        }
        public static TextFormatFlags GetTextFormatFlags(HorizontalAlignment horizontalAlignment) {

            TextFormatFlags flags = TextFormatFlags.Default;

            if (horizontalAlignment == HorizontalAlignment.Left)
                flags |= TextFormatFlags.Left;
            else if (horizontalAlignment == HorizontalAlignment.Center)
                flags |= TextFormatFlags.HorizontalCenter;
            else if (horizontalAlignment == HorizontalAlignment.Right)
                flags |= TextFormatFlags.Right;

            return flags;

        }
        public static DashStyle GetDashStyle(StyleSheets.BorderStyle borderStyle) {

            switch (borderStyle) {

                case StyleSheets.BorderStyle.Dashed:
                    return DashStyle.Dash;

                case StyleSheets.BorderStyle.Dotted:
                    return DashStyle.Dot;

                default:
                    return DashStyle.Solid;

            }

        }
        public static void ApplyColorProperties(Control control, IRuleset ruleset) {

            if (ruleset.BackgroundColor.HasValue())
                control.BackColor = ruleset.BackgroundColor.Value;

            if (ruleset.Color.HasValue())
                control.ForeColor = ruleset.Color.Value;

        }

        public static ScrollBars GetVisibleScrollbars(ListBox control) {

            ScrollBars scrollBars = ScrollBars.None;

            if (Enumerable.Range(0, control.Items.Count).Sum(i => control.GetItemHeight(i)) > control.Height)
                scrollBars |= ScrollBars.Vertical;

            return scrollBars;

        }
        public static ScrollBars GetVisibleScrollbars(TextBox control) {

            return control.ScrollBars;

        }
        public static ScrollBars GetVisibleScrollbars(Control control) {

            ScrollBars scrollBars = ScrollBars.None;

            Size size = control.GetPreferredSize(Size.Empty);

            if (size.Height > control.Height)
                scrollBars |= ScrollBars.Vertical;

            if (size.Width > control.Width)
                scrollBars |= ScrollBars.Horizontal;

            return scrollBars;

        }

        public static Rectangle GetOuterBorderRectangle(Control control, IRuleset ruleset) {

            Borders borders = ruleset.GetBorders();

            Rectangle borderRect = control.ClientRectangle;

            borderRect = new Rectangle(
                borderRect.X - (int)borders.Left.Width,
                borderRect.Y - (int)borders.Top.Width,
                borderRect.Width + (int)borders.Left.Width + (int)borders.Right.Width,
                borderRect.Height + +(int)borders.Top.Width + (int)borders.Bottom.Width
                );

            return borderRect;

        }
        public static GraphicsPath CreateBorderPath(Rectangle bounds, BorderPathType type, double topLeftRadius, double topRightRadius, double bottomRightRadius, double bottomLeftRadius) {

            int topLeftDiameter = (int)(topLeftRadius * 2);
            int topRightDiameter = (int)(topRightRadius * 2);
            int bottomLeftDiameter = (int)(bottomLeftRadius * 2);
            int bottomRightDiameter = (int)(bottomRightRadius * 2);

            GraphicsPath path = new GraphicsPath();

            Size topLeftSize = new Size(topLeftDiameter, topLeftDiameter);
            Size topRightSize = new Size(topRightDiameter, topRightDiameter);
            Size bottomLeftSize = new Size(bottomLeftDiameter, bottomLeftDiameter);
            Size bottomRightSize = new Size(bottomRightDiameter, bottomRightDiameter);

            Rectangle topLeftArc = new Rectangle(bounds.Location, topLeftSize);
            Rectangle topRightArc = new Rectangle(new Point(bounds.Right - topRightDiameter, bounds.Location.Y), topRightSize);
            Rectangle bottomLeftArc = new Rectangle(new Point(bounds.Left, bounds.Bottom - bottomLeftDiameter), bottomLeftSize);
            Rectangle bottomRightArc = new Rectangle(new Point(bounds.Right - bottomRightDiameter, bounds.Bottom - bottomRightDiameter), bottomRightSize);

            int overlap = 10;

            if (type == BorderPathType.Top || type == BorderPathType.Full) {

                if (topLeftDiameter > 0)
                    path.AddArc(topLeftArc, 225 - overlap, 45 + overlap);
                else
                    path.AddPoint(bounds.Location);

                if (topRightDiameter > 0)
                    path.AddArc(topRightArc, 270, 45 + overlap);
                else
                    path.AddPoint(new Point(bounds.Right, bounds.Top));

            }

            if (type == BorderPathType.Right || type == BorderPathType.Full) {

                if (topRightDiameter > 0)
                    path.AddArc(topRightArc, 315 - overlap, 45 + overlap);
                else if (type != BorderPathType.Full)
                    path.AddPoint(new Point(bounds.Right, bounds.Top));

                if (bottomRightDiameter > 0)
                    path.AddArc(bottomRightArc, 0, 45 + overlap);
                else
                    path.AddPoint(new Point(bounds.Right, bounds.Bottom));

            }

            if (type == BorderPathType.Bottom || type == BorderPathType.Full) {

                if (bottomRightDiameter > 0)
                    path.AddArc(bottomRightArc, 45 - overlap, 45 + overlap);
                else if (type != BorderPathType.Full)
                    path.AddPoint(new Point(bounds.Right, bounds.Bottom));

                if (bottomLeftDiameter > 0)
                    path.AddArc(bottomLeftArc, 90, 45 + overlap);
                else
                    path.AddPoint(new Point(bounds.Left, bounds.Bottom));

            }

            if (type == BorderPathType.Left || type == BorderPathType.Full) {

                if (bottomLeftDiameter > 0)
                    path.AddArc(bottomLeftArc, 135 - overlap, 45 + overlap);
                else if (type != BorderPathType.Full)
                    path.AddPoint(new Point(bounds.Left, bounds.Bottom));

                if (topLeftDiameter > 0)
                    path.AddArc(topLeftArc, 180, 45 + overlap);
                else if (type != BorderPathType.Full)
                    path.AddPoint(bounds.Location);

            }

            if (type == BorderPathType.Full)
                path.CloseFigure();

            return path;

        }

    }

}