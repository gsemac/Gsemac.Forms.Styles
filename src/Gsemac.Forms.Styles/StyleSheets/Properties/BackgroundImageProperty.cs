using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class BackgroundImage {

        // Public members

        public IEnumerable<IImage> Images { get; }

        public BackgroundImage(IImage[] images) {

            this.Images = images;

        }

    }

    public class BackgroundImageProperty :
        PropertyBase<BackgroundImage> {

        // Public members

        public BackgroundImageProperty(BackgroundImage value) :
            base(PropertyType.BackgroundImage, value, false) {
        }
        public BackgroundImageProperty(IImage[] images) :
            base(PropertyType.BackgroundImage, new BackgroundImage(images), false) {
        }

    }

}