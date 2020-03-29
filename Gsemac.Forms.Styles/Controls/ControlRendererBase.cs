﻿using Gsemac.Forms.Styles.Extensions;
using Gsemac.Forms.Styles.StyleSheets;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Controls {

    public abstract class ControlRendererBase {

        // Public members

        public IRuleset GetRuleset(Control control) {

            return GetRuleset(new ControlNode(control));

        }

        // Protected members

        protected ControlRendererBase(IStyleSheet styleSheet) {

            this.styleSheet = styleSheet;

        }

        protected IRuleset GetRuleset(INode node) {

            return styleSheet.GetRuleset(node);

        }

        protected void PaintBackground(Graphics graphics, Control control) {

            IRuleset rules = GetRuleset(control);

            ColorProperty parentBackgroundColor = null;

            if (control.Parent != null)
                parentBackgroundColor = GetRuleset(control.Parent).GetProperty(PropertyType.BackgroundColor) as ColorProperty;

            graphics.Clear(parentBackgroundColor?.Value ?? control.Parent?.BackColor ?? Color.Transparent);

            PaintBackground(graphics, control.ClientRectangle, rules);

        }
        protected void PaintBackground(Graphics graphics, Rectangle rectangle, IRuleset rules) {

            GraphicsState state = graphics.Save();

            ColorProperty backgroundColor = rules.GetProperty(PropertyType.BackgroundColor) as ColorProperty;

            ColorProperty borderColor = rules.GetProperty(PropertyType.BorderColor) as ColorProperty;
            NumericProperty borderRadius = rules.GetProperty(PropertyType.BorderRadius) as NumericProperty;
            NumericProperty borderWidth = rules.GetProperty(PropertyType.BorderWidth) as NumericProperty;

            if (borderRadius != null && borderRadius.Value > 0)
                graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Paint the background color.

            using (Brush brush = new SolidBrush(backgroundColor?.Value ?? SystemColors.Control)) {

                if (borderRadius is null || borderRadius.Value <= 0)
                    graphics.FillRectangle(brush, rectangle);
                else
                    graphics.FillRoundedRectangle(brush, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1), (int)borderRadius.Value);

            }

            // Draw outline.

            if (borderWidth != null && borderWidth.Value > 0) {

                using (Brush brush = new SolidBrush(borderColor?.Value ?? Color.Black))
                using (Pen pen = new Pen(brush, (float)borderWidth.Value)) {

                    pen.Alignment = PenAlignment.Center;

                    if (borderRadius is null || borderRadius.Value <= 0)
                        graphics.DrawRectangle(pen, rectangle);
                    else
                        graphics.DrawRoundedRectangle(pen, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1), (int)borderRadius.Value);

                }

            }

            graphics.Restore(state);

        }
        protected void PaintForeground(Graphics graphics, Control control, TextFormatFlags textFormatFlags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter) {

            IRuleset rules = GetRuleset(control);

            ColorProperty color = rules.GetProperty(PropertyType.Color) as ColorProperty;

            // Paint the foreground text.

            TextRenderer.DrawText(graphics, control.Text, control.Font, control.ClientRectangle, color?.Value ?? SystemColors.ControlText, textFormatFlags);

        }
        protected TextFormatFlags GetTextFormatFlags(ContentAlignment contentAlignment) {

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

        // Private members

        private readonly IStyleSheet styleSheet;

    }

}