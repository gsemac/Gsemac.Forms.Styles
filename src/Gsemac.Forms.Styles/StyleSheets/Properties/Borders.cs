using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public class Borders :
        IEnumerable<Border> {

        // Public members

        public Border Top { get; } = new Border();
        public Border Right { get; } = new Border();
        public Border Bottom { get; } = new Border();
        public Border Left { get; } = new Border();

        public Borders() {
        }
        public Borders(Border top, Border right, Border bottom, Border left) {

            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;

        }
        public Borders(IMeasurement width, BorderStyle style, Color color) {

            Top = new Border(width, style, color);
            Right = new Border(width, style, color);
            Bottom = new Border(width, style, color);
            Left = new Border(width, style, color);

        }

        public IEnumerator<Border> GetEnumerator() {

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