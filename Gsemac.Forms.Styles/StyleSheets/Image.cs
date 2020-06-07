using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class Image :
        IImage {

        // Public members

        public Image(System.Drawing.Image image) {

            this.image = image;

        }

        public void DrawImage(Graphics graphics, Rectangle rect) {

            graphics.DrawImage(image, rect.Location);

        }

        public void Dispose() {

            Dispose(true);

        }

        // Protected members

        protected virtual void Dispose(bool disposing) {

            if (disposing) {

                image.Dispose();

            }

        }

        // Private memberss

        private readonly System.Drawing.Image image;

    }

}