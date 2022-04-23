namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    internal static class PropertyUtilities {

        // Public members

        public static bool IsBorderRadiusProperty(IProperty property) {

            return property.Name == PropertyName.BorderTopLeftRadius ||
                property.Name == PropertyName.BorderTopRightRadius ||
                property.Name == PropertyName.BorderBottomRightRadius ||
                property.Name == PropertyName.BorderBottomLeftRadius ||
                property.Name == PropertyName.BorderRadius;

        }
        public static bool IsBorderWidthProperty(IProperty property) {

            return property.Name == PropertyName.BorderTopWidth ||
               property.Name == PropertyName.BorderRightWidth ||
               property.Name == PropertyName.BorderBottomWidth ||
               property.Name == PropertyName.BorderLeftWidth ||
               property.Name == PropertyName.BorderWidth;

        }
        public static bool IsBorderColorProperty(IProperty property) {

            return property.Name == PropertyName.BorderTopColor ||
               property.Name == PropertyName.BorderRightColor ||
               property.Name == PropertyName.BorderBottomColor ||
               property.Name == PropertyName.BorderLeftColor ||
               property.Name == PropertyName.BorderColor;

        }

    }

}