using Gsemac.Forms.Styles.Properties;
using System;
using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public class PropertyValue :
        IPropertyValue {

        // Public members

        public Type Type { get; }
        public object Value { get; }

        public static PropertyValue Null { get; } = new PropertyValue(typeof(object), null);

        public static PropertyValue<T> Create<T>(T value) {

            return new PropertyValue<T>(value);

        }

        public static PropertyValue Parse(string value) {

            if (TryParse(value, out PropertyValue result))
                return result;

            throw new FormatException(string.Format(ExceptionMessages.MalformedPropertyValue, value));

        }
        public static PropertyValue<T> Parse<T>(string value) {

            if (TryParse(value, out PropertyValue<T> result))
                return result;

            throw new FormatException(string.Format(ExceptionMessages.MalformedPropertyValueAsType, value, typeof(T).Name));

        }
        public static bool TryParse(string value, out PropertyValue result) {

            result = Null;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            if (TryParseBorderStyle(value, out BorderStyle borderStyle)) {

                result = Create(borderStyle);

            }
            else if (TryParseColor(value, out Color parsedColor)) {

                result = Create(parsedColor);

            }
            else if (Angle.TryParse(value, out Angle parsedAngle)) {

                result = Create(parsedAngle);

            }
            else if (Length.TryParse(value, out Length parsedLength)) {

                result = Create(parsedLength);

            }
            else {

                // Treat the value as a string (e.g. a background image URL).

                result = Create(value);

            }

            // If we get here, the value was successfully parsed.

            return true;

        }
        public static bool TryParse<T>(string value, out PropertyValue<T> result) {

            result = null;

            if (TryParse(value, out PropertyValue intermediateResult) && intermediateResult.Type.Equals(typeof(T))) {

                result = Create((T)intermediateResult.Value);

                return true;

            }

            return false;

        }

        // Internal members

        internal PropertyValue(Type type, object value) {

            if (type is null)
                throw new ArgumentNullException(nameof(type));

            Type = type;
            Value = value;

        }

        // Private members

        private static bool TryParseBorderStyle(string input, out BorderStyle result) {

            result = BorderStyle.None;

            if (string.IsNullOrWhiteSpace(input))
                return false;

            switch (input.Trim().ToLowerInvariant()) {

                case "dotted":
                    result = BorderStyle.Dotted;
                    break;

                case "dashed":
                    result = BorderStyle.Dashed;
                    break;

                case "solid":
                    result = BorderStyle.Solid;
                    break;

                case "double":
                    result = BorderStyle.Double;
                    break;

                case "groove":
                    result = BorderStyle.Groove;
                    break;

                case "ridge":
                    result = BorderStyle.Ridge;
                    break;

                case "inset":
                    result = BorderStyle.Inset;
                    break;

                case "outset":
                    result = BorderStyle.Outset;
                    break;

                case "none":
                    result = BorderStyle.None;
                    break;

                case "hidden":
                    result = BorderStyle.Hidden;
                    break;

                default:
                    result = BorderStyle.None;
                    return false;

            }

            return true;

        }
        private static bool TryParseColor(string input, out Color result) {

            result = Color.Transparent;

            if (string.IsNullOrWhiteSpace(input))
                return false;

            try {

                input = input.Replace("grey", "gray");

                result = ColorTranslator.FromHtml(input);

                return true;

            }
            catch (Exception) {

                result = default;

                return false;

            }

        }

    }

    public class PropertyValue<T> :
        PropertyValue,
        IPropertyValue<T> {

        // Public members

        public new T Value { get; }

        public PropertyValue(T value) :
            base(typeof(T), value) {

            Value = value;

        }

    }

}