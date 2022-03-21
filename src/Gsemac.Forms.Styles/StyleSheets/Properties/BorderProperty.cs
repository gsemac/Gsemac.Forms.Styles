using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class Border {

        public const double DefaultWidth = 0.0;
        public const BorderStyle DefaultStyle = BorderStyle.Solid;
        public static readonly Color DefaultColor = Color.Black;

        public double Width { get; } = DefaultWidth;
        public BorderStyle Style { get; } = DefaultStyle;
        public Color Color { get; } = DefaultColor;

        public Border() {
        }
        public Border(double width, BorderStyle style, Color color) {

            this.Width = width;
            this.Style = style;
            this.Color = color;

        }

    }

    public class Borders :
        IEnumerable<Border> {

        public Border Top { get; } = new Border();
        public Border Right { get; } = new Border();
        public Border Bottom { get; } = new Border();
        public Border Left { get; } = new Border();

        public Borders() {
        }
        public Borders(Border top, Border right, Border bottom, Border left) {

            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
            this.Left = left;

        }
        public Borders(double width, BorderStyle style, Color color) {

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

    public class BorderProperty :
         PropertyBase<Border> {

        // Public members

        public BorderProperty(PropertyType type, Border value, bool inheritable = false) :
            base(type, value, inheritable) {
        }

    }

}