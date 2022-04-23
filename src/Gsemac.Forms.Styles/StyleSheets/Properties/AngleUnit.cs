using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public static class AngleUnit {

        // Public members

        public const string Degree = "deg";
        public const string Gradian = "grad";
        public const string Radian = "rad";
        public const string Turn = "turn";

        // Internal members

        internal static IEnumerable<string> GetUnits() {

            return new[] {
                Degree,
                Gradian,
                Radian,
                Turn,
            };

        }

    }

}