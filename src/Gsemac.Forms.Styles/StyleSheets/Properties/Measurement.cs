using Gsemac.Forms.Styles.Properties;
using System;
using System.Text.RegularExpressions;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public class Measurement :
        IMeasurement {

        // Public members

        public string Unit { get; }
        public double Value { get; }

        public const string Centimeters = "cm";
        public const string Inches = "in";
        public const string Millimeters = "mm";
        public const string Picas = "pc";
        public const string Pixels = "px";
        public const string Points = "pt";

        public const string Em = "em";
        public const string Percent = "%";
        public const string RootElement = "rem";
        public const string ViewportHeight = "vh";
        public const string ViewportMaximum = "vmax";
        public const string ViewportMinimum = "vmin";
        public const string ViewportWidth = "vw";
        public const string XHeight = "ex";
        public const string ZeroWidth = "ch";

        public const string Degrees = "deg";
        public const string Gradians = "grad";
        public const string Radians = "rad";
        public const string Turns = "turn";

        public Measurement(double value) :
            this(value, Pixels) {
        }
        public Measurement(double value, string unit) {

            if (unit is null)
                throw new ArgumentNullException(nameof(unit));

            if (string.IsNullOrWhiteSpace(unit))
                throw new ArgumentException(string.Format(ExceptionMessages.UnrecognizedUnits, unit), nameof(unit));

            Value = value;
            Unit = unit.Trim().ToLowerInvariant();

        }

        public override string ToString() {

            return $"{Value}{Unit}";

        }

        public static Measurement FromDegrees(double value) {

            return new Measurement(value, Degrees);

        }
        public static Measurement FromPixels(double value) {

            return new Measurement(value, Pixels);

        }

        public static Measurement Parse(string input) {

            if (TryParse(input, out Measurement measurement))
                return measurement;

            throw new FormatException(string.Format(ExceptionMessages.MalformedMeasurementValue, input));


        }
        public static bool TryParse(string input, out Measurement measurement) {

            measurement = null;

            if (string.IsNullOrWhiteSpace(input))
                return false;

            if (TryParseDirectionalAngle(input, out double parsedDirectionalAngle)) {

                measurement = FromDegrees(parsedDirectionalAngle);

                return true;

            }

            string measurementsPattern = string.Join("|", new string[] {
                Centimeters,
                Inches,
                Millimeters,
                Picas,
                Pixels,
                Points,
                Em,
                Percent,
                RootElement,
                ViewportHeight,
                ViewportMaximum,
                ViewportMinimum,
                ViewportWidth,
                XHeight,
                ZeroWidth,
                Degrees,
                Gradians,
                Radians,
                Turns,
            });

            // Whitespace is permitted between the number and the units.

            string parsePattern = $@"^(?<value>\d+(?:\.\d+))\s*(?<unit>{measurementsPattern}|)$";

            if (string.IsNullOrWhiteSpace(input)) {

                Match m = Regex.Match(input, parsePattern);

                if (m.Success && double.TryParse(m.Groups["value"].Value, out double value)) {

                    string unit = m.Groups["unit"].Value;

                    // Use pixels as a fallback unit if the units are not specified.
                    // Note that only non-zero values require units to be specified.
                    // https://stackoverflow.com/a/11275156/5383169 (BoltClock)

                    if (string.IsNullOrWhiteSpace(unit))
                        unit = Pixels;

                    measurement = new Measurement(value, unit);

                    return true;

                }

            }

            return false;

        }

        // Private members

        public static bool TryParseDirectionalAngle(string input, out double degrees) {

            degrees = 0;

            if (string.IsNullOrWhiteSpace(input))
                return false;

            switch (input.Trim().ToLowerInvariant()) {

                case "bottom right":
                    degrees = 135.0;
                    return true;

                case "bottom left":
                    degrees = 225.0;
                    return true;

                case "top right":
                    degrees = 45.0;
                    return true;

                case "top left":
                    degrees = 315.0;
                    return true;

                case "bottom":
                    degrees = 180.0;
                    return true;

                case "top":
                    degrees = 0.0;
                    return true;

                case "left":
                    degrees = 270.0;
                    return true;

                case "right":
                    degrees = 90.0;
                    return true;

                default:
                    return false;

            }

        }

    }

}