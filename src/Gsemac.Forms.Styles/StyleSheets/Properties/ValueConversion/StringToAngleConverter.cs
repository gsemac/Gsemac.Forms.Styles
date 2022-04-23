namespace Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion {

    internal class StringToAngleConverter :
        ValueConverterBase<string, Angle> {

        // Public members

        public override Angle Convert(string value) {

            var baseConverter = new StringToDimensionConverter(AngleUnit.GetUnits());

            IDimension dimension = baseConverter.Convert(value);

            double dimensionValue = dimension.Value;
            string dimensionUnit = dimension.Unit;

            return new Angle(dimensionValue, dimensionUnit);

        }

    }

}