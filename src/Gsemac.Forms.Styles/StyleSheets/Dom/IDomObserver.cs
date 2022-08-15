using System;

namespace Gsemac.Forms.Styles.StyleSheets.Dom {

    public interface IDomObserver :
        IDisposable {

        event EventHandler<NodeCollectionChangedEventArgs> ChildAdded;
        event EventHandler<NodeCollectionChangedEventArgs> ChildRemoved;
        event EventHandler<NodeEventArgs> StylesChanged;
        event EventHandler<NodeEventArgs> SelectorChanged;

    }

}