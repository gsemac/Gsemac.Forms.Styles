using System.Collections.Generic;

namespace Gsemac.Collections.Specialized {

    public interface IObservableSet<T> :
        IObservableCollection<T>,
        ISet<T> {
    }

}