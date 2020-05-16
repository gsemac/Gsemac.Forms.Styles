using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Gsemac.Forms.Styles.StyleSheets {

    public static class PropertyUtilities {

        public static Color ParseColor(string input) {

            return ColorTranslator.FromHtml(input);

        }
        public static double ParseNumber(string input) {

            if (input.EndsWith("px", StringComparison.OrdinalIgnoreCase))
                input = input.Substring(0, input.IndexOf("px", StringComparison.OrdinalIgnoreCase));

            return double.Parse(input);

        }

        public static Color Rgb(int r, int g, int b) {

            return Color.FromArgb(r, g, b);

        }

    }

}