using Gsemac.Forms.Styles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class ColorProperty :
        PropertyBase<Color> {

        // Public members

        public ColorProperty(PropertyType propertyType, Color propertyValue, bool inheritable = true) :
            base(propertyType, propertyValue, inheritable) {
        }
        public ColorProperty(PropertyType propertyType, string propertyValue, bool inheritable = true) :
          base(propertyType, PropertyUtilities.ParseColor(propertyValue), inheritable) {
        }

        public override string ToString() {

            return ToString(ColorTranslator.ToHtml(Value).ToLowerInvariant());

        }

    }

}