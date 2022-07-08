using Gsemac.Data.ValueConversion;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion {

    internal class StringToPercentageConverter :
        ValueConverterBase<string, Percentage> {

        // Public members

        public override bool TryConvert(string value, out Percentage result) {

            result = default;

            var baseConverter = new StringToDimensionConverter(Units.GetPercentageUnits());

            if (baseConverter.TryConvert(value, out IDimension dimension)) {

                result = new Percentage(dimension.Value);

                return true;

            }

            return false;

        }

    }

}