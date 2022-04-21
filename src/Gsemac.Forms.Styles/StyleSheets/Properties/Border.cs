using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public class Border {

        // Public members

        public IMeasurement Width { get; } = DefaultWidth;
        public BorderStyle Style { get; } = DefaultStyle;
        public Color Color { get; } = DefaultColor;

        public Border() {
        }
        public Border(IMeasurement width, BorderStyle style, Color color) {

            if (width is null)
                throw new ArgumentNullException(nameof(width));

            Width = width;
            Style = style;
            Color = color;

        }

        // Private members

        public static readonly IMeasurement DefaultWidth = new Measurement(0, Measurement.Pixels);
        public const BorderStyle DefaultStyle = BorderStyle.None;
        public static readonly Color DefaultColor = Color.Black;

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