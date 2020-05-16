using Gsemac.Forms.Styles.Extensions;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public class ButtonControlRenderer :
        ControlRendererBase<Button> {

        // Public members

        public ButtonControlRenderer(IStyleSheetControlRenderer baseRenderer) {

            this.baseRenderer = baseRenderer;

        }

        public override void RenderControl(Graphics graphics, Button control) {

            baseRenderer.PaintBackground(graphics, control);
            baseRenderer.PaintForeground(graphics, control, RenderUtilities.GetTextFormatFlags(control.TextAlign));

        }

        // Private members

        private readonly IStyleSheetControlRenderer baseRenderer;

    }

}