using Gsemac.Forms.Styles.Extensions;
using Gsemac.Forms.Utilities;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public class ButtonRenderer :
        ControlRendererBase<Button> {

        // Public members

        public override void PaintControl(Button control, ControlPaintArgs e) {

            TextFormatFlags textFormatFlags = ControlUtilities.GetTextFormatFlags(control.TextAlign);

            e.Clear();

            e.PaintBackground();

            if (control.Image != null) {

                const int horizontalPadding = 4;
                const int verticalPadding = 4;

                Rectangle imageRect = new Rectangle(horizontalPadding, verticalPadding, control.Width - horizontalPadding * 2, control.Height - verticalPadding * 2);

                e.Graphics.DrawImage(control.Image, imageRect, control.ImageAlign);

            }

            e.PaintText(textFormatFlags);

            e.PaintBorder();

        }

    }

}