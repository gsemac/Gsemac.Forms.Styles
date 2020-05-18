using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Gsemac.Forms.Styles.StyleSheets {

    public static class PropertyUtilities {

        public static bool TryParseColor(string input, out Color result) {

            try {

                input = input.Replace("grey", "gray");

                result = ColorTranslator.FromHtml(input);

                return true;

            }
            catch (Exception) {

                result = default;

                return false;

            }

        }
        public static bool TryParseNumber(string input, out double result) {

            if (input.EndsWith("px", StringComparison.OrdinalIgnoreCase))
                input = input.Substring(0, input.IndexOf("px", StringComparison.OrdinalIgnoreCase));

            return double.TryParse(input, out result);

        }
        public static bool TryParseBorderStyle(string input, out BorderStyle result) {

            switch (input?.Trim()?.ToLowerInvariant()) {

                case "dotted":
                    result = BorderStyle.Dotted;
                    break;

                case "dashed":
                    result = BorderStyle.Dashed;
                    break;

                case "solid":
                    result = BorderStyle.Solid;
                    break;

                case "double":
                    result = BorderStyle.Double;
                    break;

                case "groove":
                    result = BorderStyle.Groove;
                    break;

                case "ridge":
                    result = BorderStyle.Ridge;
                    break;

                case "inset":
                    result = BorderStyle.Inset;
                    break;

                case "outset":
                    result = BorderStyle.Outset;
                    break;

                case "none":
                    result = BorderStyle.None;
                    break;

                case "hidden":
                    result = BorderStyle.Hidden;
                    break;

                default:
                    result = BorderStyle.None;
                    return false;

            }

            return true;

        }

        public static Color ParseColor(string input) {

            if (TryParseColor(input, out Color result))
                return result;
            else
                throw new ArgumentException(nameof(input));

        }
        public static double ParseNumber(string input) {

            if (TryParseNumber(input, out double result))
                return result;
            else
                throw new ArgumentException(nameof(input));

        }
        public static BorderStyle ParseBorderStyle(string input) {

            if (TryParseBorderStyle(input, out BorderStyle result))
                return result;
            else
                throw new ArgumentException(nameof(input));

        }

        public static Color Rgb(int r, int g, int b) {

            return Color.FromArgb(r, g, b);

        }

    }

}