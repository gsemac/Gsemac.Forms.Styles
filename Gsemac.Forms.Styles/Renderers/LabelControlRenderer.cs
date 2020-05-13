using Gsemac.Forms.Styles.Extensions;
using Gsemac.Forms.Styles.StyleSheets;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {
    public class LabelControlRenderer :
        ControlRendererBase<Label> {

        // Public members

        public LabelControlRenderer(IStyleSheetControlRenderer baseRenderer) {

            this.baseRenderer = baseRenderer;

        }

        public override void RenderControl(Graphics graphics, Label control) {

            baseRenderer.PaintBackground(graphics, control);
            baseRenderer.PaintForeground(graphics, control);

        }

        // Private members

        private readonly IStyleSheetControlRenderer baseRenderer;

    }

}