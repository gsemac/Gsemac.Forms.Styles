using System;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers2 {

    internal class LabelRenderer :
        StyleRendererBase<Label> {

        // Public members

        public override void Render(Label control, IRenderContext context) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            if (context is null)
                throw new ArgumentNullException(nameof(context));

            context.DrawBackground();

            context.DrawText(control.Text, control.Font, ControlUtilities.GetTextFormatFlags(control.TextAlign) | TextFormatFlags.WordBreak);

            context.DrawBorder();

        }

    }

}