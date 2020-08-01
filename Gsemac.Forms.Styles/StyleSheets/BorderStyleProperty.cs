using System;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class BorderStyleProperty :
        PropertyBase<BorderStyle> {

        // Public members

        public BorderStyleProperty(PropertyType type, BorderStyle value, bool inheritable = false) :
            base(type, value, inheritable) {
        }

        public override string ToString() {

            return ToString(BorderStyleToString(Value).ToLowerInvariant());

        }

        // Private members

        private string BorderStyleToString(BorderStyle borderStyle) {

            switch (borderStyle) {

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
                    throw new ArgumentException(nameof(borderStyle));

            }

        }

    }

}