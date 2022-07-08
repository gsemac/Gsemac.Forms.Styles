using Gsemac.Data.ValueConversion;
using System;
using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion {

    internal class StringToColorConverter :
        ValueConverterBase<string, Color> {

        // Public members

        public override bool TryConvert(string value, out Color result) {

            result = default;

            if (value is null)
                return false;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            value = value.ToLowerInvariant()
                .Replace("grey", "gray");

            switch (value) {

                case Keyword.CanvasText:
                    result = SystemColorPalette.Default.CanvasText;
                    return true;

                case Keyword.Initial:
                    // Ideally, we should never end up trying to access a color property with the value "initial"
                    result = Color.Black;
                    return true;

                default:

                    try {

                        result = ColorTranslator.FromHtml(value);

                    }
                    catch (Exception) {

                        return false;

                    }

                    return true;

            }

        }

    }

}