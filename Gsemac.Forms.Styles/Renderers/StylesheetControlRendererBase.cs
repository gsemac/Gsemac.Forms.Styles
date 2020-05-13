using Gsemac.Forms.Styles.Extensions;
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

            BorderRadiusProperty borderRadius = ruleset.BorderRadius;
            bool hasRightRadius = ruleset.BorderRadius?.Value.TopRight > 0 || ruleset.BorderRadius?.Value.BottomRight > 0;
            bool hasBottomRadius = ruleset.BorderRadius?.Value.BottomLeft > 0 || ruleset.BorderRadius?.Value.BottomRight > 0;

            if (borderRadius.HasValue() && borderRadius.Value.IsGreaterThanZero())
                graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Paint the background color.

            if (ruleset.BackgroundColor.HasValue()) {

                using (Brush brush = new SolidBrush(ruleset.BackgroundColor?.Value ?? SystemColors.Control)) {

                    if (!borderRadius.HasValue() || !borderRadius.Value.IsGreaterThanZero())
                        graphics.FillRectangle(brush, rectangle);
                    else {

                        // If the rectangle has right or bottom corner radii, the bounds must be decreased to ensure the curve is not clipped.

                        graphics.FillRoundedRectangle(brush, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width - (hasRightRadius ? 1 : 0), rectangle.Height - (hasBottomRadius ? 1 : 0)), borderRadius.Value);

                    }

                }

            }

            // Draw outline.

            if (ruleset.BorderWidth.HasValue()) {

                using (Brush brush = new SolidBrush(ruleset.BorderColor?.Value ?? Color.Black))
                using (Pen pen = new Pen(brush, (float)ruleset.BorderWidth.Value)) {

                    pen.Alignment = PenAlignment.Center;

                    if (!borderRadius.HasValue() || !borderRadius.Value.IsGreaterThanZero())
                        graphics.DrawRectangle(pen, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1));
                    else
                        graphics.DrawRoundedRectangle(pen, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1), borderRadius.Value);

                }

            }

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