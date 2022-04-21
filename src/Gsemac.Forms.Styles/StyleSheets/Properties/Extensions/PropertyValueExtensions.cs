using Gsemac.Forms.Styles.Properties;
using System;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.Extensions {

    public static class PropertyValueExtensions {

        // Public members

        public static T As<T>(this IPropertyValue propertyValue) {

            if (propertyValue is null)
                throw new ArgumentNullException(nameof(propertyValue));

            // Since all numbers are parsed as measurements, allow them to be casted into numeric types.

            if (propertyValue.Type.Equals(typeof(IMeasurement)) && TypeUtilities.IsNumericType(typeof(T)))
                return (T)Convert.ChangeType(propertyValue.As<IMeasurement>().Value, typeof(T));

            if (typeof(T) != propertyValue.Type)
                throw new InvalidCastException(string.Format(ExceptionMessages.CannotCastPropertyOfTypeToType, propertyValue.Type, typeof(T).Name));

            return (T)propertyValue.Value;

        }

    }

}