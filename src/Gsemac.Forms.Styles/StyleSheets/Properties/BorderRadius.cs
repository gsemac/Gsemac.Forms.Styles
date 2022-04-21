using System.Collections;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public class BorderRadius :
        IEnumerable<double> {

        public double TopLeft { get; set; } = 0.0;
        public double TopRight { get; set; } = 0.0;
        public double BottomRight { get; set; } = 0.0;
        public double BottomLeft { get; set; } = 0.0;

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

        public IEnumerator<double> GetEnumerator() {

            yield return TopLeft;
            yield return TopRight;
            yield return BottomRight;
            yield return BottomLeft;

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

    }

}