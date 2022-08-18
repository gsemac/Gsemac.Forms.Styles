using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;
using System.Drawing;

namespace Gsemac.Forms.Styles.Renderers2 {

    internal class RenderContext :
        IRenderContext {

        // Public members

        public Rectangle ClientRectangle { get; }
        public Graphics Graphics { get; }
        public IRuleset Style { get; }

        public RenderContext(Graphics graphics, Rectangle clientRect, IRuleset style) {

            if (graphics is null)
                throw new ArgumentNullException(nameof(graphics));

            if (style is null)
                throw new ArgumentNullException(nameof(style));

            ClientRectangle = clientRect;
            Graphics = graphics;
            Style = style;

        }

    }

}