using Gsemac.Forms.Styles.Renderers.Extensions;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers2 {

    internal class ProgressBarRenderer :
        StyleRendererBase<Control> {

        // Public members

        public override void Render(Control progressBar, IRenderContext context) {

            if (progressBar is null)
                throw new ArgumentNullException(nameof(progressBar));

            if (context is null)
                throw new ArgumentNullException(nameof(context));

            context.Clear();
            context.DrawBackground();

            if (progressBar is ProgressBar castedProgressBar)
                PaintProgress(castedProgressBar, context);

            context.DrawBorder();

        }

        // Private members

        private void PaintProgress(ProgressBar progressBar, IRenderContext context) {

            if (progressBar is null)
                throw new ArgumentNullException(nameof(progressBar));

            if (context is null)
                throw new ArgumentNullException(nameof(context));

            double progress = (double)progressBar.Value / progressBar.Maximum;

            Rectangle clientRect = progressBar.ClientRectangle;
            Rectangle progressRect = new Rectangle(clientRect.X, clientRect.Y, (int)Math.Floor(clientRect.Width * progress), clientRect.Height);
            Color progressColor = context.Style.AccentColor;

            using (Brush brush = new SolidBrush(progressColor))
                context.Graphics.FillRectangle(brush, progressRect);

        }

    }

}