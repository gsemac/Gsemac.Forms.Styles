using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public interface IControlRenderer {

        void InitializeControl(Control control);

        void PaintControl(Control control, ControlPaintArgs args);

    }

    public interface IControlRenderer<T> :
      IControlRenderer where T : Control {

        void InitializeControl(T control);

        void PaintControl(T control, ControlPaintArgs args);

    }

}