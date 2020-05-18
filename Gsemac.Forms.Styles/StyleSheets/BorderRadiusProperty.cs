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

        public BorderRadius(double value) {

            TopLeft = value;
            TopRight = value;
            BottomLeft = value;
            BottomRight = value;

        }
        public BorderRadius(double topLeftBottomRight, double topRightBottomLeft) {

            TopLeft = topLeftBottomRight;
            TopRight = topRightBottomLeft;
            BottomLeft = topRightBottomLeft;
            BottomRight = topLeftBottomRight;

        }
        public BorderRadius(double topLeft, double topRightBottomLeft, double bottomRight) {

            TopLeft = topLeft;
            TopRight = topRightBottomLeft;
            BottomLeft = topRightBottomLeft;
            BottomRight = bottomRight;

        }
        public BorderRadius(double topLeft, double topRight, double bottomRight, double bottomLeft) {

            TopLeft = topLeft;
            TopRight = topRight;
            BottomLeft = bottomLeft;
            BottomRight = bottomRight;

        }

        public override string ToString() {

            return $"{TopLeft}px {TopRight}px {BottomLeft}px {BottomRight}px";

        }

    }

    public class BorderRadiusProperty :
        PropertyBase<BorderRadius> {

        // Public members

        public BorderRadiusProperty() :
            this(0.0) {
        }
        public BorderRadiusProperty(BorderRadius propertyValue, bool inheritable = false) :
            base(PropertyType.BorderRadius, propertyValue, inheritable) {
        }
        public BorderRadiusProperty(double propertyValue, bool inheritable = false) :
            base(PropertyType.BorderRadius, new BorderRadius(propertyValue), inheritable) {

        }

    }

}