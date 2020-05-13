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

    public class RadioButtonControlRenderer :
    ControlRendererBase<RadioButton> {

        // Public members

        public RadioButtonControlRenderer(IStyleSheetControlRenderer baseRenderer) {

            this.baseRenderer = baseRenderer;

        }

        public override void RenderControl(Graphics graphics, RadioButton control) {

            baseRenderer.PaintBackground(graphics, control);

            PaintCheck(graphics, control);

            IRuleset ruleset = baseRenderer.GetRuleset(control);

            Rectangle clientRect = control.ClientRectangle;
            Rectangle drawRect = new Rectangle(clientRect.X + CheckWidth + 3, clientRect.Y, clientRect.Width, clientRect.Height);

            baseRenderer.PaintForeground(graphics, control.Text, control.Font, drawRect, ruleset, StyleUtilities.GetTextFormatFlags(control.TextAlign));

        }

        // Private members

        private readonly IStyleSheetControlRenderer baseRenderer;
        private const int CheckWidth = 13;

        private void PaintCheck(Graphics graphics, RadioButton control) {

            IRuleset parentRuleset = baseRenderer.GetRuleset(control);
            IRuleset ruleset = baseRenderer.GetRuleset(new UserNode(string.Empty, "Check", parent: new ControlNode(control), states: new ControlNode(control).States));

            if (!ruleset.Any())
                ruleset = CreateDefaultCheckRuleset();

            ruleset.InheritProperties(parentRuleset);

            Rectangle clientRect = control.ClientRectangle;
            Rectangle checkRect = new Rectangle(clientRect.X, clientRect.Y + (int)(clientRect.Height / 2.0f - CheckWidth / 2.0f), CheckWidth, CheckWidth);

            baseRenderer.PaintBackground(graphics, checkRect, ruleset);

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