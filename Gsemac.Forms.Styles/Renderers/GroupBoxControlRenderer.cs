using Gsemac.Forms.Styles.Extensions;
using Gsemac.Forms.Styles.StyleSheets;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public class GroupBoxControlRenderer :
         ControlRendererBase<GroupBox> {

        // Public members

        public override void PaintControl(GroupBox control, ControlPaintArgs e) {

            IRuleset ruleset = e.StyleSheet.GetRuleset(control);

            IRuleset textBackgroundRuleset = new Ruleset();
            textBackgroundRuleset.AddProperty(ruleset.BackgroundColor);

            SizeF textSize = e.Graphics.MeasureString(control.Text, control.Font);

            Rectangle clientRect = control.ClientRectangle;
            Rectangle backgroundRect = new Rectangle(clientRect.X, clientRect.Y + (int)textSize.Height / 2, clientRect.Width, clientRect.Height - (int)textSize.Height / 2);
            Rectangle textRect = new Rectangle(clientRect.X + 6, clientRect.Y, (int)textSize.Width, (int)textSize.Height);
            Rectangle textBackgroundRect = new Rectangle(textRect.X, backgroundRect.Y, textRect.Width, textRect.Height - (int)textSize.Height / 2);

            e.Clear();

            e.StyleRenderer.PaintBackground(e.Graphics, backgroundRect, ruleset);
            e.StyleRenderer.PaintBorder(e.Graphics, backgroundRect, ruleset);

            e.StyleRenderer.PaintBackground(e.Graphics, textBackgroundRect, textBackgroundRuleset);
            e.StyleRenderer.PaintBorder(e.Graphics, textBackgroundRect, textBackgroundRuleset);

            e.StyleRenderer.PaintText(e.Graphics, textRect, ruleset, control.Text, control.Font, TextFormatFlags.Top);

        }

    }

}