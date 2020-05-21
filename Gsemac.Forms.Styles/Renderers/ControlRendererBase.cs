using System;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public abstract class ControlRendererBase :
        IControlRenderer {

        public abstract void PaintControl(Control control, ControlPaintArgs e);

    }

    public abstract class ControlRendererBase<T> :
        ControlRendererBase,
        IControlRenderer<T> where T : Control {

        public override void PaintControl(Control control, ControlPaintArgs e) {

            if (control is T castedControl)
                PaintControl(castedControl, e);
            else
                throw new ArgumentException("The given control cannot be rendered by this renderer.");

        }
        public abstract void PaintControl(T control, ControlPaintArgs args);

    }

}