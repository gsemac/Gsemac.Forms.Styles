using Gsemac.Data.ValueConversion;
using Gsemac.Forms.Styles.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion {

    internal class StringToDimensionConverter :
        ValueConverterBase<string, IDimension> {

        // Public members

        public StringToDimensionConverter(IEnumerable<string> validUnits) {

            if (validUnits is null)
                throw new ArgumentNullException(nameof(validUnits));

            this.validUnits = validUnits.ToArray();

        }

        public override bool TryConvert(string value, out IDimension result) {

            result = default;

            if (value is null)
                return false;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            string measurementsPattern = string.Join("|", validUnits);

            // Whitespace is permitted between the number and the units.

            string parsePattern = $@"^(?<value>\d+(?:\.\d+)?)\s*(?<unit>{measurementsPattern}|)$";

            Match m = Regex.Match(value, parsePattern);

            if (m.Success && double.TryParse(m.Groups["value"].Value, out double parsedValue)) {

                string unit = m.Groups["unit"].Value;

                result = new ParsedDimension(parsedValue, unit);

            }

            return result is object;

        }

        // Private members

        private readonly IEnumerable<string> validUnits;

        private sealed class ParsedDimension :
            IDimension {

            // Public members

            public string Unit { get; }
            public double Value { get; }

            public ParsedDimension(double value, string unit) {

                Unit = unit;
                Value = value;

            }

        }

    }

}