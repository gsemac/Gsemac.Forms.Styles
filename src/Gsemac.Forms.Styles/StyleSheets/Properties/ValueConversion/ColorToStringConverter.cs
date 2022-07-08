using Gsemac.Data.ValueConversion;
using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion {

    internal class ColorToStringConverter :
        ValueConverterBase<Color, string> {

        // Public members

        public override bool TryConvert(Color value, out string result) {

            result = ColorTranslator.ToHtml(value).ToLowerInvariant();

            return true;

        }

    }

}