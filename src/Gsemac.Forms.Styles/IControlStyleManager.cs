using System.Windows.Forms;

namespace Gsemac.Forms.Styles {

    public interface IControlStyleManager :
        IStyleManager {

        void ApplyStyles(Control control);
        void ResetStyles(Control control);

    }

}