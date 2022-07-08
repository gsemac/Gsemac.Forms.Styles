using Gsemac.Forms.Styles.Properties;
using System;
using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion {

    internal class StringToColorConverter :
        ValueConverterBase<string, Color> {

        // Public members

        public override Color Convert(string value) {

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(string.Format(ExceptionMessages.MalformedPropertyValueAsType, value, DestinationType), nameof(value));

            value = value.Replace("grey", "gray");

            switch (value.ToLowerInvariant()) {

                case Keyword.CanvasText:
                    return SystemColorPalette.Default.CanvasText;

            }

            return ColorTranslator.FromHtml(value);

        }

    }

}