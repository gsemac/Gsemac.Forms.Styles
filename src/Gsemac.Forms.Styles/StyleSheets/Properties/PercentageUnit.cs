using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public static class PercentageUnit {

        // Public members

        public const string Percent = "%";

        // Internal members

        internal static IEnumerable<string> GetUnits() {

            return new[] {
                Percent,
            };

        }

    }

}