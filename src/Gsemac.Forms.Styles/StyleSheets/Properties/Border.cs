using System;
using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public sealed class Border {

        // Public members

        public IMeasurement Width { get; } = new Measurement(0, Measurement.Pixels);
        public BorderStyle Style { get; } = BorderStyle.None;
        public Color Color { get; } = Color.Black;

        public Border() {
        }
        public Border(Color color) {

            Color = color;

        }
        public Border(IMeasurement width, BorderStyle style, Color color) {

            if (width is null)
                throw new ArgumentNullException(nameof(width));

            Width = width;
            Style = style;
            Color = color;

        }

    }

}