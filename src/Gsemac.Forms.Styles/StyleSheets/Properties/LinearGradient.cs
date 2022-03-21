using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class LinearGradient :
        GradientBase {

        // Public members

        public LinearGradient(double degrees, Color[] colorStops) {

            Debug.Assert(colorStops.Any());

            this.degrees = degrees;
            this.colorStops = colorStops;

        }

        public override void DrawGradient(Graphics graphics, Rectangle rect) {

            if (colorStops.Count() > 1) {

                // 90 degrees is Right, but Up for CSS.

                using (LinearGradientBrush brush = new LinearGradientBrush(rect, Color.Black, Color.Black, (float)degrees - 90.0f)) {

                    ColorBlend colorBlend = new ColorBlend {
                        Colors = colorStops
                    };

                    colorBlend.Positions = Enumerable.Range(0, colorStops.Count())
                         .Select(i => (float)i / (colorStops.Count() - 1))
                         .ToArray();

                    brush.InterpolationColors = colorBlend;

                    graphics.FillRectangle(brush, rect);

                }

            }
            else if (colorStops.Count() == 1) {

                // Draw a solid rectangle if there is only one color.

                using (SolidBrush brush = new SolidBrush(colorStops.First()))
                    graphics.FillRectangle(brush, rect);

            }

        }

        // Private members

        private readonly double degrees;
        private readonly Color[] colorStops;

    }

}