using Gsemac.Data.ValueConversion;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion {

    internal class StringToAngleConverter :
        ValueConverterBase<string, Angle> {

        // Public members

        public override bool TryConvert(string value, out Angle result) {

            result = default;

            var baseConverter = new StringToDimensionConverter(Units.GetAngleUnits());

            if (baseConverter.TryConvert(value, out IDimension dimension)) {

                double dimensionValue = dimension.Value;
                string dimensionUnit = dimension.Unit;

                result = new Angle(dimensionValue, dimensionUnit);

                return true;

            }

            return false;

        }

    }

}