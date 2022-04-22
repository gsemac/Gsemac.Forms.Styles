using Gsemac.Forms.Styles.Properties;
using System;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public static class Dimension {

        // Public members

        public static IDimension Parse(string input) {

            if (TryParse(input, out IDimension result))
                return result;

            throw new FormatException(string.Format(ExceptionMessages.MalformedDimensionValue, input));


        }
        public static bool TryParse(string input, out IDimension result) {

            result = null;

            if (Angle.TryParse(input, out Angle parsedAngle)) {

                result = parsedAngle;

                return true;

            }

            if (Length.TryParse(input, out Length parsedLength)) {

                result = parsedLength;

                return true;

            }

            return false;

        }

    }

}