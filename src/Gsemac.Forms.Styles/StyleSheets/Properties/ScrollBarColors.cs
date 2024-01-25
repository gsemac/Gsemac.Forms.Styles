using Gsemac.Data.ValueConversion;
using Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public sealed class ScrollBarColors :
        IEnumerable<Color> {

        // Public members

        public Color Thumb { get; } = Color.Black;
        public Color Track { get; } = Color.Black;

        public ScrollBarColors(Color thumb, Color track) {

            Thumb = thumb;
            Track = track;

        }

        public override string ToString() {

            var converter = StyleValueConverterFactory.Default.Create<Color, string>();

            return $"{converter.Convert(Thumb)} {converter.Convert(Track)}";

        }

        public IEnumerator<Color> GetEnumerator() {

            return ((IEnumerable<Color>)(new[] {
                Thumb,
                Track,
            })).GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

    }

}