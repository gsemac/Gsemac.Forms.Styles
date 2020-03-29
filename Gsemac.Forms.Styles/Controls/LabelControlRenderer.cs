using Gsemac.Forms.Styles.Controls;
using Gsemac.Forms.Styles.StyleSheets;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Controls {
    public class LabelControlRenderer :
        ControlRendererBase {

        // Public members

        public LabelControlRenderer(IStyleSheet styleSheet) :
            base(styleSheet) {
        }

        public void RenderControl(Graphics graphics, Label control) {

            PaintBackground(graphics, control);
            PaintForeground(graphics, control);

        }

    }

}