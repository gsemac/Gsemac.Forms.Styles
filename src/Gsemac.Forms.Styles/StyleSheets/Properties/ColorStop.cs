using System;
using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public sealed class ColorStop {

        // Public members

        public Color Color { get; }
        public Length StopPosition { get; }

        public ColorStop(Color color) {

            Color = color;

        }
        public ColorStop(Color color, Length stopPosition) {

            if (stopPosition is null)
                throw new ArgumentNullException(nameof(stopPosition));

            Color = color;
            StopPosition = stopPosition;

        }

    }

}