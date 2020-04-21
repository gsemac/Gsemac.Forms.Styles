using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.Utilities {

    public static class PropertyUtilities {

        public static Color ParseColor(string input) {

            return ColorTranslator.FromHtml(input);

        }
        public static double ParseNumber(string input) {

            if (input.EndsWith("px", System.StringComparison.OrdinalIgnoreCase))
                input = input.Substring(0, input.IndexOf("px", System.StringComparison.OrdinalIgnoreCase));

            return double.Parse(input);

        }

    }

}