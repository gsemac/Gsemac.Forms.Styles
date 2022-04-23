using Gsemac.Forms.Styles.Properties;
using System;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion {

    internal class StringToBorderStyleConverter :
        ValueConverterBase<string, BorderStyle> {

        // Public members

        public override BorderStyle Convert(string value) {

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            switch (value.Trim().ToLowerInvariant()) {

                case "dotted":
                    return BorderStyle.Dotted;

                case "dashed":
                    return BorderStyle.Dashed;

                case "solid":
                    return BorderStyle.Solid;

                case "double":
                    return BorderStyle.Double;

                case "groove":
                    return BorderStyle.Groove;

                case "ridge":
                    return BorderStyle.Ridge;

                case "inset":
                    return BorderStyle.Inset;

                case "outset":
                    return BorderStyle.Outset;

                case "none":
                    return BorderStyle.None;

                case "hidden":
                    return BorderStyle.Hidden;

                default:
                    throw new ArgumentException(string.Format(ExceptionMessages.MalformedPropertyValueAsType, value, DestinationType), nameof(value));

            }

        }

    }

}