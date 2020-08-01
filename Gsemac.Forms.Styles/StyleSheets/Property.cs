using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets {

    public static class Property {

        // Public members

        public static IProperty Create(string name, StyleObject value) {

            return Create(name, new[] { value });

        }
        public static IProperty Create(string name, StyleObject[] values) {

            PropertyType type = GetType(name);

            return Create(type, values);

        }
        public static IProperty Create(PropertyType type, StyleObject value) {

            return Create(type, new[] { value });

        }
        public static IProperty Create(PropertyType type, StyleObject[] values) {

            switch (type) {

                case PropertyType.Border:
                    return new BorderProperty(type, ToBorder(values), false);

                case PropertyType.BorderColor:
                    return new ColorProperty(type, values[0].GetColor(), false);

                case PropertyType.BackgroundColor:
                case PropertyType.BorderBottomColor:
                case PropertyType.BorderLeftColor:
                case PropertyType.BorderRightColor:
                case PropertyType.BorderTopColor:
                case PropertyType.Color:
                    return new ColorProperty(type, values[0].GetColor(), true);

                case PropertyType.BorderRadius:
                    return new BorderRadiusProperty(ToBorderRadius(values));

                case PropertyType.BorderBottomLeftRadius:
                case PropertyType.BorderBottomRightRadius:
                case PropertyType.BorderBottomWidth:
                case PropertyType.BorderLeftWidth:
                case PropertyType.BorderRightWidth:
                case PropertyType.BorderTopLeftRadius:
                case PropertyType.BorderTopRightRadius:
                case PropertyType.BorderTopWidth:
                case PropertyType.BorderWidth:
                    return new NumberProperty(type, values[0].GetNumber(), false);

                case PropertyType.BorderBottomStyle:
                case PropertyType.BorderLeftStyle:
                case PropertyType.BorderRightStyle:
                case PropertyType.BorderStyle:
                case PropertyType.BorderTopStyle:
                    return new BorderStyleProperty(type, values[0].GetBorderStyle());

                case PropertyType.BackgroundImage:
                    return new BackgroundImageProperty(values.Select(v => v.GetImage()).ToArray());

                case PropertyType.Opacity:
                    return new NumberProperty(type, values[0].GetNumber(), false);

                default:
                    throw new InvalidPropertyException(type.ToString());

            }

        }

        public static IProperty Create(string name, object value) {

            return Create(name, new StyleObject(value));

        }
        public static IProperty Create(string name, object[] values) {

            return Create(name, values.Select(v => new StyleObject(v)).ToArray());

        }
        public static IProperty Create(PropertyType type, object value) {

            return Create(type, new StyleObject(value));

        }
        public static IProperty Create(PropertyType type, object[] values) {

            return Create(type, values.Select(v => new StyleObject(v)).ToArray());

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
                throw new ArgumentException(nameof(propertyType));

        }

        // Private members

        private static readonly Lazy<Dictionary<PropertyType, string>> propertyNameDict = new Lazy<Dictionary<PropertyType, string>>(() => CreatePropertyNameDictionary());
        private static readonly Lazy<Dictionary<string, PropertyType>> propertyTypeDict = new Lazy<Dictionary<string, PropertyType>>(() => CreatePropertyTypeDictionary());

        private static Border ToBorder(StyleObject[] values) {

            // At least a border style MUST be specified.
            // Arguments are allowed to occur in any order.

            BorderStyle? borderStyle = values.Select(value => value.GetString())
                .Select(value => PropertyUtilities.TryParseBorderStyle(value, out BorderStyle result) ? (BorderStyle?)result : null)
                .Where(result => result != null)
                .FirstOrDefault();

            if (!borderStyle.HasValue)
                throw new ArgumentException(nameof(values));

            double borderWidth = values.Select(value => value.GetString())
                .Select(value => PropertyUtilities.TryParseNumber(value, out double result) ? (double?)result : null)
                .Where(result => result != null)
                .FirstOrDefault() ?? 0.0;

            Color borderColor = values.Select(value => value.GetString())
               .Select(value => PropertyUtilities.TryParseColor(value, out Color result) ? (Color?)result : null)
               .Where(result => result != null)
               .FirstOrDefault() ?? default;

            return new Border(borderWidth, borderStyle.Value, borderColor);

        }
        private static BorderRadius ToBorderRadius(StyleObject[] values) {

            if (values.Count() == 4)
                return new BorderRadius(values[0].GetNumber(), values[1].GetNumber(), values[2].GetNumber(), values[3].GetNumber());
            else if (values.Count() == 3)
                return new BorderRadius(values[0].GetNumber(), values[1].GetNumber(), values[2].GetNumber());
            else if (values.Count() == 2)
                return new BorderRadius(values[0].GetNumber(), values[1].GetNumber());
            else
                return new BorderRadius(values[0].GetNumber());

        }

        private static Dictionary<PropertyType, string> CreatePropertyNameDictionary() {

            Dictionary<PropertyType, string> dict = new Dictionary<PropertyType, string> {
                [PropertyType.BackgroundColor] = "background-color",
                [PropertyType.BackgroundImage] = "background-image",
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
                [PropertyType.Opacity] = "opacity",
            };

            return dict;

        }
        private static Dictionary<string, PropertyType> CreatePropertyTypeDictionary() {

            return propertyNameDict.Value.ToDictionary(pair => pair.Value, pair => pair.Key);

        }

    }

}