using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles {

    public interface IStyleManager :
        IDisposable {

        ICollection<IStyleSheet> StyleSheets { get; }

        void ApplyStyles();
        void ApplyStyles(Control control);
        void ResetStyles();
        void ResetStyles(Control control);

    }

}