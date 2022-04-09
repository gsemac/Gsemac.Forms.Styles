using System.Collections.Generic;

namespace Gsemac.Collections.Specialized {

    public interface IObservableList<T> :
        IObservableCollection<T>,
        IList<T> {
    }

}