using System;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion {

    internal class StringToLineWidthConverter :
        ValueConverterBase<string, LineWidth> {

        // Public members

        public override LineWidth Convert(string value) {

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            switch (value.Trim().ToLowerInvariant()) {

                case "thin":
                    return LineWidth.Thin;

                case "medium":
                    return LineWidth.Medium;

                case "thick":
                    return LineWidth.Thick;

                default:
                    return new LineWidth(new StringToLengthConverter().Convert(value));

            }

        }

    }

}