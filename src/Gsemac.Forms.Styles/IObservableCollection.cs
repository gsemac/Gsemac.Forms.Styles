using System;
using System.Collections.Generic;

namespace Gsemac.Collections.Specialized {

    public interface IObservableCollection<T> :
        ICollection<T> {

        event EventHandler<CollectionChangedEventArgs<T>> CollectionChanged;

    }

}