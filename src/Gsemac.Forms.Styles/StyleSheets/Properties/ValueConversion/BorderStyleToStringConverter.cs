using System;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion {

    internal class BorderStyleToStringConverter :
        ValueConverterBase<BorderStyle, string> {

        // Public members

        public override string Convert(BorderStyle value) {

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

    }

}