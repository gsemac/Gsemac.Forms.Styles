using System.Collections;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public sealed class BorderWidths :
        IEnumerable<LineWidth> {

        // Public members

        public LineWidth Top { get; } = LineWidth.Medium;
        public LineWidth Right { get; } = LineWidth.Medium;
        public LineWidth Bottom { get; } = LineWidth.Medium;
        public LineWidth Left { get; } = LineWidth.Medium;

        public BorderWidths() {
        }
        public BorderWidths(LineWidth value) {

            Top = value;
            Right = value;
            Bottom = value;
            Left = value;

        }
        public BorderWidths(LineWidth vertical, LineWidth horizontal) {

            Top = horizontal;
            Right = vertical;
            Bottom = horizontal;
            Left = vertical;

        }
        public BorderWidths(LineWidth top, LineWidth horizontal, LineWidth bottom) {

            Top = top;
            Right = horizontal;
            Bottom = bottom;
            Left = horizontal;

        }
        public BorderWidths(LineWidth top, LineWidth right, LineWidth bottom, LineWidth left) {

            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;

        }

        public BorderWidths(int value) :
            this(new LineWidth(value)) {
        }
        public BorderWidths(int vertical, int horizontal) :
           this(new LineWidth(vertical), new LineWidth(horizontal)) {
        }
        public BorderWidths(int top, int horizontal, int bottom) :
           this(new LineWidth(top), new LineWidth(horizontal), new LineWidth(bottom)) {
        }
        public BorderWidths(int top, int right, int bottom, int left) :
            this(new LineWidth(top), new LineWidth(right), new LineWidth(bottom), new LineWidth(left)) {
        }

        public IEnumerator<LineWidth> GetEnumerator() {

            yield return Top;
            yield return Right;
            yield return Bottom;
            yield return Left;

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        public override string ToString() {

            if (Top.Equals(Right) && Top.Equals(Bottom) && Top.Equals(Left))
                return $"{Top}";

            if (Top.Equals(Bottom) && Left.Equals(Right))
                return $"{Top} {Left}";

            if (Left.Equals(Right))
                return $"{Top} {Left} {Bottom}";

            return $"{Top} {Right} {Bottom} {Left}";

        }

    }

}