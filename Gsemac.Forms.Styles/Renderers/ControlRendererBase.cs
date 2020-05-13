using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public abstract class ControlRendererBase :
        IControlRenderer {

        public abstract void RenderControl(Graphics graphics, Control control);

    }

    public abstract class ControlRendererBase<T> :
        ControlRendererBase,
        IControlRenderer<T> where T : Control {

        public override void RenderControl(Graphics graphics, Control control) {

            if (control is T castedControl)
                RenderControl(graphics, castedControl);
            else
                throw new ArgumentException("The given control cannot be rendered by this renderer.");

        }
        public abstract void RenderControl(Graphics graphics, T control);

    }

}