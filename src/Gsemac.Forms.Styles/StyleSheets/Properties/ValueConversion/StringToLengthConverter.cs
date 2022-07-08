using Gsemac.Data.ValueConversion;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion {

    internal class StringToLengthConverter :
        ValueConverterBase<string, Length> {

        // Public members

        public override bool TryConvert(string value, out Length result) {

            result = default;

            var baseConverter = new StringToDimensionConverter(Units.GetLengthUnits());

            if (baseConverter.TryConvert(value, out IDimension dimension)) {

                double dimensionValue = dimension.Value;
                string dimensionUnit = dimension.Unit;

                if (dimensionValue > 0 && string.IsNullOrWhiteSpace(dimensionUnit))
                    dimensionUnit = Units.Pixel;

                result = new Length(dimensionValue, dimensionUnit);

                return true;

            }

            return false;

        }

    }

}