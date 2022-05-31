using System;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion {

    internal class ValueConverterFactory :
        IValueConverterFactory {

        // Public members

        public static ValueConverterFactory Default => new ValueConverterFactory();

        public ValueConverterFactory() {

            RegisterValueConverters();

        }

        public IValueConverter Create(Type sourceType, Type destinationType) {

            var key = CreateKey(sourceType, destinationType);

            if (converters.TryGetValue(key, out IValueConverter valueConverter))
                return valueConverter;

            return null;

        }

        // Private members

        private readonly IDictionary<Tuple<Type, Type>, IValueConverter> converters = new Dictionary<Tuple<Type, Type>, IValueConverter>();

        private void RegisterValueConverters() {

            RegisterValueConverter(new BorderStyleToStringConverter());
            RegisterValueConverter(new ColorToStringConverter());
            RegisterValueConverter(new StringToAngleConverter());
            RegisterValueConverter(new StringToBorderStyleConverter());
            RegisterValueConverter(new StringToColorConverter());
            RegisterValueConverter(new StringToDoubleConverter());
            RegisterValueConverter(new StringToLengthConverter());
            RegisterValueConverter(new StringToLineWidthConverter());
            RegisterValueConverter(new StringToPercentageConverter());

        }
        private void RegisterValueConverter(IValueConverter valueConverter) {

            if (valueConverter is null)
                throw new ArgumentNullException(nameof(valueConverter));

            converters.Add(CreateKey(valueConverter.SourceType, valueConverter.DestinationType), valueConverter);

        }
        private static Tuple<Type, Type> CreateKey(Type sourceType, Type destinationType) {

            return Tuple.Create(sourceType, destinationType);

        }

    }

}