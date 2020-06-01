using System;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public abstract class ControlRendererBase :
        IControlRenderer {

        public abstract void PaintControl(Control control, ControlPaintArgs args);

    }

    public abstract class ControlRendererBase<T> :
        ControlRendererBase,
        IControlRenderer<T> where T : Control {

        public override void PaintControl(Control control, ControlPaintArgs args) {

            if (control is T castedControl)
                PaintControl(castedControl, args);
            else
                throw new ArgumentException("The given control cannot be rendered by this renderer.");

        }
        public abstract void PaintControl(T control, ControlPaintArgs args);

    }

}