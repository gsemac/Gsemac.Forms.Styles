using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;
using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    internal static class PropertyUtilities {

        // Public members

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

        public static IPropertyValue GetInitialValue(string propertyName) {

            return GetInitialValue(propertyName, Ruleset.Empty);

        }
        public static IPropertyValue GetInitialValue(string propertyName, IRuleset style) {

            if (string.IsNullOrWhiteSpace(propertyName))
                return null;

            switch (propertyName.ToLowerInvariant()) {

                case PropertyName.BackgroundImage:
                    return PropertyValue.Create(new BackgroundImage());

                case PropertyName.BackgroundColor:
                    return PropertyValue.Create(Color.Transparent);

                case PropertyName.Border:
                    return PropertyValue.Create(new Border(style.Color));

                case PropertyName.BorderTopColor:
                case PropertyName.BorderRightColor:
                case PropertyName.BorderBottomColor:
                case PropertyName.BorderLeftColor:
                    return PropertyValue.Create(style.Color);

                case PropertyName.BorderTopStyle:
                case PropertyName.BorderRightStyle:
                case PropertyName.BorderBottomStyle:
                case PropertyName.BorderLeftStyle:
                    return PropertyValue.Create(BorderStyle.None);

                case PropertyName.Color:
                    return PropertyValue.Create(Color.Black); // Dependant on user agent

                default:
                    return PropertyValue.Null;

            }

        }
        public static T GetInitialValue<T>(string propertyName) {

            return GetInitialValue<T>(propertyName, Ruleset.Empty);

        }
        public static T GetInitialValue<T>(string propertyName, IRuleset style) {

            if (style is null)
                throw new ArgumentNullException(nameof(style));

            IPropertyValue initialValue = GetInitialValue(propertyName);

            if (initialValue is object && initialValue.Type.Equals(typeof(T)))
                return (T)initialValue.Value;

            return default;

        }

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