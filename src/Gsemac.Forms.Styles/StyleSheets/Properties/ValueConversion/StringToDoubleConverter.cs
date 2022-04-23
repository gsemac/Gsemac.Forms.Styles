using System.Globalization;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion {

    internal class StringToDoubleConverter :
        ValueConverterBase<string, double> {

        // Public members

        public override double Convert(string value) {

            return double.Parse(value, CultureInfo.InvariantCulture);

        }

    }

}