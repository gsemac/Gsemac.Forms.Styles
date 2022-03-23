using System;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators {

    public interface IStyleApplicator {

        void ApplyStyles();
        void ClearStyles();

        void ApplyStyles(Control control, IStyleOptions options = null);
        void ClearStyles(Control control, IStyleOptions options = null);

    }

}