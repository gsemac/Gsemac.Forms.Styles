using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public class LinearGradient :
        ILinearGradient {

        // Public members

        public Angle Direction { get; }
        public IEnumerable<ColorStop> ColorStops { get; }

        public LinearGradient(Angle direction, IEnumerable<ColorStop> colorStops) {

            if (direction is null)
                throw new ArgumentNullException(nameof(direction));

            if (colorStops is null)
                throw new ArgumentNullException(nameof(colorStops));

            Direction = direction;
            ColorStops = colorStops.ToArray();

        }
        public LinearGradient(double degrees, IEnumerable<Color> colorStops) :
            this(new Angle(degrees, Units.Degree), colorStops.Select(color => new ColorStop(color))) {
        }

    }

}