using Gsemac.Data.ValueConversion;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion {

    internal class StyleValueConverterFactory :
        ValueConverterFactoryBase {

        // Public members

        public StyleValueConverterFactory() :
            base(CreateOptions()) {

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

        private static IValueConverterFactoryOptions CreateOptions() {

            return new ValueConverterFactoryOptions() {
                EnableDefaultConverters = true,
                EnableDerivedClassLookup = true,
            };

        }

    }

}