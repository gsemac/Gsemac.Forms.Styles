using System;
using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public class ColorStop :
        IColorStop {

        // Public members

        public Color Color { get; }
        public Measurement StopPosition { get; }

        public ColorStop(Color color) {

            Color = color;

        }
        public ColorStop(Color color, Measurement stopPosition) {

            if (stopPosition is null)
                throw new ArgumentNullException(nameof(stopPosition));

            Color = color;
            StopPosition = stopPosition;

        }

    }

}