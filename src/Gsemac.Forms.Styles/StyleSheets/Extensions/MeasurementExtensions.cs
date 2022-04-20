using Gsemac.Forms.Styles.Dom;
using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets.Extensions {

    public static class MeasurementExtensions {

        // Public members

        public static int ToPixels(this IMeasurement measurement, INode2 node, Rectangle viewport, int dpi) {

            return Measurement.ToPixels(measurement, node, viewport, dpi);

        }
        public static double ToDegrees(this IMeasurement measurement) {

            return Measurement.ToDegrees(measurement);

        }

    }

}