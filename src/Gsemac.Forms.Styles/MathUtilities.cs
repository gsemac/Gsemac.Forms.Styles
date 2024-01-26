using System;

namespace Gsemac.Forms.Styles {

    public static class MathUtilities {

        // Public members

        public static NumberT Clamp<NumberT>(NumberT value, NumberT min, NumberT max)
            where NumberT : IComparable {

            if (value.CompareTo(min) < 0)
                value = min;

            if (value.CompareTo(max) > 0)
                value = max;

            return value;

        }

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