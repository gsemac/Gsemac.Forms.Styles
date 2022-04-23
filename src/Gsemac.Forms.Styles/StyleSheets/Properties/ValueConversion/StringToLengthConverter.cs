namespace Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion {

    internal class StringToLengthConverter :
        ValueConverterBase<string, Length> {

        // Public members

        public override Length Convert(string value) {

            var baseConverter = new StringToDimensionConverter(LengthUnit.GetUnits());

            IDimension dimension = baseConverter.Convert(value);

            double dimensionValue = dimension.Value;
            string dimensionUnit = dimension.Unit;

            if (dimensionValue > 0 && string.IsNullOrWhiteSpace(dimensionUnit))
                dimensionUnit = LengthUnit.Pixel;

            return new Length(dimensionValue, dimensionUnit);

        }

    }

}