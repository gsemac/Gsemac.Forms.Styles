using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public static class LengthUnit {

        // Public members

        public const string Centimeter = "cm";
        public const string Inch = "in";
        public const string Millimeter = "mm";
        public const string Pica = "pc";
        public const string Pixel = "px";
        public const string Point = "pt";

        public const string Em = "em";
        public const string RootElement = "rem";
        public const string ViewportHeight = "vh";
        public const string ViewportMaximum = "vmax";
        public const string ViewportMinimum = "vmin";
        public const string ViewportWidth = "vw";
        public const string XHeight = "ex";
        public const string ZeroWidth = "ch";

        // Internal members

        internal static IEnumerable<string> GetUnits() {

            return new[] {
                Centimeter,
                Inch,
                Millimeter,
                Pica,
                Pixel,
                Point,
                Em,
                RootElement,
                ViewportHeight,
                ViewportMaximum,
                ViewportMinimum,
                ViewportWidth,
                XHeight,
                ZeroWidth,
            };

        }

    }

}