using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion {

    internal class ColorToStringConverter :
        ValueConverterBase<Color, string> {

        // Public members

        public override string Convert(Color value) {

            return ColorTranslator.ToHtml(value).ToLowerInvariant();

        }

    }

}