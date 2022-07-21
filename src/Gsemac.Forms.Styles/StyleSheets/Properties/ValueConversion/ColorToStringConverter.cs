using Gsemac.Data.ValueConversion;
using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion {

    internal class ColorToStringConverter :
        ValueConverterBase<Color, string> {

        // Public members

        public override bool TryConvert(Color value, out string result) {

            result = ColorTranslator.ToHtml(value).ToLowerInvariant();

            // Treat "empty" colors the same as black.

            if (string.IsNullOrWhiteSpace(result))
                result = ColorTranslator.ToHtml(Color.Black).ToLowerInvariant();

            return true;

        }

    }

}