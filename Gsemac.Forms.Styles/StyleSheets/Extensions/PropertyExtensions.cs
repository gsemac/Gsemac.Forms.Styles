namespace Gsemac.Forms.Styles.StyleSheets.Extensions {

    public static class PropertyExtensions {

        public static bool HasValue(this IProperty property) {

            return property != null;

        }

        public static bool IsBorderRadiusProperty(this IProperty property) {

            return property.Type == PropertyType.BorderTopLeftRadius ||
                property.Type == PropertyType.BorderTopRightRadius ||
                property.Type == PropertyType.BorderBottomRightRadius ||
                property.Type == PropertyType.BorderBottomLeftRadius ||
                property.Type == PropertyType.BorderRadius;

        }
        public static bool IsBorderWidthProperty(this IProperty property) {

            return property.Type == PropertyType.BorderTopWidth ||
               property.Type == PropertyType.BorderRightWidth ||
               property.Type == PropertyType.BorderBottomWidth ||
               property.Type == PropertyType.BorderLeftWidth ||
               property.Type == PropertyType.BorderWidth;

        }
        public static bool IsBorderColorProperty(this IProperty property) {

            return property.Type == PropertyType.BorderTopColor ||
               property.Type == PropertyType.BorderRightColor ||
               property.Type == PropertyType.BorderBottomColor ||
               property.Type == PropertyType.BorderLeftColor ||
               property.Type == PropertyType.BorderColor;

        }

    }

}