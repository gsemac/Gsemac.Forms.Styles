using Gsemac.Data.ValueConversion;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion {

    internal class StyleValueConverterFactory :
        ValueConverterFactoryBase {

        // Public members

        public static StyleValueConverterFactory Default { get; } = new StyleValueConverterFactory();

        public StyleValueConverterFactory() {

            AddDefaultValueConverters();

        }

        // Private members

        private void AddDefaultValueConverters() {

            AddValueConverter(new BorderStyleToStringConverter());
            AddValueConverter(new ColorToStringConverter());
            AddValueConverter(new StringToAngleConverter());
            AddValueConverter(new StringToBorderStyleConverter());
            AddValueConverter(new StringToColorConverter());
            AddValueConverter(new StringToLengthConverter());
            AddValueConverter(new StringToLineWidthConverter());
            AddValueConverter(new StringToPercentageConverter());

        }

    }

}