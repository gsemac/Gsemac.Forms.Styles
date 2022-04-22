using System;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.Extensions {

    public static class PropertyExtensions {

        // Public members

        public static IEnumerable<IProperty> GetChildProperties(this IProperty property) {

            if (property is null)
                throw new ArgumentNullException(nameof(property));

            return property.GetChildProperties(PropertyFactory.Default);

        }

        public static T GetValueAs<T>(this IProperty property) {

            if (property is null)
                throw new ArgumentNullException(nameof(property));

            return new PropertyValue(property.ValueType, property.Value)
                .GetValueAs<T>();

        }
        public static bool TryGetValueAs<T>(this IProperty property, out T value) {

            value = default;

            if (property is null)
                throw new ArgumentNullException(nameof(property));

            return new PropertyValue(property.ValueType, property.Value)
                .TryGetValueAs(out value);

        }

    }

}