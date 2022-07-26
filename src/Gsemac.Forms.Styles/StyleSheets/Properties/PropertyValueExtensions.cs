using Gsemac.Data.ValueConversion;
using Gsemac.Forms.Styles.Properties;
using Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion;
using System;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public static class PropertyValueExtensions {

        // Public members

        public static T As<T>(this IPropertyValue propertyValue) {

            if (propertyValue is null)
                throw new ArgumentNullException(nameof(propertyValue));

            return (T)propertyValue.As(typeof(T)).Value;

        }
        public static IPropertyValue As(this IPropertyValue propertyValue, Type type) {

            if (TryAs(propertyValue, type, out IPropertyValue result))
                return result;

            throw new InvalidCastException(string.Format(ExceptionMessages.CannotCastPropertyOfTypeToType, propertyValue.Type, type));

        }

        public static bool TryAs<T>(this IPropertyValue propertyValue, out T result) {

            result = default;

            if (TryAs(propertyValue, typeof(T), out IPropertyValue propertyValueResult)) {

                result = propertyValueResult.As<T>();

                return true;

            }

            return false;

        }
        public static bool TryAs(this IPropertyValue propertyValue, Type type, out IPropertyValue result) {

            if (propertyValue is null)
                throw new ArgumentNullException(nameof(propertyValue));

            if (type is null)
                throw new ArgumentNullException(nameof(type));

            result = default;

            bool success = false;

            if (type == propertyValue.Type) {

                // If the type is an exact match, we can simply cast the value directly.

                result = PropertyValue.Create(type, propertyValue.Value);
                success = true;

            }
            else {

                IValueConverter valueConverter = valueConverterFactory.Create(propertyValue.Type, type);

                if (valueConverter is object) {

                    result = PropertyValue.Create(type, valueConverter.Convert(propertyValue.Value));
                    success = true;

                }

            }

            return success;

        }

        public static bool Is<T>(this IPropertyValue propertyValue) {

            if (propertyValue is null)
                throw new ArgumentNullException(nameof(propertyValue));

            return propertyValue.Type.Equals(typeof(T));

        }

        // Private members

        private static readonly IValueConverterFactory valueConverterFactory = new StyleValueConverterFactory();

    }

}