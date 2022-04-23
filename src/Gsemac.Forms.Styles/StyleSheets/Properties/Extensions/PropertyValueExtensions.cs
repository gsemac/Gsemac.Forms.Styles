using Gsemac.Forms.Styles.Properties;
using Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion;
using System;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.Extensions {

    public static class PropertyValueExtensions {

        // Public members

        public static T As<T>(this IPropertyValue propertyValue) {

            if (propertyValue is null)
                throw new ArgumentNullException(nameof(propertyValue));

            if (typeof(T) == propertyValue.Type) {

                // If the type is an exact match, we can simply cast the value directly.

                return (T)propertyValue.Value;

            }
            else {

                IValueConverter valueConverter = ValueConverterFactory.Default.Create(propertyValue.Type, typeof(T));

                if (valueConverter is object) {

                    return (T)valueConverter.Convert(propertyValue.Value);

                }

            }

            throw new InvalidCastException(string.Format(ExceptionMessages.CannotCastPropertyOfTypeToType, propertyValue.Type, typeof(T)));

        }

        public static bool Is<T>(this IPropertyValue propertyValue) {

            if (propertyValue is null)
                throw new ArgumentNullException(nameof(propertyValue));

            return propertyValue.Type.Equals(typeof(T));

        }

    }

}