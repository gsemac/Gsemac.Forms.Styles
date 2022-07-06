namespace Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion {

    internal class StringToLengthConverter :
        ValueConverterBase<string, Length> {

        // Public members

        public override Length Convert(string value) {

            var baseConverter = new StringToDimensionConverter(Units.GetLengthUnits());

            IDimension dimension = baseConverter.Convert(value);

            double dimensionValue = dimension.Value;
            string dimensionUnit = dimension.Unit;

            if (dimensionValue > 0 && string.IsNullOrWhiteSpace(dimensionUnit))
                dimensionUnit = Units.Pixel;

            return new Length(dimensionValue, dimensionUnit);

        }

    }

}