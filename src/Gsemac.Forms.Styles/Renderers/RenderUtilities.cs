using Gsemac.Forms.Styles.Renderers.Extensions;
using Gsemac.Forms.Styles.StyleSheets.Properties;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
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

        public static void ApplyColorProperties(Control control, IRuleset style) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            if (style is null)
                throw new ArgumentNullException(nameof(style));

            if (style.ContainsKey(PropertyName.BackgroundColor) && style.BackgroundColor != control.BackColor) {

                Color backColor = style.BackgroundColor;

                if (backColor.A != byte.MaxValue && !ControlUtilities.GetStyle(control, ControlStyles.SupportsTransparentBackColor))
                    backColor = Color.FromArgb(backColor.R, backColor.G, backColor.B);

                control.BackColor = backColor;

            }

            if (style.ContainsKey(PropertyName.Color) && style.Color != control.ForeColor)
                control.ForeColor = style.Color;

        }

        public static DashStyle GetDashStyle(StyleSheets.Properties.BorderStyle borderStyle) {

            switch (borderStyle) {

                case StyleSheets.Properties.BorderStyle.Dashed:
                    return DashStyle.Dash;

                case StyleSheets.Properties.BorderStyle.Dotted:
                    return DashStyle.Dot;

                default:
                    return DashStyle.Solid;

            }

        }
        public static Color GetColorWithAlpha(Color baseColor, float alpha) {

            return Color.FromArgb((int)Math.Round(byte.MaxValue * alpha), baseColor.R, baseColor.G, baseColor.B);

        }

        public static void ClipToBorder(Graphics graphics, Rectangle rectangle, IRuleset ruleset) {

            Region region = new Region(rectangle);

            if (ruleset.Any(p => PropertyUtilities.IsBorderRadiusProperty(p))) {

                region.Intersect(GraphicsExtensions.CreateRoundedRectangle(rectangle,
                    (int)(ruleset.BorderTopLeftRadius?.Value ?? 0),
                    (int)(ruleset.BorderTopRightRadius?.Value ?? 0),
                    (int)(ruleset.BorderBottomLeftRadius?.Value ?? 0),
                    (int)(ruleset.BorderBottomRightRadius?.Value ?? 0)));

            }
            else {

                region.Intersect(rectangle);

            }

            graphics.SetClip(region, CombineMode.Intersect);

        }

        public static void DrawBackground(Graphics graphics, Rectangle rectangle, IRuleset ruleset) {

            GraphicsState state = graphics.Save();

            bool hasRadius = ruleset.BorderRadius.Any(r => r.Value > 0);
            bool hasRightRadius = hasRadius && (ruleset.BorderTopRightRadius?.Value > 0 || ruleset.BorderBottomRightRadius?.Value > 0);
            bool hasBottomRadius = hasRadius && (ruleset.BorderBottomLeftRadius?.Value > 0 || ruleset.BorderBottomRightRadius?.Value > 0);

            if (hasRadius)
                graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Draw the background color.

            Rectangle backgroundRect = rectangle;

            // If the rectangle has right or bottom corner radii, the bounds must be decreased to ensure the curve is not clipped.

            if (hasRadius)
                backgroundRect = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width - (hasRightRadius ? 1 : 0), rectangle.Height - (hasBottomRadius ? 1 : 0));

            int topLeft = (int)(ruleset.BorderTopLeftRadius?.Value ?? 0);
            int topRight = (int)(ruleset.BorderTopRightRadius?.Value ?? 0);
            int bottomLeft = (int)(ruleset.BorderBottomLeftRadius?.Value ?? 0);
            int bottomRight = (int)(ruleset.BorderBottomRightRadius?.Value ?? 0);

            if (ruleset.ContainsKey(PropertyName.BackgroundColor)) {

                Color backgroundColor = ruleset.BackgroundColor; //?.Value ?? SystemColors.Control;

                if (ruleset.ContainsKey(PropertyName.Opacity))
                    backgroundColor = GetColorWithAlpha(backgroundColor, (float)ruleset.Opacity);

                using (Brush brush = new SolidBrush(backgroundColor)) {

                    if (!hasRadius)
                        graphics.FillRectangle(brush, backgroundRect);
                    else
                        graphics.FillRoundedRectangle(brush, backgroundRect, topLeft, topRight, bottomLeft, bottomRight);
                }

            }

            // Draw the background image.

            //if (ruleset.Contains(PropertyName.BackgroundImage)) {

            //    ClipToBorder(graphics, backgroundRect, ruleset);

            //    foreach (IImage image in ruleset.BackgroundImage.Images)
            //        graphics.DrawImage(image, backgroundRect);

            //}

            graphics.Restore(state);

        }
        public static void DrawBorder(Graphics graphics, Rectangle rectangle, IRuleset ruleset) {

            GraphicsState state = graphics.Save();

            bool hasRadius = ruleset.BorderRadius.Any(r => r.Value > 0);

            if (hasRadius)
                graphics.SmoothingMode = SmoothingMode.AntiAlias;

            double topWidth = ruleset.BorderTopWidth?.Value ?? 0;
            double rightWidth = ruleset.BorderRightWidth?.Value ?? 0;
            double bottomWidth = ruleset.BorderBottomWidth?.Value ?? 0;
            double leftWidth = ruleset.BorderLeftWidth?.Value ?? 0;

            double topLeftRadius = ruleset.BorderTopLeftRadius?.Value ?? 0;
            double topRightRadius = ruleset.BorderTopRightRadius?.Value ?? 0;
            double bottomRightRadius = ruleset.BorderBottomRightRadius?.Value ?? 0;
            double bottomLeftRadius = ruleset.BorderBottomLeftRadius?.Value ?? 0;

            double horizontalBorderWidth = leftWidth + rightWidth;
            double verticalBorderWidth = topWidth + bottomWidth;

            int rectX = rectangle.X + (int)(leftWidth / 2);
            int rectY = rectangle.Y + (int)(topWidth / 2);
            int rectWidth = rectangle.Width - (int)(horizontalBorderWidth / 2);
            int rectHeight = rectangle.Height - (int)(verticalBorderWidth / 2);

            Rectangle drawRect = new Rectangle(rectX, rectY, rectWidth - (rightWidth == 1 && leftWidth <= 0 ? 1 : 0), rectHeight - (bottomWidth == 1 && topWidth <= 0 ? 1 : 0));

            float opacity = (float)ruleset.Opacity;

            using (Pen pen = new Pen(Color.Black)) {

                pen.Alignment = PenAlignment.Center;
                pen.StartCap = LineCap.Square;

                if (topWidth > 0) {

                    StyleSheets.Properties.BorderStyle borderStyle = ruleset.BorderTopStyle;

                    if (borderStyle != StyleSheets.Properties.BorderStyle.None && borderStyle != StyleSheets.Properties.BorderStyle.Hidden) {

                        pen.Width = (float)topWidth;
                        pen.Color = GetColorWithAlpha(ruleset.BorderTopColor, opacity);
                        pen.DashStyle = GetDashStyle(borderStyle);

                        using (GraphicsPath path = CreateBorderPath(drawRect, BorderPathType.Top, topLeftRadius, topRightRadius, bottomRightRadius, bottomLeftRadius))
                            graphics.DrawPath(pen, path);

                    }

                }

                if (rightWidth > 0) {

                    StyleSheets.Properties.BorderStyle borderStyle = ruleset.BorderRightStyle;

                    if (borderStyle != StyleSheets.Properties.BorderStyle.None && borderStyle != StyleSheets.Properties.BorderStyle.Hidden) {

                        pen.Width = (float)rightWidth;
                        pen.Color = GetColorWithAlpha(ruleset.BorderRightColor, opacity);
                        pen.DashStyle = GetDashStyle(borderStyle);

                        using (GraphicsPath path = CreateBorderPath(drawRect, BorderPathType.Right, topLeftRadius, topRightRadius, bottomRightRadius, bottomLeftRadius))
                            graphics.DrawPath(pen, path);

                    }

                }

                if (bottomWidth > 0) {

                    StyleSheets.Properties.BorderStyle borderStyle = ruleset.BorderBottomStyle;

                    if (borderStyle != StyleSheets.Properties.BorderStyle.None && borderStyle != StyleSheets.Properties.BorderStyle.Hidden) {

                        pen.Width = (float)bottomWidth;
                        pen.Color = GetColorWithAlpha(ruleset.BorderBottomColor, opacity);
                        pen.DashStyle = GetDashStyle(borderStyle);

                        using (GraphicsPath path = CreateBorderPath(drawRect, BorderPathType.Bottom, topLeftRadius, topRightRadius, bottomRightRadius, bottomLeftRadius))
                            graphics.DrawPath(pen, path);

                    }

                }

                if (leftWidth > 0) {

                    StyleSheets.Properties.BorderStyle borderStyle = ruleset.BorderLeftStyle;

                    if (borderStyle != StyleSheets.Properties.BorderStyle.None && borderStyle != StyleSheets.Properties.BorderStyle.Hidden) {

                        pen.Width = (float)leftWidth;
                        pen.Color = GetColorWithAlpha(ruleset.BorderLeftColor, opacity);
                        pen.DashStyle = GetDashStyle(borderStyle);

                        using (GraphicsPath path = CreateBorderPath(drawRect, BorderPathType.Left, topLeftRadius, topRightRadius, bottomRightRadius, bottomLeftRadius))
                            graphics.DrawPath(pen, path);

                    }

                }

            }

            graphics.Restore(state);

        }
        public static void DrawText(Graphics graphics, Rectangle rectangle, IRuleset ruleset, string text, Font font, TextFormatFlags textFormatFlags = TextFormatFlags.Left | TextFormatFlags.NoPrefix) {

            // Paint the foreground text.

            Color textColor = ruleset.Color;

            TextRenderer.DrawText(graphics, text, font, rectangle, textColor, textFormatFlags);

            //graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            //using (Brush brush = new SolidBrush(textColor))
            //    graphics.DrawString(text, font, brush, new RectangleF(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height));

        }

        // Private members

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