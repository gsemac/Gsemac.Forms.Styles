using Gsemac.Forms.Styles.Extensions;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public class PictureBoxRenderer :
        ControlRendererBase<PictureBox> {

        public override void PaintControl(PictureBox control, ControlPaintArgs args) {

            args.PaintBackground();

            if (control.Image != null)
                args.Graphics.DrawImage(control.Image, new Rectangle(0, 0, control.Width, control.Height), (ImageSizeMode)control.SizeMode);

            args.PaintBorder();

        }

    }

}
