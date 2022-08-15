using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers2 {

    public class TextBoxRenderer :
        StyleRendererBase<TextBox> {

        // Public members

        public override void Render(TextBox textBox, IRenderContext context) {

            if (textBox is null)
                throw new ArgumentNullException(nameof(textBox));

            if (context is null)
                throw new ArgumentNullException(nameof(context));

            context.Clear();

            Rectangle clientRect = context.ClientRectangle;

            clientRect.Inflate(-1, -1);

            context.DrawBackground(clientRect);

            context.DrawBorder(clientRect);

        }

    }

}