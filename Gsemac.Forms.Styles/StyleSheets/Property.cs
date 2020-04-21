﻿using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets {

    public static class Property {

        // Public members

        public static IProperty Create(string propertyName, string propertyValue) {

            PropertyType type = GetType(propertyName);

            return Create(type, propertyValue);

        }
        public static IProperty Create(PropertyType type, string propertyValue) {

            switch (type) {

                case PropertyType.BorderColor:
                    return new ColorProperty(type, propertyValue, false);

                case PropertyType.BackgroundColor:
                case PropertyType.Color:
                    return new ColorProperty(type, propertyValue, true);

                case PropertyType.BorderRadius:
                    return new BorderRadiusProperty(type, propertyValue, false);

                case PropertyType.BorderTopLeftRadius:
                case PropertyType.BorderTopRightRadius:
                case PropertyType.BorderBottomRightRadius:
                case PropertyType.BorderBottomLeftRadius:
                case PropertyType.BorderWidth:
                    return new NumberProperty(type, propertyValue, false);

                default:
                    throw new InvalidPropertyException(type.ToString());

            }

        }

        public static PropertyType GetType(string propertyName) {

            switch (propertyName.ToLowerInvariant()) {

                case "background-color":
                    return PropertyType.BackgroundColor;

                case "border-color":
                    return PropertyType.BorderColor;

                case "border-radius":
                    return PropertyType.BorderRadius;

                case "border-top-left-radius":
                    return PropertyType.BorderTopLeftRadius;

                case "border-top-right-radius":
                    return PropertyType.BorderTopRightRadius;

                case "border-bottom-left-radius":
                    return PropertyType.BorderBottomLeftRadius;

                case "border-bottom-right-radius":
                    return PropertyType.BorderBottomRightRadius;

                case "border-width":
                    return PropertyType.BorderWidth;

                case "color":
                    return PropertyType.Color;

                default:
                    throw new InvalidPropertyException(propertyName);

            }

        }
        public static string GetName(PropertyType propertyType) {

            switch (propertyType) {

                case PropertyType.BackgroundColor:
                    return "background-color";

                case PropertyType.BorderColor:
                    return "border-color";

                case PropertyType.BorderRadius:
                    return "border-radius";

                case PropertyType.BorderTopLeftRadius:
                    return "border-top-left-radius";

                case PropertyType.BorderTopRightRadius:
                    return "border-top-right-radius";

                case PropertyType.BorderBottomLeftRadius:
                    return "border-bottom-left-radius";

                case PropertyType.BorderBottomRightRadius:
                    return "border-bottom-right-radius";

                case PropertyType.BorderWidth:
                    return "border-width";

                case PropertyType.Color:
                    return "color";

                default:
                    throw new InvalidPropertyException(((int)propertyType).ToString());

            }

        }

    }

}