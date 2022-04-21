using Gsemac.Forms.Styles.Properties;
using Gsemac.Forms.Styles.StyleSheets.Dom;
using System;
using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public static class MeasurementUtilities {

        // Public members

        public static double ToDegrees(IMeasurement measurement) {

            if (measurement is null)
                throw new ArgumentNullException(nameof(measurement));

            switch (measurement.Unit) {

                case Measurement.Degrees:
                    return measurement.Value;

                case Measurement.Gradians:
                    return MathUtilities.GradiansToDegrees(measurement.Value);

                case Measurement.Radians:
                    return MathUtilities.RadiansToDegrees(measurement.Value);

                case Measurement.Turns:
                    return MathUtilities.TurnsToDegrees(measurement.Value);

                default:
                    throw new InvalidOperationException(string.Format(ExceptionMessages.UnitCannotBeConvertedToDegreesWithUnit, measurement.Unit));

            }

        }

        public static int ToPixels(IMeasurement measurement, INode2 node, Rectangle viewport) {

            return ToPixels(measurement, node, viewport, DefaultDpi);

        }
        public static int ToPixels(IMeasurement measurement, INode2 node, Rectangle viewport, int dpi) {

            if (measurement is null)
                throw new ArgumentNullException(nameof(measurement));

            switch (measurement.Unit) {

                case Measurement.Pixels:
                    return (int)measurement.Value;

                default:
                    throw new InvalidOperationException(string.Format(ExceptionMessages.UnitCannotBeConvertedToPixelsWithUnit, measurement.Unit));

            }

        }

        // Private members

        private const int DefaultDpi = 96; // https://stackoverflow.com/a/7619569/5383169 (ShadyKiller)

    }

}