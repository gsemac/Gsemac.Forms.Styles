using Gsemac.Data.ValueConversion;
using System;

namespace Gsemac.Forms.Styles.StyleSheets.Properties.ValueConversion {

    internal class StringToLineWidthConverter :
        ValueConverterBase<string, LineWidth> {

        // Public members

        public override bool TryConvert(string value, out LineWidth result) {

            result = default;

            if (value is null)
                return false;

            switch (value.Trim().ToLowerInvariant()) {

                case "thin":
                    result = LineWidth.Thin;
                    return true;

                case "medium":
                    result = LineWidth.Medium;
                    return true;

                case "thick":
                    result = LineWidth.Thick;
                    return true;

                default:

                    if (new StringToLengthConverter().TryConvert(value, out Length length)) {

                        result = new LineWidth(length);

                        return true;

                    }

                    return false;

            }

        }

    }

}