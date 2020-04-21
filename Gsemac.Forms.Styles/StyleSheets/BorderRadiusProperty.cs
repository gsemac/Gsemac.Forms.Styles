using Gsemac.Forms.Styles.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class BorderRadius {

        public double TopLeft { get; set; } = 0.0;
        public double TopRight { get; set; } = 0.0;
        public double BottomLeft { get; set; } = 0.0;
        public double BottomRight { get; set; } = 0.0;

        public bool IsGreaterThanZero() {

            return TopLeft > 0.0 ||
                TopRight > 0.0 ||
                BottomLeft > 0.0 ||
                BottomRight > 0.0;

        }

    }

    public class BorderRadiusProperty :
        PropertyBase<BorderRadius> {

        // Public members

        public BorderRadiusProperty(BorderRadius propertyValue) :
            this(PropertyType.BorderRadius, propertyValue, false) {
        }
        public BorderRadiusProperty(string propertyValue) :
            this(PropertyType.BorderRadius, propertyValue, false) {

        }
        public BorderRadiusProperty(PropertyType propertyType, BorderRadius propertyValue, bool inheritable = true) :
            base(propertyType, propertyValue, inheritable) {
        }
        public BorderRadiusProperty(PropertyType propertyType, string propertyValue, bool inheritable = true) :
           base(propertyType, ParseBorderRadius(propertyValue), inheritable) {
        }

        // Private members

        private static BorderRadius ParseBorderRadius(string input) {

            double value = PropertyUtilities.ParseNumber(input);

            return new BorderRadius() {
                TopLeft = value,
                TopRight = value,
                BottomLeft = value,
                BottomRight = value
            };

        }

    }

}