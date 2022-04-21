using Gsemac.Forms.Styles.StyleSheets.Dom;
using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.Extensions {

    public static class MeasurementExtensions {

        // Public members

        public static double ToDegrees(this IMeasurement measurement) {

            return MeasurementUtilities.ToDegrees(measurement);

        }

        public static int ToPixels(this IMeasurement measurement, INode2 node, Rectangle viewport) {

            return MeasurementUtilities.ToPixels(measurement, node, viewport);

        }
        public static int ToPixels(this IMeasurement measurement, INode2 node, Rectangle viewport, int dpi) {

            return MeasurementUtilities.ToPixels(measurement, node, viewport, dpi);

        }

    }

}