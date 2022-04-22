using System.Collections;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public sealed class BorderRadius :
        IEnumerable<Length> {

        public Length TopLeft { get; } = Length.FromPixels(0);
        public Length TopRight { get; } = Length.FromPixels(0);
        public Length BottomRight { get; } = Length.FromPixels(0);
        public Length BottomLeft { get; } = Length.FromPixels(0);

        public BorderRadius() {
        }
        public BorderRadius(Length value) {

            TopLeft = value;
            TopRight = value;
            BottomLeft = value;
            BottomRight = value;

        }
        public BorderRadius(Length topLeftBottomRight, Length topRightBottomLeft) {

            TopLeft = topLeftBottomRight;
            TopRight = topRightBottomLeft;
            BottomLeft = topRightBottomLeft;
            BottomRight = topLeftBottomRight;

        }
        public BorderRadius(Length topLeft, Length topRightBottomLeft, Length bottomRight) {

            TopLeft = topLeft;
            TopRight = topRightBottomLeft;
            BottomLeft = topRightBottomLeft;
            BottomRight = bottomRight;

        }
        public BorderRadius(Length topLeft, Length topRight, Length bottomRight, Length bottomLeft) {

            TopLeft = topLeft;
            TopRight = topRight;
            BottomLeft = bottomLeft;
            BottomRight = bottomRight;

        }

        public IEnumerator<Length> GetEnumerator() {

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