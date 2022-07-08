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

            if (propertyValue is null)
                throw new ArgumentNullException(nameof(propertyValue));

            if (type is null)
                throw new ArgumentNullException(nameof(type));

            if (type == propertyValue.Type) {

                // If the type is an exact match, we can simply cast the value directly.

                return PropertyValue.Create(type, propertyValue.Value);

            }
            else {

                IValueConverter valueConverter = ValueConverterFactory.Default.Create(propertyValue.Type, type);

                if (valueConverter is object) {

                    return PropertyValue.Create(type, valueConverter.Convert(propertyValue.Value));

                }

            }

            throw new InvalidCastException(string.Format(ExceptionMessages.CannotCastPropertyOfTypeToType, propertyValue.Type, type));

        }

        public static bool Is<T>(this IPropertyValue propertyValue) {

            if (propertyValue is null)
                throw new ArgumentNullException(nameof(propertyValue));

            return propertyValue.Type.Equals(typeof(T));

        }

    }

}