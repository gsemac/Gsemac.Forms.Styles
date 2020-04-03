using Gsemac.Forms.Styles.StyleSheets;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Controls {

    public class ButtonControlRenderer :
        ControlRendererBase<Button> {

        // Public members

        public ButtonControlRenderer(IStyleSheet styleSheet) :
                base(styleSheet) {
        }

        public override void RenderControl(Graphics graphics, Button control) {

            PaintBackground(graphics, control);
            PaintForeground(graphics, control, GetTextFormatFlags(control.TextAlign));

        }

    }

}