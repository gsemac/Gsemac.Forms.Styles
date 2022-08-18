using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers2 {

    internal class ButtonRenderer :
        StyleRendererBase<Button> {

        // Public members

        public override void Render(Button control, IRenderContext context) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            if (context is null)
                throw new ArgumentNullException(nameof(context));

            context.Clear();

            Rectangle clientRect = context.ClientRectangle;

            clientRect.Inflate(-1, -1);

            context.DrawBackground(clientRect);

            if (control.Image is object) {

                const int horizontalPadding = 4;
                const int verticalPadding = 4;

                Rectangle imageRect = new Rectangle(horizontalPadding, verticalPadding, control.Width - horizontalPadding * 2, control.Height - verticalPadding * 2);

                context.DrawImage(control.Image, imageRect, control.ImageAlign);

            }

            context.DrawText(control.Text, control.Font, ControlUtilities.GetTextFormatFlags(control.TextAlign));

            if (ControlUtilities2.FocusCuesShown(control)) {

                Rectangle focusRectangle = new Rectangle(clientRect.X + 2, clientRect.Y + 2, clientRect.Width - 5, clientRect.Height - 5);

                ControlRenderUtilities.DrawFocusRectangle(context.Graphics, focusRectangle, context.Style);

            }

            context.DrawBorder(clientRect);

        }

    }

}