using Gsemac.Forms.Styles.Dom;
using Gsemac.Forms.Styles.Properties;
using System;
using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class Measurement :
        IMeasurement {

        // Public members

        public string Unit { get; }
        public double Value { get; }

        public const string Pixels = "px";

        public const string Degrees = "deg";
        public const string Gradians = "grad";
        public const string Radians = "rad";
        public const string Turns = "turn";

        public Measurement(double value, string unit) {

            if (unit is null)
                throw new ArgumentNullException(nameof(unit));

            if (string.IsNullOrWhiteSpace(unit))
                throw new ArgumentException(string.Format(ExceptionMessages.UnrecognizedUnitsWithUnit, unit), nameof(unit));

            Value = value;
            Unit = unit.Trim().ToLowerInvariant();

        }

        public override string ToString() {

            return $"{Value}{Unit}";

        }

        public static Measurement FromDegrees(double value) {

            return new Measurement(value, Degrees);

        }

        public static int ToPixels(IMeasurement measurement, INode2 node, Rectangle viewport, int dpi) {

            if (measurement is null)
                throw new ArgumentNullException(nameof(measurement));

            switch (measurement.Unit) {

                case Pixels:
                    return (int)measurement.Value;

                default:
                    throw new InvalidOperationException(string.Format(ExceptionMessages.UnitCannotBeConvertedToPixelsWithUnit, measurement.Unit));

            }

        }
        public static double ToDegrees(IMeasurement measurement) {

            if (measurement is null)
                throw new ArgumentNullException(nameof(measurement));

            switch (measurement.Unit) {

                case Degrees:
                    return measurement.Value;

                case Gradians:
                    return MathUtilities.GradiansToDegrees(measurement.Value);

                case Radians:
                    return MathUtilities.RadiansToDegrees(measurement.Value);

                case Turns:
                    return MathUtilities.TurnsToDegrees(measurement.Value);

                default:
                    throw new InvalidOperationException(string.Format(ExceptionMessages.UnitCannotBeConvertedToDegreesWithUnit, measurement.Unit));

            }

        }

    }

}