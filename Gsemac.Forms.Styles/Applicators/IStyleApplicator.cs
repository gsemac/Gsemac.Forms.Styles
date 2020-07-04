using System;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators {

    [Flags]
    public enum ControlStyleOptions {
        None = 0,
        RulesRequired = 1,
        Recursive = 2,
        Default = RulesRequired | Recursive
    }

    public interface IStyleApplicator {

        void ApplyStyles(Control control, ControlStyleOptions options = ControlStyleOptions.Default);
        void ClearStyles(Control control, ControlStyleOptions options = ControlStyleOptions.Default);

    }

}