using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public enum BorderStyle {
        Dotted,
        Dashed,
        Solid,
        Double,
        Groove,
        Ridge,
        Inset,
        Outset,
        None,
        Hidden
    }

    public class BorderStyleProperty :
        PropertyBase<BorderStyle> {

        // Public members

        public BorderStyleProperty(PropertyType type, BorderStyle value, bool inheritable = false) :
            base(type, value, inheritable) {
        }

        public override string ToString() {

            return ToString(Value.ToString().ToLowerInvariant());

        }

    }

}