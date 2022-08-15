using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles {

    public interface IStyleManager :
        IDisposable {

        ICollection<IStyleSheet> StyleSheets { get; }

        void ApplyStyles();
        void ResetStyles();

    }

}