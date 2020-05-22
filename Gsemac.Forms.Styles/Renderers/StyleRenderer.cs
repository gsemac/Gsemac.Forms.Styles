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

        public void PaintBackground(Graphics graphics, Rectangle rectangle, IRuleset ruleset) {

            GraphicsState state = graphics.Save();

            bool hasRadius = ruleset.GetBorderRadii().Any(r => r > 0);
            bool hasRightRadius = hasRadius && (ruleset.BorderTopRightRadius?.Value > 0 || ruleset.BorderBottomRightRadius?.Value > 0);
            bool hasBottomRadius = hasRadius && (ruleset.BorderBottomLeftRadius?.Value > 0 || ruleset.BorderBottomRightRadius?.Value > 0);

            if (hasRadius)
                graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Paint the background color.

            if (ruleset.BackgroundColor.HasValue()) {

                using (Brush brush = new SolidBrush(ruleset.BackgroundColor?.Value ?? SystemColors.Control)) {

                    if (!hasRadius)
                        graphics.FillRectangle(brush, rectangle);
                    else {

                        // If the rectangle has right or bottom corner radii, the bounds must be decreased to ensure the curve is not clipped.

                        graphics.FillRoundedRectangle(brush, new Rectangle(
                            rectangle.X,
                            rectangle.Y,
                            rectangle.Width - (hasRightRadius ? 1 : 0),
                            rectangle.Height - (hasBottomRadius ? 1 : 0)),
                            (int)(ruleset.BorderTopLeftRadius?.Value ?? 0),
                            (int)(ruleset.BorderTopRightRadius?.Value ?? 0),
                            (int)(ruleset.BorderBottomLeftRadius?.Value ?? 0),
                            (int)(ruleset.BorderBottomRightRadius?.Value ?? 0));

                    }

                }

            }

            graphics.Restore(state);

        }
        public void PaintBorder(Graphics graphics, Rectangle rectangle, IRuleset ruleset) {

            GraphicsState state = graphics.Save();

            bool hasRadius = ruleset.GetBorderRadii().Any(r => r > 0);

            if(hasRadius)
                graphics.SmoothingMode = SmoothingMode.AntiAlias;

            double topWidth = ruleset.BorderTopWidth?.Value ?? 0;
            double rightWidth = ruleset.BorderRightWidth?.Value ?? 0;
            double bottomWidth = ruleset.BorderBottomWidth?.Value ?? 0;
            double leftWidth = ruleset.BorderLeftWidth?.Value ?? 0;

            double topLeftRadius = ruleset.BorderTopLeftRadius?.Value ?? 0;
            double topRightRadius = ruleset.BorderTopRightRadius?.Value ?? 0;
            double bottomRightRadius = ruleset.BorderBottomRightRadius?.Value ?? 0;
            double bottomLeftRadius = ruleset.BorderBottomLeftRadius?.Value ?? 0;

            Rectangle drawRect = new Rectangle(
                rectangle.X + (int)(leftWidth / 2),
                rectangle.Y + (int)(topWidth / 2),
                rectangle.Width - ((int)leftWidth / 2 + (int)(rightWidth / 2)),
                rectangle.Height - ((int)(topWidth / 2) + (int)(bottomWidth / 2)));

            drawRect = new Rectangle(drawRect.X, drawRect.Y, drawRect.Width - 1, drawRect.Height - 1);

            using (Pen pen = new Pen(Color.Black)) {

                pen.Alignment = PenAlignment.Center;
                pen.StartCap = LineCap.Square;

                if (topWidth > 0) {

                    StyleSheets.BorderStyle borderStyle = ruleset.BorderTopStyle?.Value ?? StyleSheets.BorderStyle.Solid;

                    if (borderStyle != StyleSheets.BorderStyle.None && borderStyle != StyleSheets.BorderStyle.Hidden) {

                        pen.Width = (float)topWidth;
                        pen.Color = ruleset.BorderTopColor?.Value ?? default;
                        pen.DashStyle = RenderUtilities.GetDashStyle(borderStyle);

                        using (GraphicsPath path = RenderUtilities.CreateBorderPath(drawRect, BorderPathType.Top, topLeftRadius, topRightRadius, bottomRightRadius, bottomLeftRadius))
                            graphics.DrawPath(pen, path);

                    }

                }

                if (rightWidth > 0) {

                    StyleSheets.BorderStyle borderStyle = ruleset.BorderRightStyle?.Value ?? StyleSheets.BorderStyle.Solid;

                    if (borderStyle != StyleSheets.BorderStyle.None && borderStyle != StyleSheets.BorderStyle.Hidden) {

                        pen.Width = (float)rightWidth;
                        pen.Color = ruleset.BorderRightColor?.Value ?? default;
                        pen.DashStyle = RenderUtilities.GetDashStyle(borderStyle);

                        using (GraphicsPath path = RenderUtilities.CreateBorderPath(drawRect, BorderPathType.Right, topLeftRadius, topRightRadius, bottomRightRadius, bottomLeftRadius))
                            graphics.DrawPath(pen, path);

                    }

                }

                if (bottomWidth > 0) {

                    StyleSheets.BorderStyle borderStyle = ruleset.BorderBottomStyle?.Value ?? StyleSheets.BorderStyle.Solid;

                    if (borderStyle != StyleSheets.BorderStyle.None && borderStyle != StyleSheets.BorderStyle.Hidden) {

                        pen.Width = (float)bottomWidth;
                        pen.Color = ruleset.BorderBottomColor?.Value ?? default;
                        pen.DashStyle = RenderUtilities.GetDashStyle(borderStyle);

                        using (GraphicsPath path = RenderUtilities.CreateBorderPath(drawRect, BorderPathType.Bottom, topLeftRadius, topRightRadius, bottomRightRadius, bottomLeftRadius))
                            graphics.DrawPath(pen, path);

                    }

                }

                if (leftWidth > 0) {

                    StyleSheets.BorderStyle borderStyle = ruleset.BorderLeftStyle?.Value ?? StyleSheets.BorderStyle.Solid;

                    if (borderStyle != StyleSheets.BorderStyle.None && borderStyle != StyleSheets.BorderStyle.Hidden) {

                        pen.Width = (float)leftWidth;
                        pen.Color = ruleset.BorderLeftColor?.Value ?? default;
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

        // Private members



    }

}