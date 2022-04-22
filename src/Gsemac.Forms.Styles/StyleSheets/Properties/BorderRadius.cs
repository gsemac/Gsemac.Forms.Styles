using System.Collections;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public sealed class BorderRadius :
        IEnumerable<IMeasurement> {

        public IMeasurement TopLeft { get; } = Measurement.FromPixels(0);
        public IMeasurement TopRight { get; } = Measurement.FromPixels(0);
        public IMeasurement BottomRight { get; } = Measurement.FromPixels(0);
        public IMeasurement BottomLeft { get; } = Measurement.FromPixels(0);

        public BorderRadius() {
        }
        public BorderRadius(IMeasurement value) {

            TopLeft = value;
            TopRight = value;
            BottomLeft = value;
            BottomRight = value;

        }
        public BorderRadius(IMeasurement topLeftBottomRight, IMeasurement topRightBottomLeft) {

            TopLeft = topLeftBottomRight;
            TopRight = topRightBottomLeft;
            BottomLeft = topRightBottomLeft;
            BottomRight = topLeftBottomRight;

        }
        public BorderRadius(IMeasurement topLeft, IMeasurement topRightBottomLeft, IMeasurement bottomRight) {

            TopLeft = topLeft;
            TopRight = topRightBottomLeft;
            BottomLeft = topRightBottomLeft;
            BottomRight = bottomRight;

        }
        public BorderRadius(IMeasurement topLeft, IMeasurement topRight, IMeasurement bottomRight, IMeasurement bottomLeft) {

            TopLeft = topLeft;
            TopRight = topRight;
            BottomLeft = bottomLeft;
            BottomRight = bottomRight;

        }

        public IEnumerator<IMeasurement> GetEnumerator() {

            yield return TopLeft;
            yield return TopRight;
            yield return BottomRight;
            yield return BottomLeft;

        }

        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        public override string ToString() {

            return $"{TopLeft} {TopRight} {BottomRight} {BottomLeft}";

        }

    }

}