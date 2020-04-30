using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class NumberProperty :
        PropertyBase<double> {

        // Public members

        public NumberProperty(PropertyType propertyType, double propertyValue, bool inheritable = true) :
            base(propertyType, propertyValue, inheritable) {
        }
        public NumberProperty(PropertyType propertyType, string propertyValue, bool inheritable = true) :
            base(propertyType, PropertyUtilities.ParseNumber(propertyValue), inheritable) {
        }

        // Protected members

        protected static double ParseNumber(string input) {

            if (input.EndsWith("px", System.StringComparison.OrdinalIgnoreCase))
                input = input.Substring(0, input.IndexOf("px", System.StringComparison.OrdinalIgnoreCase));

            return double.Parse(input);

        }

    }

}