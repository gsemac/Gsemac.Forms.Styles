using Gsemac.Forms.Styles.StyleSheets;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.Dom {

    public interface IDom {

        IDomNode Root { get; }
        ICollection<IStyleSheet> StyleSheets { get; }

        void SuspendStyleUpdates();
        void ResumeStyleUpdates();

    }

}