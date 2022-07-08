using System.Collections;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public sealed class BorderRadii :
        IEnumerable<ILengthPercentage> {

        public ILengthPercentage TopLeft { get; } = Length.Zero;
        public ILengthPercentage TopRight { get; } = Length.Zero;
        public ILengthPercentage BottomRight { get; } = Length.Zero;
        public ILengthPercentage BottomLeft { get; } = Length.Zero;

        public BorderRadii() {
        }

        public BorderRadii(ILengthPercentage value) {

            TopLeft = value;
            TopRight = value;
            BottomLeft = value;
            BottomRight = value;

        }
        public BorderRadii(ILengthPercentage topLeftBottomRight, ILengthPercentage topRightBottomLeft) {

            TopLeft = topLeftBottomRight;
            TopRight = topRightBottomLeft;
            BottomLeft = topRightBottomLeft;
            BottomRight = topLeftBottomRight;

        }
        public BorderRadii(ILengthPercentage topLeft, ILengthPercentage topRightBottomLeft, ILengthPercentage bottomRight) {

            TopLeft = topLeft;
            TopRight = topRightBottomLeft;
            BottomLeft = topRightBottomLeft;
            BottomRight = bottomRight;

        }
        public BorderRadii(ILengthPercentage topLeft, ILengthPercentage topRight, ILengthPercentage bottomRight, ILengthPercentage bottomLeft) {

            TopLeft = topLeft;
            TopRight = topRight;
            BottomLeft = bottomLeft;
            BottomRight = bottomRight;

        }

        public BorderRadii(int value) :
            this(new Length(value)) {
        }
        public BorderRadii(int topLeftBottomRight, int topRightBottomLeft) :
           this(new Length(topLeftBottomRight), new Length(topRightBottomLeft)) {
        }
        public BorderRadii(int topLeft, int topRightBottomLeft, int bottomRight) :
           this(new Length(topLeft), new Length(topRightBottomLeft), new Length(bottomRight)) {
        }
        public BorderRadii(int topLeft, int topRight, int bottomRight, int bottomLeft) :
            this(new Length(topLeft), new Length(topRight), new Length(bottomRight), new Length(bottomLeft)) {
        }

        public IEnumerator<ILengthPercentage> GetEnumerator() {

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