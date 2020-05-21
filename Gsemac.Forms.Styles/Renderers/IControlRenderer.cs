using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public interface IControlRenderer {

        void PaintControl(Control control, ControlPaintArgs args);

    }

    public interface IControlRenderer<T> :
      IControlRenderer where T : Control {

        void PaintControl(T control, ControlPaintArgs args);

    }

}