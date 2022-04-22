using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public sealed class BackgroundImage {

        // Public members

        public IEnumerable<IImage> Images { get; }

        public BackgroundImage() :
            this(Enumerable.Empty<IImage>()) {
        }
        public BackgroundImage(IEnumerable<IImage> images) {

            if (images is null)
                throw new ArgumentNullException(nameof(images));

            Images = images.ToArray();

        }

        public override string ToString() {

            return string.Join(", ", Images);

        }

    }

}