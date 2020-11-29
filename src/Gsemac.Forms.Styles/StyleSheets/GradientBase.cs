using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets {

    public abstract class GradientBase :
        IImage,
        IGradient {

        // Public members

        public abstract void DrawGradient(Graphics graphics, Rectangle rect);
        public void DrawImage(Graphics graphics, Rectangle rect) {

            DrawGradient(graphics, rect);

        }

        public void Dispose() {

            Dispose(true);

        }

        // Protected members

        protected virtual void Dispose(bool disposing) { }

    }

}
