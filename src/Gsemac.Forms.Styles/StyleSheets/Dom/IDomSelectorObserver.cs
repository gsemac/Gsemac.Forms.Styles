using System;

namespace Gsemac.Forms.Styles.StyleSheets.Dom {

    public interface IDomSelectorObserver :
        IDisposable {

        event EventHandler<NodeEventArgs> SelectorChanged;

        void Refresh();

    }

}