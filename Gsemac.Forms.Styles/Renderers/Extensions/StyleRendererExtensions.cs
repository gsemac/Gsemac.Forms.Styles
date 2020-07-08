using Gsemac.Forms.Styles.StyleSheets;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Gsemac.Forms.Styles.Renderers.Extensions {

    public static class StyleRendererExtensions {

        public static void PaintParentBackground(this IStyleRenderer styleRenderer, Graphics graphics, Rectangle rectangle, Rectangle parentRectangle, IRuleset parentRuleset) {

            Rectangle drawRect = new Rectangle(parentRectangle.X + rectangle.X, parentRectangle.Y + rectangle.Y, parentRectangle.Width, parentRectangle.Height);

            Region oldClippingRegion = graphics.Clip;
            Region clippingRegion = new Region();

            clippingRegion.Intersect(oldClippingRegion);
            clippingRegion.Intersect(rectangle);

            graphics.SetClip(clippingRegion, CombineMode.Replace);
            graphics.TranslateTransform(-rectangle.X, -rectangle.Y);

            styleRenderer.PaintBackground(graphics, drawRect, parentRuleset);

            graphics.TranslateTransform(rectangle.X, rectangle.Y);
            graphics.SetClip(oldClippingRegion, CombineMode.Replace);

        }

    }

}