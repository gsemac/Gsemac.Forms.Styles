using System;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public abstract class ControlRendererBase :
        IControlRenderer {

        // Public members

        public virtual void InitializeControl(Control control) { }

        public abstract void PaintControl(Control control, ControlPaintArgs args);

    }

    public abstract class ControlRendererBase<T> :
        ControlRendererBase,
        IControlRenderer<T> where T : Control {

        // Public members

        public override void InitializeControl(Control control) {

            InitializeControl(GetDowncastedControlOrThrow(control));

        }
        public virtual void InitializeControl(T control) { }

        public override void PaintControl(Control control, ControlPaintArgs args) {

            PaintControl(GetDowncastedControlOrThrow(control), args);

        }
        public abstract void PaintControl(T control, ControlPaintArgs args);

        // Private members

        private T GetDowncastedControlOrThrow(Control control) {

            if (control is T castedControl)
                return castedControl;
            else
                throw new ArgumentException("The renderer does not support this type of control.");

        }

    }

}