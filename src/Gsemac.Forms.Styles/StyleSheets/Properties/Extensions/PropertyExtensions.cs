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

        public static bool HasValue(this IProperty property) {

            return property != null;

        }

        public static bool IsBorderRadiusProperty(this IProperty property) {

            return property.Name == PropertyName.BorderTopLeftRadius ||
                property.Name == PropertyName.BorderTopRightRadius ||
                property.Name == PropertyName.BorderBottomRightRadius ||
                property.Name == PropertyName.BorderBottomLeftRadius ||
                property.Name == PropertyName.BorderRadius;

        }
        public static bool IsBorderWidthProperty(this IProperty property) {

            return property.Name == PropertyName.BorderTopWidth ||
               property.Name == PropertyName.BorderRightWidth ||
               property.Name == PropertyName.BorderBottomWidth ||
               property.Name == PropertyName.BorderLeftWidth ||
               property.Name == PropertyName.BorderWidth;

        }
        public static bool IsBorderColorProperty(this IProperty property) {

            return property.Name == PropertyName.BorderTopColor ||
               property.Name == PropertyName.BorderRightColor ||
               property.Name == PropertyName.BorderBottomColor ||
               property.Name == PropertyName.BorderLeftColor ||
               property.Name == PropertyName.BorderColor;

        }

    }

}