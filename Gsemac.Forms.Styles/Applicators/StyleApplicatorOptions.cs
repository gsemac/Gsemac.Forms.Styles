using System;

namespace Gsemac.Forms.Styles.Applicators {

    [Flags]
    public enum StyleApplicatorOptions {
        None,
        Default = None,
        DisposeStyleSheet = 1,
        AddMessageFilter = 2
    }

}