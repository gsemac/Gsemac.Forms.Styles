using Gsemac.Forms.Styles.StyleSheets.Dom;
using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.Extensions {

    public static class DimensionExtensions {

        // Public members

        public static double ToDegrees(this IDimension measurement) {

            return DimensionUtilities.ToDegrees(measurement);

        }

        public static int ToPixels(this IDimension measurement, INode2 node, Rectangle viewport) {

            return DimensionUtilities.ToPixels(measurement, node, viewport);

        }
        public static int ToPixels(this IDimension measurement, INode2 node, Rectangle viewport, int dpi) {

            return DimensionUtilities.ToPixels(measurement, node, viewport, dpi);

        }

    }

}