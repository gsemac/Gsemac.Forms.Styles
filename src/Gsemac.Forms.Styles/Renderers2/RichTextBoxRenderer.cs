using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers2 {

    internal class RichTextBoxRenderer :
        StyleRendererBase<RichTextBox> {

        // Public members

        public override void Render(RichTextBox richTextBox, IRenderContext context) {

            if (richTextBox is null)
                throw new ArgumentNullException(nameof(richTextBox));

            if (context is null)
                throw new ArgumentNullException(nameof(context));

            context.Clear();

            Rectangle clientRect = context.ClientRectangle;

            context.DrawBackground(clientRect);

            context.DrawBorder(clientRect);

        }

    }

}