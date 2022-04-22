using Gsemac.Forms.Styles.Properties;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public sealed class Length :
        DimensionBase {

        // Public members

        public static Length Zero => new Length(0, string.Empty);

        public Length(double value) :
            this(value, LengthUnit.Pixel) {
        }
        public Length(double value, string unit) :
            base(value, unit) {
        }

        public static Length FromPixels(double value) {

            return new Length(value, LengthUnit.Pixel);

        }

        public static Length Parse(string input) {

            if (TryParse(input, out Length result))
                return result;

            throw new FormatException(string.Format(ExceptionMessages.MalformedDimensionValue, input));

        }
        public static bool TryParse(string input, out Length result) {

            result = null;

            if (string.IsNullOrWhiteSpace(input))
                return false;

            input = input.Trim().ToLowerInvariant();

            string measurementsPattern = string.Join("|", GetValidUnitsInternal());

            // Whitespace is permitted between the number and the units.

            string parsePattern = $@"^(?<value>\d+(?:\.\d+)?)\s*(?<unit>{measurementsPattern}|)$";

            Match m = Regex.Match(input, parsePattern);

            if (m.Success && double.TryParse(m.Groups["value"].Value, out double value)) {

                string unit = m.Groups["unit"].Value;

                // Use pixels as a fallback unit if the units are not specified.
                // Note that only non-zero values require units to be specified.
                // https://stackoverflow.com/a/11275156/5383169 (BoltClock)

                if (string.IsNullOrWhiteSpace(unit))
                    unit = LengthUnit.Pixel;

                result = new Length(value, unit);

                return true;

            }

            return false;

        }

        // Protected members

        protected override IEnumerable<string> GetValidUnits() {

            return GetValidUnitsInternal();

        }

        // Private members

        private static IEnumerable<string> GetValidUnitsInternal() {

            return new[] {
                LengthUnit.Centimeter,
                LengthUnit.Inch,
                LengthUnit.Millimeter,
                LengthUnit.Pica,
                LengthUnit.Pixel,
                LengthUnit.Point,
                LengthUnit.Em,
                LengthUnit.Percent,
                LengthUnit.RootElement,
                LengthUnit.ViewportHeight,
                LengthUnit.ViewportMaximum,
                LengthUnit.ViewportMinimum,
                LengthUnit.ViewportWidth,
                LengthUnit.XHeight,
                LengthUnit.ZeroWidth,
            };

        }

    }

}