using System;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class BorderStyleProperty :
        PropertyBase<BorderStyle> {

        // Public members

        public BorderStyleProperty(PropertyType type, BorderStyle value, bool inheritable = false) :
            base(type, value, inheritable) {
        }

        public override string ToString() {

            return ToString(PropertyUtilities.ToString(Value).ToLowerInvariant());

        }

    }

}