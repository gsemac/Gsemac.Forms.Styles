using Gsemac.Forms.Styles.Properties;
using Gsemac.Forms.Styles.StyleSheets.Dom;
using System;
using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public static class DimensionUtilities {

        // Public members

        public static double ToDegrees(IDimension measurement) {

            if (measurement is null)
                throw new ArgumentNullException(nameof(measurement));

            switch (measurement.Unit) {

                case AngleUnit.Degree:
                    return measurement.Value;

                case AngleUnit.Gradian:
                    return MathUtilities.GradiansToDegrees(measurement.Value);

                case AngleUnit.Radian:
                    return MathUtilities.RadiansToDegrees(measurement.Value);

                case AngleUnit.Turn:
                    return MathUtilities.TurnsToDegrees(measurement.Value);

                default:
                    throw new InvalidOperationException(string.Format(ExceptionMessages.UnitCannotBeConvertedToDegreesWithUnit, measurement.Unit));

            }

        }

        public static int ToPixels(IDimension measurement, INode2 node, Rectangle viewport) {

            return ToPixels(measurement, node, viewport, DefaultDpi);

        }
        public static int ToPixels(IDimension measurement, INode2 node, Rectangle viewport, int dpi) {

            if (measurement is null)
                throw new ArgumentNullException(nameof(measurement));

            switch (measurement.Unit) {

                case LengthUnit.Pixel:
                    return (int)measurement.Value;

                default:
                    throw new InvalidOperationException(string.Format(ExceptionMessages.UnitCannotBeConvertedToPixelsWithUnit, measurement.Unit));

            }

        }

        // Private members

        private const int DefaultDpi = 96; // https://stackoverflow.com/a/7619569/5383169 (ShadyKiller)

    }

}