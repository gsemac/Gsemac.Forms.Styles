using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;
using System.Drawing;
using System.Globalization;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    internal static class PropertyUtilities {

        // Internal members

        internal static bool IsInheritable(string propertyName) {

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

        internal static IPropertyValue GetInitialValue(string propertyName) {

            return GetInitialValue(propertyName, Ruleset.Empty);

        }
        internal static IPropertyValue GetInitialValue(string propertyName, IRuleset style) {

            if (string.IsNullOrWhiteSpace(propertyName))
                return null;

            switch (propertyName.ToLowerInvariant()) {

                case PropertyName.BackgroundColor:
                    return PropertyValue.Create(Color.Transparent);

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
        internal static T GetInitialValue<T>(string propertyName) {

            return GetInitialValue<T>(propertyName, Ruleset.Empty);

        }
        internal static T GetInitialValue<T>(string propertyName, IRuleset style) {

            if (style is null)
                throw new ArgumentNullException(nameof(style));

            IPropertyValue initialValue = GetInitialValue(propertyName);

            if (initialValue is object && initialValue.Type.Equals(typeof(T)))
                return (T)initialValue;

            return default;

        }

        internal static string SerializePropertyValue(object value) {

            if (value is null)
                return string.Empty;

            switch (value) {

                case BorderStyle borderStyle:
                    return BorderStyletoString(borderStyle);

                case Color color:
                    return ColorToString(color);

                case double @double:
                    return @double.ToString(CultureInfo.InvariantCulture);

                default:
                    return value.ToString();

            }

        }

        // Private members

        private static string BorderStyletoString(BorderStyle value) {

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
        private static string ColorToString(Color value) {

            return ColorTranslator.ToHtml(value).ToLowerInvariant();

        }

    }

}