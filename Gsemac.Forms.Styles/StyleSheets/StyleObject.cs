using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public enum StyleObjectType {
        String,
        Number,
        Color,
        Image,
        BorderStyle
    }

    public class StyleObject {

        // Public members

        public StyleObjectType Type { get; }

        public StyleObject(object value) :
            this(FromObject(value)) {
        }
        public StyleObject(StyleObject value) {

            this.Type = value.Type;
            this.value = value.value;

        }

        public StyleObject(string value) {

            this.Type = StyleObjectType.String;
            this.value = value;

        }
        public StyleObject(double value) {

            this.Type = StyleObjectType.Number;
            this.value = value;

        }
        public StyleObject(Color value) {

            this.Type = StyleObjectType.Color;
            this.value = value;

        }
        public StyleObject(IImage value) {

            this.Type = StyleObjectType.Image;
            this.value = value;

        }
        public StyleObject(BorderStyle value) {

            this.Type = StyleObjectType.BorderStyle;
            this.value = value;

        }

        public string GetString() {

            switch (Type) {

                case StyleObjectType.BorderStyle:
                    return PropertyUtilities.ToString((BorderStyle)value);

                default:
                    return value.ToString();

            }

        }
        public double GetNumber() {

            switch (Type) {

                case StyleObjectType.Number:
                    return (double)value;

                case StyleObjectType.String:
                    return PropertyUtilities.ParseNumber(GetString());

                default:
                    throw GetInvalidOperationException<double>();

            }

        }
        public Color GetColor() {

            switch (Type) {

                case StyleObjectType.Color:
                    return (Color)value;

                case StyleObjectType.String:
                    return PropertyUtilities.ParseColor(GetString());

                default:
                    throw GetInvalidOperationException<Color>();

            }

        }
        public IImage GetImage() {

            switch (Type) {

                case StyleObjectType.Image:
                    return (IImage)value;

                default:
                    throw GetInvalidOperationException<IImage>();

            }

        }
        public BorderStyle GetBorderStyle() {

            switch (Type) {

                case StyleObjectType.BorderStyle:
                    return (BorderStyle)value;

                case StyleObjectType.String:
                    return PropertyUtilities.ParseBorderStyle(GetString());

                default:
                    throw GetInvalidOperationException<BorderStyle>();

            }

        }

        // Private members

        private readonly object value;

        private InvalidOperationException GetInvalidOperationException<T>() {

            return new InvalidOperationException($"Cannot convert from type {value.GetType().Name} to type {typeof(T).Name}.");

        }

        private static StyleObject FromObject(object value) {

            switch (value) {

                case string stringValue:
                    return new StyleObject(stringValue);

                case double doubleValue:
                    return new StyleObject(doubleValue);

                case float floatValue:
                    return new StyleObject(floatValue);

                case int intValue:
                    return new StyleObject(intValue);

                case Color colorValue:
                    return new StyleObject(colorValue);

                case IImage imageValue:
                    return new StyleObject(imageValue);

                case BorderStyle borderStyleValue:
                    return new StyleObject(borderStyleValue);

                default:
                    throw new ArgumentOutOfRangeException(nameof(value));

            }

        }

    }

}