using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public class LinearGradient :
        IGradient {

        // Public members

        public Angle Direction { get; }
        public IEnumerable<ColorStop> ColorStops { get; }

        public LinearGradient(Angle direction, ColorStop[] colorStops) {

            if (direction is null)
                throw new ArgumentNullException(nameof(direction));

            if (colorStops is null)
                throw new ArgumentNullException(nameof(colorStops));

            Direction = direction;
            ColorStops = colorStops.ToArray();

        }
        public LinearGradient(double degrees, Color[] colorStops) :
            this(new Angle(degrees, Units.Degree), colorStops.Select(color => new ColorStop(color)).ToArray()) {
        }

    }

}