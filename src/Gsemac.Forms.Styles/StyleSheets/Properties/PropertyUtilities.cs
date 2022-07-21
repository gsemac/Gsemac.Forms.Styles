using System;

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

        public static bool IsInheritable(string propertyName) {

            // https://stackoverflow.com/a/30536051/5383169 (David Bonnet)

            if (propertyName is null)
                return false;

            switch (propertyName.ToLowerInvariant()) {

                case PropertyName.BorderCollapse:
                case PropertyName.BorderSpacing:
                case PropertyName.CaptionSide:
                case PropertyName.Color:
                case PropertyName.Cursor:
                case PropertyName.Direction:
                case PropertyName.EmptyCells:
                case PropertyName.FontFamily:
                case PropertyName.FontSize:
                case PropertyName.FontStyle:
                case PropertyName.FontVariant:
                case PropertyName.FontWeight:
                case PropertyName.FontSizeAdjust:
                case PropertyName.FontStretch:
                case PropertyName.Font:
                case PropertyName.LetterSpacing:
                case PropertyName.LineHeight:
                case PropertyName.ListStyleImage:
                case PropertyName.ListStylePosition:
                case PropertyName.ListStyleType:
                case PropertyName.ListStyle:
                case PropertyName.Orphans:
                case PropertyName.Quotes:
                case PropertyName.TabSize:
                case PropertyName.TextAlign:
                case PropertyName.TextAlignLast:
                case PropertyName.TextDecorationColor:
                case PropertyName.TextIndent:
                case PropertyName.TextJustify:
                case PropertyName.TextShadow:
                case PropertyName.TextTransform:
                case PropertyName.Visibility:
                case PropertyName.WhiteSpace:
                case PropertyName.Widows:
                case PropertyName.WordBreak:
                case PropertyName.WordSpacing:
                case PropertyName.WordWrap:
                    return true;

                default:
                    return false;

            }

        }

        public static bool IsVariableName(string propertyName) {

            if (string.IsNullOrEmpty(propertyName))
                return false;

            return propertyName
                .TrimStart()
                .StartsWith("--");

        }
        public static bool IsVariableReference(IPropertyValue value) {

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            return value.Type.Equals(typeof(VariableReference));

        }
        public static bool IsKeyword(string value) {

            if (string.IsNullOrEmpty(value))
                return false;

            switch (value.Trim().ToLowerInvariant()) {

                case Keyword.Inherit:
                case Keyword.Initial:
                case Keyword.Revert:
                case Keyword.RevertLayer:
                case Keyword.Unset:

                case Keyword.Auto:
                case Keyword.None:

                case Keyword.CanvasText:
                case Keyword.CurrentColor:
                    return true;

                default:
                    return false;

            }

        }
        public static bool IsKeyword(IPropertyValue value) {

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (!value.Is<string>())
                return false;

            return IsKeyword(value.As<string>());

        }

    }

}