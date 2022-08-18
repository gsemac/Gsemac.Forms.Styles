using System.Drawing;

namespace Gsemac.Forms.Styles {

    internal static class ColorUtilities2 {

        public static bool AreEqual(Color color1, Color color2) {

            // Color.Equals takes the name of the color into account, whereas this method just compares their ARGB properties.

            return color1.A.Equals(color2.A) &&
                color1.R.Equals(color2.R) &&
                color1.G.Equals(color2.G) &&
                color1.B.Equals(color2.B);

        }

    }

}