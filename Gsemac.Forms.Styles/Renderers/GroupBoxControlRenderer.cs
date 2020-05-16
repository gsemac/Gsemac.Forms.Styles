using Gsemac.Forms.Styles.Extensions;
using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public class GroupBoxControlRenderer :
         ControlRendererBase<GroupBox> {

        // Public members

        public GroupBoxControlRenderer(IStyleSheetControlRenderer baseRenderer) {

            this.baseRenderer = baseRenderer;

        }

        public override void RenderControl(Graphics graphics, GroupBox control) {

            IRuleset ruleset = baseRenderer.GetRuleset(control);

            IRuleset textBackgroundRuleset = new Ruleset();
            textBackgroundRuleset.AddProperty(ruleset.BackgroundColor);

            SizeF textSize = graphics.MeasureString(control.Text, control.Font);

            Rectangle clientRect = control.ClientRectangle;
            Rectangle backgroundRect = new Rectangle(clientRect.X, clientRect.Y + (int)textSize.Height / 2, clientRect.Width, clientRect.Height - (int)textSize.Height / 2);
            Rectangle textRect = new Rectangle(clientRect.X + 6, clientRect.Y, (int)textSize.Width, (int)textSize.Height);
            Rectangle textBackgroundRect = new Rectangle(textRect.X, backgroundRect.Y, textRect.Width, textRect.Height - (int)textSize.Height / 2);

            baseRenderer.ClearBackground(graphics, control);

            baseRenderer.PaintBackground(graphics, backgroundRect, ruleset);
            baseRenderer.PaintBackground(graphics, textBackgroundRect, textBackgroundRuleset);
            baseRenderer.PaintForeground(graphics, control.Text, control.Font, textRect, ruleset, TextFormatFlags.Top);

        }

        // Private members

        private readonly IStyleSheetControlRenderer baseRenderer;

    }

}
