using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets {

    public static class Property {

        // Public members

        public static IProperty Create(string name, object value) {

            return Create(name, new[] { value });

        }
        public static IProperty Create(string name, object[] values) {

            PropertyType type = GetType(name);

            return Create(type, values);

        }
        public static IProperty Create(PropertyType type, object value) {

            return Create(type, new[] { value });

        }
        public static IProperty Create(PropertyType type, object[] values) {

            switch (type) {

                case PropertyType.Border:
                    return new BorderProperty(type, ToBorder(values), false);

                case PropertyType.BorderColor:
                    return new ColorProperty(type, ToColor(values[0]), false);

                case PropertyType.BackgroundColor:
                case PropertyType.Color:
                case PropertyType.BorderTopColor:
                case PropertyType.BorderRightColor:
                case PropertyType.BorderBottomColor:
                case PropertyType.BorderLeftColor:
                    return new ColorProperty(type, ToColor(values[0]), true);

                case PropertyType.BorderRadius:
                    return new BorderRadiusProperty(ToNumber(values[0]));

                case PropertyType.BorderTopLeftRadius:
                case PropertyType.BorderTopRightRadius:
                case PropertyType.BorderBottomRightRadius:
                case PropertyType.BorderBottomLeftRadius:
                case PropertyType.BorderWidth:
                case PropertyType.BorderTopWidth:
                case PropertyType.BorderRightWidth:
                case PropertyType.BorderBottomWidth:
                case PropertyType.BorderLeftWidth:
                    return new NumberProperty(type, ToNumber(values[0]), false);

                case PropertyType.BorderStyle:
                case PropertyType.BorderTopStyle:
                case PropertyType.BorderRightStyle:
                case PropertyType.BorderBottomStyle:
                case PropertyType.BorderLeftStyle:
                    return new BorderStyleProperty(type, ToBorderStyle(values[0]));

                default:
                    throw new InvalidPropertyException(type.ToString());

            }

        }

        public static PropertyType GetType(string propertyName) {

            if (propertyTypeDict.Value.TryGetValue(propertyName.ToLowerInvariant(), out PropertyType type))
                return type;
            else
                throw new InvalidPropertyException(propertyName);

        }
        public static string GetName(PropertyType propertyType) {

            if (propertyNameDict.Value.TryGetValue(propertyType, out string name))
                return name;
            else
                throw new InvalidPropertyException(propertyType.ToString());

        }

        // Private members

        private static readonly Lazy<Dictionary<PropertyType, string>> propertyNameDict = new Lazy<Dictionary<PropertyType, string>>(() => CreatePropertyNameDictionary());
        private static readonly Lazy<Dictionary<string, PropertyType>> propertyTypeDict = new Lazy<Dictionary<string, PropertyType>>(() => CreatePropertyTypeDictionary());

        private static Color ToColor(object value) {

            if (value is Color)
                return (Color)value;
            else if (value is string)
                return PropertyUtilities.ParseColor((string)value);
            else
                throw new ArgumentException(nameof(value));

        }
        private static double ToNumber(object value) {

            if (value is double)
                return (double)value;
            else if (value is string)
                return PropertyUtilities.ParseNumber((string)value);
            else
                throw new ArgumentException(nameof(value));

        }
        private static string ToString(object value) {

            return value.ToString();

        }
        private static BorderStyle ToBorderStyle(object value) {

            if (value is BorderStyle)
                return (BorderStyle)value;
            else
                return PropertyUtilities.ParseBorderStyle(ToString(value));

        }
        private static Border ToBorder(object[] values) {

            // At least a border style MUST be specified.
            // Arguments are allowed to occur in any order.

            BorderStyle? borderStyle = values.Select(value => ToString(value))
                .Select(value => PropertyUtilities.TryParseBorderStyle(value, out BorderStyle result) ? (BorderStyle?)result : null)
                .Where(result => result != null)
                .FirstOrDefault();

            if (!borderStyle.HasValue)
                throw new ArgumentException(nameof(values));

            double borderWidth = values.Select(value => ToString(value))
                .Select(value => PropertyUtilities.TryParseNumber(value, out double result) ? (double?)result : null)
                .Where(result => result != null)
                .FirstOrDefault() ?? 0.0;

            Color borderColor = values.Select(value => ToString(value))
               .Select(value => PropertyUtilities.TryParseColor(value, out Color result) ? (Color?)result : null)
               .Where(result => result != null)
               .FirstOrDefault() ?? default;

            return new Border(borderWidth, borderStyle.Value, borderColor);

        }

        private static Dictionary<PropertyType, string> CreatePropertyNameDictionary() {

            Dictionary<PropertyType, string> dict = new Dictionary<PropertyType, string> {
                [PropertyType.BackgroundColor] = "background-color",
                [PropertyType.BorderBottomColor] = "border-bottom-color",
                [PropertyType.BorderBottomLeftRadius] = "border-bottom-left-radius",
                [PropertyType.BorderBottomRightRadius] = "border-bottom-right-radius",
                [PropertyType.BorderBottomStyle] = "border-bottom-style",
                [PropertyType.BorderBottomWidth] = "border-bottom-width",
                [PropertyType.BorderBottom] = "border-bottom",
                [PropertyType.BorderColor] = "border-color",
                [PropertyType.BorderLeftColor] = "border-left-color",
                [PropertyType.BorderLeftStyle] = "border-left-style",
                [PropertyType.BorderLeftWidth] = "border-left-width",
                [PropertyType.BorderLeft] = "border-left",
                [PropertyType.BorderRadius] = "border-radius",
                [PropertyType.BorderRightColor] = "border-right-color",
                [PropertyType.BorderRightStyle] = "border-right-style",
                [PropertyType.BorderRightWidth] = "border-right-width",
                [PropertyType.BorderRight] = "border-right",
                [PropertyType.BorderStyle] = "border-style",
                [PropertyType.BorderTopColor] = "border-top-color",
                [PropertyType.BorderTopLeftRadius] = "border-top-left-radius",
                [PropertyType.BorderTopRightRadius] = "border-top-right-radius",
                [PropertyType.BorderTopStyle] = "border-top-style",
                [PropertyType.BorderTopWidth] = "border-top-width",
                [PropertyType.BorderTop] = "border-top",
                [PropertyType.BorderWidth] = "border-width",
                [PropertyType.Border] = "border",
                [PropertyType.Color] = "color",
            };

            return dict;

        }
        private static Dictionary<string, PropertyType> CreatePropertyTypeDictionary() {

            return propertyNameDict.Value.ToDictionary(pair => pair.Value, pair => pair.Key);

        }

    }

}