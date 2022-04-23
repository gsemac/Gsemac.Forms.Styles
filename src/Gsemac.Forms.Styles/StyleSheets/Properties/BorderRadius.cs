using System.Collections;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public sealed class BorderRadius :
        IEnumerable<ILengthOrPercentage> {

        public ILengthOrPercentage TopLeft { get; } = Length.Zero;
        public ILengthOrPercentage TopRight { get; } = Length.Zero;
        public ILengthOrPercentage BottomRight { get; } = Length.Zero;
        public ILengthOrPercentage BottomLeft { get; } = Length.Zero;

        public BorderRadius() {
        }
        public BorderRadius(ILengthOrPercentage value) {

            TopLeft = value;
            TopRight = value;
            BottomLeft = value;
            BottomRight = value;

        }
        public BorderRadius(ILengthOrPercentage topLeftBottomRight, ILengthOrPercentage topRightBottomLeft) {

            TopLeft = topLeftBottomRight;
            TopRight = topRightBottomLeft;
            BottomLeft = topRightBottomLeft;
            BottomRight = topLeftBottomRight;

        }
        public BorderRadius(ILengthOrPercentage topLeft, ILengthOrPercentage topRightBottomLeft, ILengthOrPercentage bottomRight) {

            TopLeft = topLeft;
            TopRight = topRightBottomLeft;
            BottomLeft = topRightBottomLeft;
            BottomRight = bottomRight;

        }
        public BorderRadius(ILengthOrPercentage topLeft, ILengthOrPercentage topRight, ILengthOrPercentage bottomRight, ILengthOrPercentage bottomLeft) {

            TopLeft = topLeft;
            TopRight = topRight;
            BottomLeft = bottomLeft;
            BottomRight = bottomRight;

        }

        public IEnumerator<ILengthOrPercentage> GetEnumerator() {

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