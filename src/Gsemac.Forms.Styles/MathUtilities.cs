using System;

namespace Gsemac.Forms.Styles {

    public static class MathUtilities {

        // Public members

        public static double GradiansToDegrees(double gradians) {

            return gradians * (180 / 200);

        }
        public static double RadiansToDegrees(double radians) {

            return radians * (180 / Math.PI);

        }
        public static double TurnsToDegrees(double turns) {

            return turns * 360;

        }

    }

}