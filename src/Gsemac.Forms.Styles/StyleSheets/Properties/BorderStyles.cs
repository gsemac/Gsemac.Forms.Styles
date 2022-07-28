using Gsemac.Data.ValueConversion;
using Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion;
using System.Collections;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public class BorderStyles :
        IEnumerable<BorderStyle> {

        // Public members

        public BorderStyle Top { get; } = BorderStyle.None;
        public BorderStyle Right { get; } = BorderStyle.None;
        public BorderStyle Bottom { get; } = BorderStyle.None;
        public BorderStyle Left { get; } = BorderStyle.None;

        public BorderStyles(BorderStyle top, BorderStyle right, BorderStyle bottom, BorderStyle left) {

            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;

        }
        public BorderStyles(BorderStyle top, BorderStyle leftAndRight, BorderStyle bottom) :
            this(top, leftAndRight, bottom, leftAndRight) {
        }
        public BorderStyles(BorderStyle topAndBottom, BorderStyle leftAndRight) :
         this(topAndBottom, leftAndRight, topAndBottom, leftAndRight) {
        }
        public BorderStyles(BorderStyle borderStyle) :
            this(borderStyle, borderStyle, borderStyle, borderStyle) {
        }

        public override string ToString() {

            var converter = StyleValueConverterFactory.Default.Create<BorderStyle, string>();

            if (Top.Equals(Right) && Top.Equals(Bottom) && Top.Equals(Left))
                return converter.Convert(Top);

            if (Top.Equals(Bottom) && Left.Equals(Right))
                return $"{converter.Convert(Top)} {converter.Convert(Left)}";

            if (Left.Equals(Right))
                return $"{converter.Convert(Top)} {converter.Convert(Left)} {converter.Convert(Bottom)}";

            return $"{converter.Convert(Top)} {converter.Convert(Right)} {converter.Convert(Bottom)} {converter.Convert(Left)}";

        }

        public IEnumerator<BorderStyle> GetEnumerator() {

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