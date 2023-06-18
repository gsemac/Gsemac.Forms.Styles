using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers2 {

    internal class NumericUpDownRenderer :
        StyleRendererBase<NumericUpDown> {

        // Public members

        public override void Render(NumericUpDown numericUpDown, IRenderContext context) {

            if (numericUpDown is null)
                throw new ArgumentNullException(nameof(numericUpDown));

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