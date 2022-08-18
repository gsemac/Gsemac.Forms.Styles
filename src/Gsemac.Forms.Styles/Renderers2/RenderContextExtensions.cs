using Gsemac.Forms.Styles.Renderers.Extensions;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers2 {

    internal static class RenderContextExtensions {

        // Public members

        public static void Clear(this IRenderContext context) {

            if (context is null)
                throw new ArgumentNullException(nameof(context));

            RenderUtilities.Clear(context.Graphics, context.Style);

        }

        public static void DrawBackground(this IRenderContext context) {

            DrawBackground(context, context.ClientRectangle);

        }
        public static void DrawBackground(this IRenderContext context, Rectangle bounds) {

            if (context is null)
                throw new ArgumentNullException(nameof(context));

            RenderUtilities.DrawBackground(context.Graphics, bounds, context.Style);

        }

        public static void DrawBorder(this IRenderContext context) {

            DrawBorder(context, context.ClientRectangle);

        }
        public static void DrawBorder(this IRenderContext context, Rectangle bounds) {

            if (context is null)
                throw new ArgumentNullException(nameof(context));

            RenderUtilities.DrawBorder(context.Graphics, bounds, context.Style);

        }

        public static void DrawImage(this IRenderContext context, Image image, Rectangle bounds) {

            if (context is null)
                throw new ArgumentNullException(nameof(context));

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            context.Graphics.DrawImage(image, bounds);


        }
        public static void DrawImage(this IRenderContext context, Image image, Rectangle bounds, ContentAlignment alignment) {

            if (context is null)
                throw new ArgumentNullException(nameof(context));

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            context.Graphics.DrawImage(image, bounds, alignment);

        }

        public static void DrawText(this IRenderContext context, string text, Font font) {

            if (context is null)
                throw new ArgumentNullException(nameof(context));

            RenderUtilities.DrawText(context.Graphics, context.ClientRectangle, text, font, context.Style);

        }
        public static void DrawText(this IRenderContext context, string text, Font font, TextFormatFlags textFormatFlags) {

            if (context is null)
                throw new ArgumentNullException(nameof(context));

            RenderUtilities.DrawText(context.Graphics, context.ClientRectangle, text, font, textFormatFlags, context.Style);

        }
        public static void DrawText(this IRenderContext context, Rectangle bounds, string text, Font font) {

            if (context is null)
                throw new ArgumentNullException(nameof(context));

            RenderUtilities.DrawText(context.Graphics, bounds, text, font, context.Style);

        }
        public static void DrawText(this IRenderContext context, Rectangle bounds, string text, Font font, TextFormatFlags textFormatFlags) {

            if (context is null)
                throw new ArgumentNullException(nameof(context));

            RenderUtilities.DrawText(context.Graphics, bounds, text, font, textFormatFlags, context.Style);

        }

    }

}