using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators {

    public interface IStyleApplicator {

        void ApplyStyles();
        void ClearStyles();

        void ApplyStyles(Control control, IStyleApplicationOptions options = null);
        void ClearStyles(Control control, IStyleApplicationOptions options = null);

    }

}