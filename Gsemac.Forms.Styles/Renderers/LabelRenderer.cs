using Gsemac.Forms.Utilities;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {
    public class LabelRenderer :
        ControlRendererBase<Label> {

        // Public members

        public override void PaintControl(Label control, ControlPaintArgs e) {

            e.PaintBackground();
            e.PaintText(ControlUtilities.GetTextFormatFlags(control.TextAlign) | TextFormatFlags.WordBreak);
            e.PaintBorder();

        }

    }

}