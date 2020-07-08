using Gsemac.Forms.Styles.Renderers.Extensions;
using Gsemac.Forms.Styles.StyleSheets;
using Gsemac.Forms.Styles.StyleSheets.Extensions;
using Gsemac.Forms.Utilities;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
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

            if (ruleset.BackgroundColor.HasValue() && ruleset.BackgroundColor.Value != control.BackColor) {

                Color backColor = ruleset.BackgroundColor.Value;

                if (backColor.A != byte.MaxValue && !ControlUtilities.GetStyle(control, ControlStyles.SupportsTransparentBackColor))
                    backColor = Color.FromArgb(backColor.R, backColor.G, backColor.B);

                control.BackColor = backColor;

            }

            if (ruleset.Color.HasValue() && ruleset.Color.Value != control.ForeColor)
                control.ForeColor = ruleset.Color.Value;

        }

        public static Color GetColorWithAlpha(Color baseColor, float alpha) {

            return Color.FromArgb((int)Math.Round(byte.MaxValue * alpha), baseColor.R, baseColor.G, baseColor.B);

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