using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Controls {

    public class CheckBoxControlRenderer :
        ControlRendererBase<CheckBox> {

        // Public members

        public CheckBoxControlRenderer(IStyleSheet styleSheet) :
            base(styleSheet) {
        }

        public override void RenderControl(Graphics graphics, CheckBox control) {

            PaintBackground(graphics, control);

            PaintCheck(graphics, control);

            IRuleset ruleset = GetRuleset(control);

            Rectangle clientRect = control.ClientRectangle;
            Rectangle drawRect = new Rectangle(clientRect.X + CheckWidth + 3, clientRect.Y - 1, clientRect.Width, clientRect.Height);

            PaintForeground(graphics, control.Text, control.Font, drawRect, ruleset, GetTextFormatFlags(control.TextAlign));

        }

        // Private members

        private const int CheckWidth = 13;

        private void PaintCheck(Graphics graphics, CheckBox control) {

            IRuleset parentRuleset = GetRuleset(control);
            IRuleset ruleset = GetRuleset(new Node(string.Empty, "Check", parent: new ControlNode(control), states: new ControlNode(control).States));

            if (!ruleset.Any())
                ruleset = CreateDefaultCheckRuleset();

            ruleset.InheritProperties(parentRuleset);

            Rectangle clientRect = control.ClientRectangle;
            Rectangle checkRect = new Rectangle(clientRect.X, clientRect.Y + (int)((clientRect.Height / 2.0f) - (CheckWidth / 2.0f)) - 1, CheckWidth, CheckWidth);

            PaintBackground(graphics, checkRect, ruleset);

            // Draw the checkmark.

            if (control.Checked) {

                using (Brush brush = new SolidBrush(ruleset.Color?.Value ?? SystemColors.ControlText))
                using (Pen pen = new Pen(brush)) {

                    graphics.SmoothingMode = SmoothingMode.AntiAlias;

                    pen.Alignment = PenAlignment.Center;
                    pen.Width = 2.0f;
                    pen.StartCap = LineCap.Square;
                    pen.EndCap = LineCap.Square;

                    graphics.DrawLine(pen, checkRect.X + 3, checkRect.Y + checkRect.Height / 2.0f, checkRect.X + checkRect.Width / 2.0f - 1, checkRect.Y + checkRect.Height - 5);
                    graphics.DrawLine(pen, checkRect.X + checkRect.Width / 2.0f - 1, checkRect.Y + checkRect.Height - 5, checkRect.X + checkRect.Width - 4, checkRect.Y + 3);

                }

            }

        }

        private IRuleset CreateDefaultCheckRuleset() {

            IRuleset ruleset = new Ruleset();

            ruleset.AddProperty(Property.Create(PropertyType.BackgroundColor, "white"));
            ruleset.AddProperty(Property.Create(PropertyType.BorderColor, "#707070"));
            ruleset.AddProperty(Property.Create(PropertyType.BorderWidth, "1px"));
            ruleset.AddProperty(Property.Create(PropertyType.Color, "black"));

            return ruleset;

        }

    }

}