using Gsemac.Data.ValueConversion;
using Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public class BorderColors :
        IEnumerable<Color> {

        // Public members

        public Color Top { get; } = Color.Black; // currentcolor
        public Color Right { get; } = Color.Black; // currentcolor
        public Color Bottom { get; } = Color.Black; // currentcolor
        public Color Left { get; } = Color.Black; // currentcolor

        public BorderColors(Color top, Color right, Color bottom, Color left) {

            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;

        }
        public BorderColors(Color top, Color leftAndRight, Color bottom) :
            this(top, leftAndRight, bottom, leftAndRight) {
        }
        public BorderColors(Color topAndBottom, Color leftAndRight) :
         this(topAndBottom, leftAndRight, topAndBottom, leftAndRight) {
        }
        public BorderColors(Color color) :
            this(color, color, color, color) {
        }

        public override string ToString() {

            var converter = StyleValueConverterFactory.Default.Create<Color, string>();

            if (Top.Equals(Right) && Top.Equals(Bottom) && Top.Equals(Left))
                return converter.Convert(Top);

            if (Top.Equals(Bottom) && Left.Equals(Right))
                return $"{converter.Convert(Top)} {converter.Convert(Left)}";

            if (Left.Equals(Right))
                return $"{converter.Convert(Top)} {converter.Convert(Left)} {converter.Convert(Bottom)}";

            return $"{converter.Convert(Top)} {converter.Convert(Right)} {converter.Convert(Bottom)} {converter.Convert(Left)}";

        }

        public IEnumerator<Color> GetEnumerator() {

            yield return Top;
            yield return Right;
            yield return Bottom;
            yield return Left;

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

    }

}