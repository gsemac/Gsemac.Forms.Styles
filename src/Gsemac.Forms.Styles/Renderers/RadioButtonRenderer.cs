using Gsemac.Forms.Styles.StyleSheets.Extensions;
using Gsemac.Forms.Styles.StyleSheets.Properties;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public class RadioButtonRenderer :
    ControlRendererBase<RadioButton> {

        // Public members

        public override void PaintControl(RadioButton control, ControlPaintArgs e) {

            e.PaintBackground();
            e.PaintBorder();

            PaintCheck(control, e);

            IRuleset ruleset = e.StyleSheet.GetRuleset(control);

            Rectangle clientRect = control.ClientRectangle;
            Rectangle textRect = new Rectangle(clientRect.X + CheckWidth + 3, clientRect.Y, clientRect.Width, clientRect.Height);
            TextFormatFlags textFormatFlags = ControlUtilities.GetTextFormatFlags(control.TextAlign);

            e.StyleRenderer.PaintText(e.Graphics, textRect, ruleset, control.Text, control.Font, textFormatFlags);

        }

        // Private members

        private const int CheckWidth = 13;

        private void PaintCheck(RadioButton control, ControlPaintArgs e) {

            //INode controlNode = new ControlNode(control);
            //UserNode checkNode = new UserNode(string.Empty, new[] { "Check" });

            //checkNode.SetParent(controlNode);
            //checkNode.SetStates(controlNode.States);

            //IRuleset parentRuleset = e.StyleSheet.GetRuleset(control);
            //IRuleset ruleset = e.StyleSheet.GetRuleset(checkNode, inherit: false);

            //if (!ruleset.Any())
            //    ruleset = CreateDefaultCheckRuleset();

            //ruleset.InheritPropertiesFrom(parentRuleset);

            //Rectangle clientRect = control.ClientRectangle;
            //Rectangle checkRect = new Rectangle(clientRect.X, clientRect.Y + (int)(clientRect.Height / 2.0f - CheckWidth / 2.0f), CheckWidth, CheckWidth);

            //e.StyleRenderer.PaintBackground(e.Graphics, checkRect, ruleset);
            //e.StyleRenderer.PaintBorder(e.Graphics, checkRect, ruleset);

            //// Draw the checkmark.

            //if (control.Checked) {

            //    using (Brush brush = new SolidBrush(ruleset.Color?.Value ?? SystemColors.ControlText)) {

            //        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            //        e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            //        checkRect.Inflate(-3, -3);

            //        e.Graphics.FillEllipse(brush, checkRect);

            //    }

            //}

        }

        private IRuleset CreateDefaultCheckRuleset() {

            IRuleset ruleset = new Ruleset {
                PropertyFactory.Default.Create(PropertyName.BackgroundColor, PropertyValue.Create("white")),
                PropertyFactory.Default.Create(PropertyName.BorderColor, PropertyValue.Create("#707070")),
                PropertyFactory.Default.Create(PropertyName.BorderWidth, PropertyValue.Create("1px")),
                PropertyFactory.Default.Create(PropertyName.Color, PropertyValue.Create("black"))
            };

            return ruleset;

        }

    }

}