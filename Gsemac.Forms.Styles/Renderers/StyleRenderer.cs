using Gsemac.Forms.Styles.Extensions;
using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public class StyleRenderer :
        IStyleRenderer {

        // Public members

        public void PaintBackground(Graphics graphics, Rectangle rect, IRuleset ruleset) {

            GraphicsState state = graphics.Save();

            bool hasRadius = ruleset.GetBorderRadii().Any(r => r > 0);
            bool hasRightRadius = hasRadius && (ruleset.BorderTopRightRadius?.Value > 0 || ruleset.BorderBottomRightRadius?.Value > 0);
            bool hasBottomRadius = hasRadius && (ruleset.BorderBottomLeftRadius?.Value > 0 || ruleset.BorderBottomRightRadius?.Value > 0);

            if (hasRadius)
                graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Draw the background color.

            Rectangle backgroundRect = rect;

            // If the rectangle has right or bottom corner radii, the bounds must be decreased to ensure the curve is not clipped.

            if (hasRadius)
                backgroundRect = new Rectangle(rect.X, rect.Y, rect.Width - (hasRightRadius ? 1 : 0), rect.Height - (hasBottomRadius ? 1 : 0));

            int topLeft = (int)(ruleset.BorderTopLeftRadius?.Value ?? 0);
            int topRight = (int)(ruleset.BorderTopRightRadius?.Value ?? 0);
            int bottomLeft = (int)(ruleset.BorderBottomLeftRadius?.Value ?? 0);
            int bottomRight = (int)(ruleset.BorderBottomRightRadius?.Value ?? 0);

            if (ruleset.BackgroundColor.HasValue()) {

                Color backgroundColor = ruleset.BackgroundColor?.Value ?? SystemColors.Control;

                if (ruleset.Opacity.HasValue())
                    backgroundColor = RenderUtilities.GetColorWithAlpha(backgroundColor, (float)ruleset.Opacity.Value);

                using (Brush brush = new SolidBrush(backgroundColor)) {

                    if (!hasRadius)
                        graphics.FillRectangle(brush, backgroundRect);
                    else
                        graphics.FillRoundedRectangle(brush, backgroundRect, topLeft, topRight, bottomLeft, bottomRight);
                }

            }

            // Draw the background image.

            if (ruleset.BackgroundImage.HasValue()) {

                ClipToBorder(graphics, backgroundRect, ruleset);

                foreach (IImage image in ruleset.BackgroundImage.Value.Images)
                    graphics.DrawImage(image, backgroundRect);

            }

            graphics.Restore(state);

        }
        public void PaintBorder(Graphics graphics, Rectangle rectangle, IRuleset ruleset) {

            GraphicsState state = graphics.Save();

            bool hasRadius = ruleset.GetBorderRadii().Any(r => r > 0);

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

            float opacity = (float)(ruleset.Opacity?.Value ?? 1.0f);

            using (Pen pen = new Pen(Color.Black)) {

                pen.Alignment = PenAlignment.Center;
                pen.StartCap = LineCap.Square;

                if (topWidth > 0) {

                    StyleSheets.BorderStyle borderStyle = ruleset.BorderTopStyle?.Value ?? StyleSheets.BorderStyle.Solid;

                    if (borderStyle != StyleSheets.BorderStyle.None && borderStyle != StyleSheets.BorderStyle.Hidden) {

                        pen.Width = (float)topWidth;
                        pen.Color = RenderUtilities.GetColorWithAlpha(ruleset.BorderTopColor?.Value ?? default, opacity);
                        pen.DashStyle = RenderUtilities.GetDashStyle(borderStyle);

                        using (GraphicsPath path = RenderUtilities.CreateBorderPath(drawRect, BorderPathType.Top, topLeftRadius, topRightRadius, bottomRightRadius, bottomLeftRadius))
                            graphics.DrawPath(pen, path);

                    }

                }

                if (rightWidth > 0) {

                    StyleSheets.BorderStyle borderStyle = ruleset.BorderRightStyle?.Value ?? StyleSheets.BorderStyle.Solid;

                    if (borderStyle != StyleSheets.BorderStyle.None && borderStyle != StyleSheets.BorderStyle.Hidden) {

                        pen.Width = (float)rightWidth;
                        pen.Color = RenderUtilities.GetColorWithAlpha(ruleset.BorderRightColor?.Value ?? default, opacity);
                        pen.DashStyle = RenderUtilities.GetDashStyle(borderStyle);

                        using (GraphicsPath path = RenderUtilities.CreateBorderPath(drawRect, BorderPathType.Right, topLeftRadius, topRightRadius, bottomRightRadius, bottomLeftRadius))
                            graphics.DrawPath(pen, path);

                    }

                }

                if (bottomWidth > 0) {

                    StyleSheets.BorderStyle borderStyle = ruleset.BorderBottomStyle?.Value ?? StyleSheets.BorderStyle.Solid;

                    if (borderStyle != StyleSheets.BorderStyle.None && borderStyle != StyleSheets.BorderStyle.Hidden) {

                        pen.Width = (float)bottomWidth;
                        pen.Color = RenderUtilities.GetColorWithAlpha(ruleset.BorderBottomColor?.Value ?? default, opacity);
                        pen.DashStyle = RenderUtilities.GetDashStyle(borderStyle);

                        using (GraphicsPath path = RenderUtilities.CreateBorderPath(drawRect, BorderPathType.Bottom, topLeftRadius, topRightRadius, bottomRightRadius, bottomLeftRadius))
                            graphics.DrawPath(pen, path);

                    }

                }

                if (leftWidth > 0) {

                    StyleSheets.BorderStyle borderStyle = ruleset.BorderLeftStyle?.Value ?? StyleSheets.BorderStyle.Solid;

                    if (borderStyle != StyleSheets.BorderStyle.None && borderStyle != StyleSheets.BorderStyle.Hidden) {

                        pen.Width = (float)leftWidth;
                        pen.Color = RenderUtilities.GetColorWithAlpha(ruleset.BorderLeftColor?.Value ?? default, opacity);
                        pen.DashStyle = RenderUtilities.GetDashStyle(borderStyle);

                        using (GraphicsPath path = RenderUtilities.CreateBorderPath(drawRect, BorderPathType.Left, topLeftRadius, topRightRadius, bottomRightRadius, bottomLeftRadius))
                            graphics.DrawPath(pen, path);

                    }

                }

            }

            graphics.Restore(state);

        }
        public void PaintText(Graphics graphics, Rectangle rectangle, IRuleset ruleset, string text, Font font, TextFormatFlags textFormatFlags = TextFormatFlags.Left) {

            // Paint the foreground text.

            Color textColor = ruleset.Color?.Value ?? SystemColors.ControlText;

            TextRenderer.DrawText(graphics, text, font, rectangle, textColor, textFormatFlags);

            //graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            //using (Brush brush = new SolidBrush(textColor))
            //    graphics.DrawString(text, font, brush, new RectangleF(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height));

        }

        public void ClipToBorder(Graphics graphics, Rectangle rectangle, IRuleset ruleset) {

            Region region = new Region(rectangle);

            if (ruleset.Any(p => p.IsBorderRadiusProperty())) {

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

    }

}