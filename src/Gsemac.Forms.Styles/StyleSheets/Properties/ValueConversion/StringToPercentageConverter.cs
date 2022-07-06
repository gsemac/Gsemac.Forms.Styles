namespace Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion {

    internal class StringToPercentageConverter :
        ValueConverterBase<string, Percentage> {

        // Public members

        public override Percentage Convert(string value) {

            var baseConverter = new StringToDimensionConverter(Units.GetPercentageUnits());

            IDimension dimension = baseConverter.Convert(value);

            return new Percentage(dimension.Value);

        }

    }

}