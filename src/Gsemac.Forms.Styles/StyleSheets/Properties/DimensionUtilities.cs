using Gsemac.Forms.Styles.Properties;
using Gsemac.Forms.Styles.StyleSheets.Dom;
using System;
using System.Drawing;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public static class DimensionUtilities {

        // Public members

        public static double ToDegrees(IDimension dimension) {

            if (dimension is null)
                throw new ArgumentNullException(nameof(dimension));

            switch (dimension.Unit) {

                case AngleUnit.Degree:
                    return dimension.Value;

                case AngleUnit.Gradian:
                    return MathUtilities.GradiansToDegrees(dimension.Value);

                case AngleUnit.Radian:
                    return MathUtilities.RadiansToDegrees(dimension.Value);

                case AngleUnit.Turn:
                    return MathUtilities.TurnsToDegrees(dimension.Value);

                default:
                    throw new InvalidOperationException(string.Format(ExceptionMessages.UnitCannotBeConvertedToDegrees, dimension.Unit));

            }

        }

        public static int ToPixels(IDimension dimension, INode2 node, Rectangle viewport) {

            return ToPixels(dimension, node, viewport, DefaultDpi);

        }
        public static int ToPixels(IDimension dimension, INode2 node, Rectangle viewport, int dpi) {

            if (dimension is null)
                throw new ArgumentNullException(nameof(dimension));

            switch (dimension.Unit) {

                case LengthUnit.Pixel:
                    return (int)dimension.Value;

                default:
                    throw new InvalidOperationException(string.Format(ExceptionMessages.UnitCannotBeConvertedToPixels, dimension.Unit));

            }

        }

        // Private members

        private const int DefaultDpi = 96; // https://stackoverflow.com/a/7619569/5383169 (ShadyKiller)

    }

}