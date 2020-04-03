using Gsemac.Forms.Styles.Extensions;
using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Controls {

    public abstract class ControlRendererBase :
        IControlRenderer {

        // Public members

        public abstract void RenderControl(Graphics graphics, Control control);

        // Protected members

        protected IStyleSheet StyleSheet { get; }

        protected ControlRendererBase(IStyleSheet styleSheet) {

            this.StyleSheet = styleSheet;

        }

        protected IRuleset GetRuleset(Control control, bool inherit = true) {

            return GetRuleset(new ControlNode(control), inherit);

        }
        protected IRuleset GetRuleset(INode node, bool inherit = true) {

            return StyleSheet.GetRuleset(node, inherit);

        }
        protected IRuleset GetRuleset(Control parent, INode node) {

            IRuleset ruleset = GetRuleset(parent);

            return GetRuleset(ruleset, node);

        }
        protected IRuleset GetRuleset(IRuleset parentRuleset, INode node) {

            IRuleset ruleset = new Ruleset();

            ruleset.InheritProperties(parentRuleset);
            ruleset.AddProperties(GetRuleset(node));

            return ruleset;

        }

        protected static bool MouseIntersectsWith(Control control) {

            return MouseIntersectsWith(control, control.ClientRectangle);

        }
        protected static bool MouseIntersectsWith(Control control, Rectangle rect) {

            Point mousePos = control.PointToClient(Cursor.Position);
            Rectangle mouseRect = new Rectangle(mousePos.X, mousePos.Y, 1, 1);

            return rect.IntersectsWith(mouseRect);

        }

        protected void PaintBackground(Graphics graphics, Control control) {

            IRuleset rules = GetRuleset(control);

            ColorProperty parentBackgroundColor = null;

            if (control.Parent != null)
                parentBackgroundColor = GetRuleset(control.Parent).GetProperty(PropertyType.BackgroundColor) as ColorProperty;

            graphics.Clear(parentBackgroundColor?.Value ?? control.Parent?.BackColor ?? Color.Transparent);

            PaintBackground(graphics, control.ClientRectangle, rules);

        }
        protected static void PaintBackground(Graphics graphics, Rectangle rectangle, IRuleset rules) {

            GraphicsState state = graphics.Save();

            ColorProperty borderColor = rules.GetProperty(PropertyType.BorderColor) as ColorProperty;
            NumericProperty borderRadius = rules.GetProperty(PropertyType.BorderRadius) as NumericProperty;

            if (borderRadius != null && borderRadius.Value > 0)
                graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Paint the background color.

            if (rules.GetProperty(PropertyType.BackgroundColor) is ColorProperty backgroundColor) {

                using (Brush brush = new SolidBrush(backgroundColor?.Value ?? SystemColors.Control)) {

                    if (borderRadius is null || borderRadius.Value <= 0)
                        graphics.FillRectangle(brush, rectangle);
                    else
                        graphics.FillRoundedRectangle(brush, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1), (int)borderRadius.Value);

                }

            }

            // Draw outline.

            if (rules.GetProperty(PropertyType.BorderWidth) is NumericProperty borderWidth && borderWidth.Value > 0) {

                using (Brush brush = new SolidBrush(borderColor?.Value ?? Color.Black))
                using (Pen pen = new Pen(brush, (float)borderWidth.Value)) {

                    pen.Alignment = PenAlignment.Center;

                    if (borderRadius is null || borderRadius.Value <= 0)
                        graphics.DrawRectangle(pen, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1));
                    else
                        graphics.DrawRoundedRectangle(pen, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1), (int)borderRadius.Value);

                }

            }

            graphics.Restore(state);

        }

        protected void PaintForeground(Graphics graphics, Control control, TextFormatFlags textFormatFlags = DefaultTextFormatFlags) {

            IRuleset rules = GetRuleset(control);

            ColorProperty color = rules.GetProperty(PropertyType.Color) as ColorProperty;

            // Paint the foreground text.

            TextRenderer.DrawText(graphics, control.Text, control.Font, control.ClientRectangle, color?.Value ?? SystemColors.ControlText, textFormatFlags);

        }
        protected static void PaintForeground(Graphics graphics, string text, Font font, Rectangle rectangle, IRuleset rules, TextFormatFlags textFormatFlags = DefaultTextFormatFlags) {

            ColorProperty color = rules.GetProperty(PropertyType.Color) as ColorProperty;

            // Paint the foreground text.

            TextRenderer.DrawText(graphics, text, font, rectangle, color?.Value ?? SystemColors.ControlText, textFormatFlags);

        }

        protected static TextFormatFlags GetTextFormatFlags(ContentAlignment contentAlignment) {

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
        protected static void SetColorProperties(Control control, IRuleset ruleset) {

            if (ruleset.GetProperty(PropertyType.BackgroundColor) is ColorProperty backgroundColor)
                control.BackColor = backgroundColor.Value;

            if (ruleset.GetProperty(PropertyType.Color) is ColorProperty color)
                control.ForeColor = color.Value;

        }

        protected static void ClipToRectangle(Graphics graphics, Rectangle clientRetangle, IRuleset ruleset) {

            if (ruleset.GetProperty(PropertyType.BorderRadius) is NumericProperty borderRadius && borderRadius.Value > 0.0)
                graphics.SetClip(GraphicsExtensions.CreateRoundedRectangle(clientRetangle, (int)borderRadius.Value));
            else
                graphics.SetClip(clientRetangle);

        }

        // Private members

        private const TextFormatFlags DefaultTextFormatFlags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter;

    }

    public abstract class ControlRendererBase<T> :
        ControlRendererBase,
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

        protected ControlRendererBase(IStyleSheet styleSheet) :
            base(styleSheet) {
        }

    }

}