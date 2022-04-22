using Gsemac.Forms.Styles.Properties;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public sealed class Angle :
        DimensionBase {

        // Public members

        public static Angle Zero => new Angle(0, string.Empty);

        public Angle(double value) :
            this(value, AngleUnit.Degree) {
        }
        public Angle(double value, string unit) :
            base(value, unit) {
        }

        public static Angle FromDegrees(double value) {

            return new Angle(value, AngleUnit.Degree);

        }

        public static Angle Parse(string input) {

            if (TryParse(input, out Angle result))
                return result;

            throw new FormatException(string.Format(ExceptionMessages.MalformedDimensionValue, input));


        }
        public static bool TryParse(string input, out Angle result) {

            result = null;

            if (string.IsNullOrWhiteSpace(input))
                return false;

            input = input.Trim().ToLowerInvariant();

            if (TryParseDirectionalAngle(input, out double parsedDirectionalAngle)) {

                result = FromDegrees(parsedDirectionalAngle);

                return true;

            }

            string measurementsPattern = string.Join("|", GetValidUnitsInternal());

            // Whitespace is permitted between the number and the units.

            string parsePattern = $@"^(?<value>\d+(?:\.\d+)?)\s*(?<unit>{measurementsPattern}|)$";

            Match m = Regex.Match(input, parsePattern);

            if (m.Success && double.TryParse(m.Groups["value"].Value, out double value)) {

                string unit = m.Groups["unit"].Value;

                if (string.IsNullOrWhiteSpace(unit))
                    unit = AngleUnit.Degree;

                result = new Angle(value, unit);

                return true;

            }

            return false;

        }

        // Protected members

        protected override IEnumerable<string> GetValidUnits() {

            return GetValidUnitsInternal();

        }

        // Private members

        private static bool TryParseDirectionalAngle(string input, out double degrees) {

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

        private static IEnumerable<string> GetValidUnitsInternal() {

            return new[] {
                AngleUnit.Degree,
                AngleUnit.Gradian,
                AngleUnit.Radian,
                AngleUnit.Turn,
            };

        }

    }

}