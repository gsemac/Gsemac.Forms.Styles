using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class NumericProperty :
        PropertyBase<double> {

        // Public members

        public NumericProperty(PropertyType propertyType, double propertyValue, bool inheritable = true) :
            base(propertyType, propertyValue, inheritable) {
        }

    }

}