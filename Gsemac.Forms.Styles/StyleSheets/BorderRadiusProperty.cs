using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class BorderRadii {

        public double TopLeft { get; set; } = 0.0;
        public double TopRight { get; set; } = 0.0;
        public double BottomLeft { get; set; } = 0.0;
        public double BottomRight { get; set; } = 0.0;

        public BorderRadii(double value) {

            TopLeft = value;
            TopRight = value;
            BottomLeft = value;
            BottomRight = value;

        }

        public bool IsGreaterThanZero() {

            return TopLeft > 0.0 ||
                TopRight > 0.0 ||
                BottomLeft > 0.0 ||
                BottomRight > 0.0;

        }

    }

    public class BorderRadiusProperty :
        PropertyBase<BorderRadii> {

        // Public members

        public BorderRadiusProperty() :
            this(0.0) {
        }
        public BorderRadiusProperty(BorderRadii propertyValue, bool inheritable = false) :
            base(PropertyType.BorderRadius, propertyValue, inheritable) {
        }
        public BorderRadiusProperty(double propertyValue, bool inheritable = false) :
            base(PropertyType.BorderRadius, new BorderRadii(propertyValue), inheritable) {

        }

    }

}