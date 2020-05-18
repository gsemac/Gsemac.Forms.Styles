﻿using Gsemac.Forms.Styles.Extensions;
using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public abstract class StylesheetControlRendererBase :
        IStyleSheetControlRenderer {

        // Public members

        public abstract void RenderControl(Graphics graphics, Control control);

        public virtual IRuleset GetRuleset(Control control, bool inherit) {

            return GetRuleset(new ControlNode(control), inherit);

        }
        public IRuleset GetRuleset(INode node, bool inherit) {

            return StyleSheet.GetRuleset(node, inherit);

        }

        public void PaintBackground(Graphics graphics, Rectangle rectangle, IRuleset ruleset) {

            GraphicsState state = graphics.Save();

            bool hasRadius = ruleset.Where(p => p.IsBorderRadiusProperty()).OfType<NumberProperty>().Any(p => p.Value > 0);
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

            // Draw outline.

            PaintBorder(graphics, rectangle, ruleset);

            graphics.Restore(state);

        }

        public void PaintForeground(Graphics graphics, string text, Font font, Rectangle rectangle, IRuleset ruleset, TextFormatFlags textFormatFlags) {

            // Paint the foreground text.

            TextRenderer.DrawText(graphics, text, font, rectangle, ruleset.Color?.Value ?? SystemColors.ControlText, textFormatFlags);

        }

        // Protected members

        protected IStyleSheet StyleSheet { get; }

        protected StylesheetControlRendererBase(IStyleSheet styleSheet) {

            StyleSheet = styleSheet;

        }

        // Private members

        private void PaintBorder(Graphics graphics, Rectangle rectangle, IRuleset ruleset) {

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
                rectangle.Width - (int)(rightWidth / 2 + leftWidth / 2),
                rectangle.Height - (int)(bottomWidth / 2 + topWidth / 2));

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

        }

    }

    public abstract class StylesheetControlRendererBase<T> :
        StylesheetControlRendererBase,
        IControlRenderer<T> where T : Control {

        // Public members

        public override void RenderControl(Graphics graphics, Control control) {

            if (control is T castedControl)
                RenderControl(graphics, castedControl);
            else
                throw new ArgumentException("The given control cannot be rendered by this renderer.");

        }
        public abstract void RenderControl(Graphics graphics, T control);

        // Protected members

        protected StylesheetControlRendererBase(IStyleSheet styleSheet) :
            base(styleSheet) {
        }

    }

}