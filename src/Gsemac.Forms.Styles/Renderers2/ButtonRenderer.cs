using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers2 {

    public class ButtonRenderer :
        StyleRendererBase<Button> {

        // Public members

        public override void Render(Button button, IRenderContext context) {

            if (button is null)
                throw new ArgumentNullException(nameof(button));

            if (context is null)
                throw new ArgumentNullException(nameof(context));

            context.Clear();

            context.DrawBackground();

            if (button.Image is object) {

                const int horizontalPadding = 4;
                const int verticalPadding = 4;

                Rectangle imageRect = new Rectangle(horizontalPadding, verticalPadding, button.Width - horizontalPadding * 2, button.Height - verticalPadding * 2);

                context.DrawImage(button.Image, imageRect, button.ImageAlign);

            }

            context.DrawText(button.Text, button.Font, ControlUtilities.GetTextFormatFlags(button.TextAlign));

            context.DrawBorder();

        }

    }

}