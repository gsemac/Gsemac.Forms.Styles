using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public sealed class BackgroundImage :
        IEnumerable<IImage> {

        // Public members

        public IEnumerable<IImage> Images { get; }

        public BackgroundImage() :
            this(Enumerable.Empty<IImage>().ToArray()) {
        }
        public BackgroundImage(IImage[] images) {

            if (images is null)
                throw new ArgumentNullException(nameof(images));

            Images = images;

        }

        public override string ToString() {

            return string.Join(", ", Images);

        }

        public IEnumerator<IImage> GetEnumerator() {

            return Images.GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

    }

}