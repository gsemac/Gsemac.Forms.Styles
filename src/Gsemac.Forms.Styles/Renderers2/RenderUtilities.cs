using Gsemac.Forms.Styles.Properties;
using Gsemac.Forms.Styles.Renderers.Extensions;
using Gsemac.Forms.Styles.StyleSheets.Properties;
using Gsemac.Forms.Styles.StyleSheets.Properties.Extensions;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers2 {

    internal static class RenderUtilities {

        // Public members

        public static Color GetColorWithAlpha(Color baseColor, float alpha) {

            return Color.FromArgb((int)Math.Round(byte.MaxValue * alpha), baseColor.R, baseColor.G, baseColor.B);

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
        public static Rectangle GetOuterBorderRectangle(Rectangle bounds, IRuleset style) {

            Borders borders = style.Border;

            Rectangle borderRect = bounds;

            borderRect = new Rectangle(
                borderRect.X - (int)borders.Left.Width.Value,
                borderRect.Y - (int)borders.Top.Width.Value,
                borderRect.Width + (int)borders.Left.Width.Value + (int)borders.Right.Width.Value,
                borderRect.Height + +(int)borders.Top.Width.Value + (int)borders.Bottom.Width.Value
                );

            return borderRect;

        }

        public static void ClipToBorder(Graphics graphics, Rectangle bounds, IRuleset style) {

            Region region = new Region(bounds);

            if (style.Any(p => PropertyUtilities.IsBorderRadiusProperty(p))) {

                region.Intersect(GraphicsExtensions.CreateRoundedRectangle(bounds,
                    (int)(style.BorderTopLeftRadius?.Value ?? 0),
                    (int)(style.BorderTopRightRadius?.Value ?? 0),
                    (int)(style.BorderBottomLeftRadius?.Value ?? 0),
                    (int)(style.BorderBottomRightRadius?.Value ?? 0)));

            }
            else {

                region.Intersect(bounds);

            }

            graphics.SetClip(region, CombineMode.Intersect);

        }

        public static void Clear(Graphics graphics, IRuleset style) {

            IProperty clearColorProperty = style.Get(CustomPropertyName.ClearColor);

            if (clearColorProperty is object && clearColorProperty.Value.Is<Color>()) {

                graphics.Clear(clearColorProperty.Value.As<Color>());

            }
            else {

                graphics.Clear(Color.Transparent);

            }

        }

        public static void DrawBackground(Graphics graphics, Rectangle bounds, IRuleset style) {

            GraphicsState state = graphics.Save();

            bool hasRadius = style.BorderRadius.Any(r => r.Value > 0);
            bool hasRightRadius = hasRadius && (style.BorderTopRightRadius?.Value > 0 || style.BorderBottomRightRadius?.Value > 0);
            bool hasBottomRadius = hasRadius && (style.BorderBottomLeftRadius?.Value > 0 || style.BorderBottomRightRadius?.Value > 0);

            if (hasRadius)
                graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Draw the background color.

            Rectangle backgroundRect = bounds;

            // If the rectangle has right or bottom corner radii, the bounds must be decreased to ensure the curve is not clipped.

            if (hasRadius)
                backgroundRect = new Rectangle(bounds.X, bounds.Y, bounds.Width - (hasRightRadius ? 1 : 0), bounds.Height - (hasBottomRadius ? 1 : 0));

            int topLeft = (int)(style.BorderTopLeftRadius?.Value ?? 0);
            int topRight = (int)(style.BorderTopRightRadius?.Value ?? 0);
            int bottomLeft = (int)(style.BorderBottomLeftRadius?.Value ?? 0);
            int bottomRight = (int)(style.BorderBottomRightRadius?.Value ?? 0);

            if (style.Contains(PropertyName.BackgroundColor)) {

                Color backgroundColor = style.BackgroundColor;

                if (style.Contains(PropertyName.Opacity))
                    backgroundColor = GetColorWithAlpha(backgroundColor, (float)style.Opacity);

                using (Brush brush = new SolidBrush(backgroundColor)) {

                    if (!hasRadius)
                        graphics.FillRectangle(brush, backgroundRect);
                    else
                        graphics.FillRoundedRectangle(brush, backgroundRect, topLeft, topRight, bottomLeft, bottomRight);
                }

            }

            // Draw the background image.

            //if (style.BackgroundImage.HasValue()) {

            //    ClipToBorder(graphics, backgroundRect, style);

            //    foreach (IImage image in style.BackgroundImage.Value.Images)
            //        graphics.DrawImage(image, backgroundRect);

            //}

            graphics.Restore(state);

        }

        public static void DrawBorder(Graphics graphics, Rectangle bounds, IRuleset style) {

            GraphicsState state = graphics.Save();

            bool hasRadius = style.BorderRadius.Any(r => r.Value > 0);

            if (hasRadius)
                graphics.SmoothingMode = SmoothingMode.AntiAlias;

            double topWidth = style.BorderTopWidth?.Value ?? 0;
            double rightWidth = style.BorderRightWidth?.Value ?? 0;
            double bottomWidth = style.BorderBottomWidth?.Value ?? 0;
            double leftWidth = style.BorderLeftWidth?.Value ?? 0;

            double topLeftRadius = style.BorderTopLeftRadius?.Value ?? 0;
            double topRightRadius = style.BorderTopRightRadius?.Value ?? 0;
            double bottomRightRadius = style.BorderBottomRightRadius?.Value ?? 0;
            double bottomLeftRadius = style.BorderBottomLeftRadius?.Value ?? 0;

            double horizontalBorderWidth = leftWidth + rightWidth;
            double verticalBorderWidth = topWidth + bottomWidth;

            int rectX = bounds.X + (int)(leftWidth / 2);
            int rectY = bounds.Y + (int)(topWidth / 2);
            int rectWidth = bounds.Width - (int)(horizontalBorderWidth / 2);
            int rectHeight = bounds.Height - (int)(verticalBorderWidth / 2);

            Rectangle drawRect = new Rectangle(rectX, rectY, rectWidth - (rightWidth == 1 && leftWidth <= 0 ? 1 : 0), rectHeight - (bottomWidth == 1 && topWidth <= 0 ? 1 : 0));

            float opacity = (float)(style.Opacity);

            using (Pen pen = new Pen(Color.Black)) {

                pen.Alignment = PenAlignment.Center;
                pen.StartCap = LineCap.Square;

                if (topWidth > 0) {

                    StyleSheets.Properties.BorderStyle borderStyle = style.BorderTopStyle;

                    if (borderStyle != StyleSheets.Properties.BorderStyle.None && borderStyle != StyleSheets.Properties.BorderStyle.Hidden) {

                        pen.Width = (float)topWidth;
                        pen.Color = GetColorWithAlpha(style.BorderTopColor, opacity);
                        pen.DashStyle = GetDashStyle(borderStyle);

                        using (GraphicsPath path = CreateBorderPath(drawRect, BorderPathType.Top, topLeftRadius, topRightRadius, bottomRightRadius, bottomLeftRadius))
                            graphics.DrawPath(pen, path);

                    }

                }

                if (rightWidth > 0) {

                    StyleSheets.Properties.BorderStyle borderStyle = style.BorderRightStyle;

                    if (borderStyle != StyleSheets.Properties.BorderStyle.None && borderStyle != StyleSheets.Properties.BorderStyle.Hidden) {

                        pen.Width = (float)rightWidth;
                        pen.Color = GetColorWithAlpha(style.BorderRightColor, opacity);
                        pen.DashStyle = GetDashStyle(borderStyle);

                        using (GraphicsPath path = CreateBorderPath(drawRect, BorderPathType.Right, topLeftRadius, topRightRadius, bottomRightRadius, bottomLeftRadius))
                            graphics.DrawPath(pen, path);

                    }

                }

                if (bottomWidth > 0) {

                    StyleSheets.Properties.BorderStyle borderStyle = style.BorderBottomStyle;

                    if (borderStyle != StyleSheets.Properties.BorderStyle.None && borderStyle != StyleSheets.Properties.BorderStyle.Hidden) {

                        pen.Width = (float)bottomWidth;
                        pen.Color = GetColorWithAlpha(style.BorderBottomColor, opacity);
                        pen.DashStyle = GetDashStyle(borderStyle);

                        using (GraphicsPath path = CreateBorderPath(drawRect, BorderPathType.Bottom, topLeftRadius, topRightRadius, bottomRightRadius, bottomLeftRadius))
                            graphics.DrawPath(pen, path);

                    }

                }

                if (leftWidth > 0) {

                    StyleSheets.Properties.BorderStyle borderStyle = style.BorderLeftStyle;

                    if (borderStyle != StyleSheets.Properties.BorderStyle.None && borderStyle != StyleSheets.Properties.BorderStyle.Hidden) {

                        pen.Width = (float)leftWidth;
                        pen.Color = GetColorWithAlpha(style.BorderLeftColor, opacity);
                        pen.DashStyle = GetDashStyle(borderStyle);

                        using (GraphicsPath path = CreateBorderPath(drawRect, BorderPathType.Left, topLeftRadius, topRightRadius, bottomRightRadius, bottomLeftRadius))
                            graphics.DrawPath(pen, path);

                    }

                }

            }

            graphics.Restore(state);

        }

        public static void DrawText(Graphics graphics, Rectangle bounds, string text, Font font, IRuleset style) {

            DrawText(graphics, bounds, text, font, TextFormatFlags.Left | TextFormatFlags.NoPrefix, style);

        }
        public static void DrawText(Graphics graphics, Rectangle bounds, string text, Font font, TextFormatFlags textFormatFlags, IRuleset style) {

            if (graphics is null)
                throw new ArgumentNullException(nameof(graphics));

            if (font is null)
                throw new ArgumentNullException(nameof(font));

            // Paint the foreground text.

            Color textColor = style.Color;

            TextRenderer.DrawText(graphics, text, font, bounds, textColor, textFormatFlags);

            //graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            //using (Brush brush = new SolidBrush(textColor))
            //    graphics.DrawString(text, font, brush, new RectangleF(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height));

        }

        // Private members

        private enum BorderPathType {
            Top,
            Right,
            Bottom,
            Left,
            Full
        }

        private static GraphicsPath CreateBorderPath(Rectangle bounds, BorderPathType type, double topLeftRadius, double topRightRadius, double bottomRightRadius, double bottomLeftRadius) {

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

        private static void DrawGradient(Graphics graphics, Rectangle bounds, ILinearGradient gradient) {

            if (graphics is null)
                throw new ArgumentNullException(nameof(graphics));

            if (gradient is null)
                throw new ArgumentNullException(nameof(gradient));

            int colorStopsCount = gradient.ColorStops.Count();

            if (colorStopsCount > 1) {

                // 90 degrees is Right, but Up for CSS.

                using (LinearGradientBrush brush = new LinearGradientBrush(bounds, Color.Black, Color.Black, (float)gradient.Direction.ToDegrees() - 90.0f)) {

                    ColorBlend colorBlend = new ColorBlend {
                        Colors = gradient.ColorStops.Select(stop => stop.Color).ToArray(),
                    };

                    colorBlend.Positions = Enumerable.Range(0, colorStopsCount)
                         .Select(i => (float)i / (colorStopsCount - 1))
                         .ToArray();

                    brush.InterpolationColors = colorBlend;

                    graphics.FillRectangle(brush, bounds);

                }

            }
            else if (colorStopsCount == 1) {

                // Draw a solid rectangle if there is only one color.

                using (SolidBrush brush = new SolidBrush(gradient.ColorStops.First().Color))
                    graphics.FillRectangle(brush, bounds);

            }

        }

    }

}