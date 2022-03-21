using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class Image :
        IImage {

        // Public members

        public static Image Empty => new Image();

        public Image(System.Drawing.Image image) {

            this.image = image;

        }

        public void DrawImage(Graphics graphics, Rectangle rect) {

            if (image != null)
                graphics.DrawImage(image, rect.Location);

        }

        public void Dispose() {

            Dispose(true);

        }

        // Protected members

        protected virtual void Dispose(bool disposing) {

            if (disposing) {

                if (image != null)
                    image.Dispose();

                image = null;

            }

        }

        // Private memberss

        private System.Drawing.Image image;

        private Image() {

            image = null;

        }

    }

}