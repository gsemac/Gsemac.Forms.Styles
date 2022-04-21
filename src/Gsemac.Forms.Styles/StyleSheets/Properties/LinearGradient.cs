using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public class LinearGradient :
        ILinearGradient {

        // Public members

        public IMeasurement Direction { get; }
        public IEnumerable<IColorStop> ColorStops { get; }

        public LinearGradient(IMeasurement direction, IEnumerable<IColorStop> colorStops) {

            if (direction is null)
                throw new ArgumentNullException(nameof(direction));

            if (colorStops is null)
                throw new ArgumentNullException(nameof(colorStops));

            Direction = direction;
            ColorStops = colorStops.ToArray();

        }
        public LinearGradient(double degrees, IEnumerable<Color> colorStops) :
            this(Measurement.FromDegrees(degrees), colorStops.Select(color => new ColorStop(color))) {
        }

    }

}