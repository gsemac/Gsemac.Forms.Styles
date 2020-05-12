using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Controls {

    public class RadioButtonControlRenderer :
    ControlRendererBase<RadioButton> {

        // Public members

        public RadioButtonControlRenderer(IStyleSheet styleSheet) :
            base(styleSheet) {
        }

        public override void RenderControl(Graphics graphics, RadioButton control) {

            PaintBackground(graphics, control);

            PaintCheck(graphics, control);

            IRuleset ruleset = GetRuleset(control);

            Rectangle clientRect = control.ClientRectangle;
            Rectangle drawRect = new Rectangle(clientRect.X + CheckWidth + 3, clientRect.Y, clientRect.Width, clientRect.Height);

            PaintForeground(graphics, control.Text, control.Font, drawRect, ruleset, GetTextFormatFlags(control.TextAlign));

        }

        // Private members

        private const int CheckWidth = 13;

        private void PaintCheck(Graphics graphics, RadioButton control) {

            IRuleset parentRuleset = GetRuleset(control);
            IRuleset ruleset = GetRuleset(new Node(string.Empty, "Check", parent: new ControlNode(control), states: new ControlNode(control).States));

            if (!ruleset.Any())
                ruleset = CreateDefaultCheckRuleset();

            ruleset.InheritProperties(parentRuleset);

            Rectangle clientRect = control.ClientRectangle;
            Rectangle checkRect = new Rectangle(clientRect.X, clientRect.Y + (int)((clientRect.Height / 2.0f) - (CheckWidth / 2.0f)), CheckWidth, CheckWidth);

            PaintBackground(graphics, checkRect, ruleset);

            // Draw the checkmark.

            if (control.Checked) {

                using (Brush brush = new SolidBrush(ruleset.Color?.Value ?? SystemColors.ControlText))
                using (Pen pen = new Pen(brush)) {

                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    checkRect.Inflate(-3, -3);

                    graphics.FillEllipse(brush, checkRect);

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