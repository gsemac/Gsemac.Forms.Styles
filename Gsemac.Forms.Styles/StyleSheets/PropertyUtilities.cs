using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Gsemac.Forms.Styles.StyleSheets {

    public static class PropertyUtilities {

        public static Color ParseColor(string input) {

            string functionPattern = @"^(rgba?|hsla?)\((.+?)\)$";
            Match functionMatch = new Regex(functionPattern).Match(input);

            if (functionMatch.Success) {

                string functionName = functionMatch.Groups[1].Value;
                string[] functionArgs = functionMatch.Groups[2].Value.Split(',')
                    .Where(arg => !string.IsNullOrWhiteSpace(arg))
                    .ToArray();

                switch (functionName) {

                    case "rgb":
                        return Color.FromArgb(int.Parse(functionArgs[0]), int.Parse(functionArgs[1]), int.Parse(functionArgs[2]));

                    default:
                        throw new Exception($"Unrecognized function \"{functionName}\"");

                }

            }
            else
                return ColorTranslator.FromHtml(input);

        }
        public static double ParseNumber(string input) {

            if (input.EndsWith("px", StringComparison.OrdinalIgnoreCase))
                input = input.Substring(0, input.IndexOf("px", StringComparison.OrdinalIgnoreCase));

            return double.Parse(input);

        }

    }

}