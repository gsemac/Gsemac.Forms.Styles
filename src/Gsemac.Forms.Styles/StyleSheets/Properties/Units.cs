using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public static class Units {

        // Public members

        // angle units

        public const string Degree = "deg";
        public const string Gradian = "grad";
        public const string Radian = "rad";
        public const string Turn = "turn";

        // length units

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

        // percentage units

        public const string Percent = "%";

        // Internal members

        internal static IEnumerable<string> GetAngleUnits() {

            return new[] {
                Degree,
                Gradian,
                Radian,
                Turn,
            };

        }
        internal static IEnumerable<string> GetLengthUnits() {

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
        internal static IEnumerable<string> GetPercentageUnits() {

            return new[] {
                Percent,
            };

        }

    }

}