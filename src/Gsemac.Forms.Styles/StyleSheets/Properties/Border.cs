using Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion;
using System;
using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public sealed class Border {

        // Public members

        public LineWidth Width { get; } = LineWidth.Medium;
        public BorderStyle Style { get; } = BorderStyle.None;
        public Color Color { get; } = Color.Black; // currentcolor

        public Border() {
        }
        public Border(Color color) {

            Color = color;

        }
        public Border(LineWidth width, BorderStyle style, Color color) {

            if (width is null)
                throw new ArgumentNullException(nameof(width));

            Width = width;
            Style = style;
            Color = color;

        }

        public override string ToString() {

            return $"{Width} {new BorderStyleToStringConverter().Convert(Style)} {new ColorToStringConverter().Convert(Color)}";

        }

    }

}