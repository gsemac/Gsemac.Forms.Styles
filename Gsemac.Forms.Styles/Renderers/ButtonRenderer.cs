using Gsemac.Forms.Styles.Extensions;
using Gsemac.Forms.Styles.StyleSheets;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public class ButtonRenderer :
        ControlRendererBase<Button> {

        // Public members

        public override void PaintControl(Button control, ControlPaintArgs e) {

            TextFormatFlags textFormatFlags = RenderUtilities.GetTextFormatFlags(control.TextAlign);

            e.PaintBackground();
            e.PaintText(textFormatFlags);
            e.PaintBorder();

        }

    }

}