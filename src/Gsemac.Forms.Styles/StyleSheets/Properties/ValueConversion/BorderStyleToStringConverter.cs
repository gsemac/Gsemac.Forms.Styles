using Gsemac.Data.ValueConversion;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion {

    internal class BorderStyleToStringConverter :
        ValueConverterBase<BorderStyle, string> {

        // Public members

        public override bool TryConvert(BorderStyle value, out string result) {

            switch (value) {

                case BorderStyle.Dotted:
                    result = "dotted";
                    return true;

                case BorderStyle.Dashed:
                    result = "dashed";
                    return true;

                case BorderStyle.Solid:
                    result = "solid";
                    return true;

                case BorderStyle.Double:
                    result = "double";
                    return true;

                case BorderStyle.Groove:
                    result = "groove";
                    return true;

                case BorderStyle.Ridge:
                    result = "ridge";
                    return true;

                case BorderStyle.Inset:
                    result = "inset";
                    return true;

                case BorderStyle.Outset:
                    result = "outset";
                    return true;

                case BorderStyle.None:
                    result = "none";
                    return true;

                case BorderStyle.Hidden:
                    result = "hidden";
                    return true;

                default:
                    result = default;
                    return false;

            }

        }

    }

}