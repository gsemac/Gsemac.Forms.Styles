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

        public override IDimension Convert(string value) {

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(string.Format(ExceptionMessages.MalformedPropertyValueAsType, value, DestinationType), nameof(value));

            string measurementsPattern = string.Join("|", validUnits);

            // Whitespace is permitted between the number and the units.

            string parsePattern = $@"^(?<value>\d+(?:\.\d+)?)\s*(?<unit>{measurementsPattern}|)$";

            Match m = Regex.Match(value, parsePattern);

            if (m.Success && double.TryParse(m.Groups["value"].Value, out double parsedValue)) {

                string unit = m.Groups["unit"].Value;

                return new ParsedDimension(parsedValue, unit);

            }

            throw new ArgumentException(string.Format(ExceptionMessages.MalformedPropertyValueAsType, value, DestinationType), nameof(value));

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