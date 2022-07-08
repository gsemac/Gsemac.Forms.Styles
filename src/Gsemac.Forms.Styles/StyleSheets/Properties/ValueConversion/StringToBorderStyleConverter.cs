using Gsemac.Data.ValueConversion;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion {

    internal class StringToBorderStyleConverter :
        ValueConverterBase<string, BorderStyle> {

        // Public members

        public override bool TryConvert(string value, out BorderStyle result) {

            result = default;

            if (value is null)
                return false;

            switch (value.Trim().ToLowerInvariant()) {

                case "dotted":
                    result = BorderStyle.Dotted;
                    return true;

                case "dashed":
                    result = BorderStyle.Dashed;
                    return true;

                case "solid":
                    result = BorderStyle.Solid;
                    return true;

                case "double":
                    result = BorderStyle.Double;
                    return true;

                case "groove":
                    result = BorderStyle.Groove;
                    return true;

                case "ridge":
                    result = BorderStyle.Ridge;
                    return true;

                case "inset":
                    result = BorderStyle.Inset;
                    return true;

                case "outset":
                    result = BorderStyle.Outset;
                    return true;

                case "none":
                    result = BorderStyle.None;
                    return true;

                case "hidden":
                    result = BorderStyle.Hidden;
                    return true;

                default:
                    return false;

            }

        }

    }

}