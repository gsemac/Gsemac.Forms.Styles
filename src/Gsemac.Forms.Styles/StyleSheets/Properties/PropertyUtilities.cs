using System;
using System.Drawing;

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

        public static string SerializeColor(Color value) {

            return ColorTranslator.ToHtml(value).ToLowerInvariant();

        }
        public static string SerializeBorderStyle(BorderStyle value) {

            switch (value) {

                case BorderStyle.Dotted:
                    return "dotted";

                case BorderStyle.Dashed:
                    return "dashed";

                case BorderStyle.Solid:
                    return "solid";

                case BorderStyle.Double:
                    return "double";

                case BorderStyle.Groove:
                    return "groove";

                case BorderStyle.Ridge:
                    return "ridge";

                case BorderStyle.Inset:
                    return "inset";

                case BorderStyle.Outset:
                    return "outset";

                case BorderStyle.None:
                    return "none";

                case BorderStyle.Hidden:
                    return "hidden";

                default:
                    throw new ArgumentOutOfRangeException(nameof(value));

            }

        }

    }

}