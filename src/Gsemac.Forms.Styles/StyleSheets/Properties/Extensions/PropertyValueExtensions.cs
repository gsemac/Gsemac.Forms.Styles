using Gsemac.Forms.Styles.Properties;
using System;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.Extensions {

    public static class PropertyValueExtensions {

        // Public members

        public static T As<T>(this IPropertyValue propertyValue) {

            if (propertyValue is null)
                throw new ArgumentNullException(nameof(propertyValue));

            if (TryAs(propertyValue, out T value))
                return value;

            throw new InvalidCastException(string.Format(ExceptionMessages.CannotCastPropertyOfTypeToType, propertyValue.Type, typeof(T).Name));

        }
        public static bool TryAs<T>(this IPropertyValue propertyValue, out T value) {

            value = default;

            if (propertyValue is null)
                throw new ArgumentNullException(nameof(propertyValue));

            if (typeof(T) == propertyValue.Type) {

                // If the type is an exact match, we can simply cast the value directly.

                value = (T)propertyValue.Value;

                return true;

            }
            else if (propertyValue.Type.Equals(typeof(IDimension)) && TypeUtilities.IsNumericType(typeof(T))) {

                // Since all numbers are parsed as measurements, allow them to be casted into numeric types.

                value = (T)Convert.ChangeType(propertyValue.As<IDimension>().Value, typeof(T));

                return true;

            }
            else if (propertyValue.Type.Equals(typeof(string)) && PropertyValue.TryParse((string)propertyValue.Value, out PropertyValue<T> parsedStringValue)) {

                // Attempt to parse the string into the desired type.

                value = parsedStringValue.Value;

                return true;

            }

            // We were not able to cast to the requested type.

            return false;

        }

    }

}