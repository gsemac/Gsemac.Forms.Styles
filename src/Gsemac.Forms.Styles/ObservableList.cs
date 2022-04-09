using System.Collections.Generic;

namespace Gsemac.Collections.Specialized {

    public class ObservableList<T> :
        ObservableCollectionBase<T>,
        IObservableList<T> {

        // Public members

        public T this[int index] {
            get => Items[index];
            set => Items[index] = value;
        }

        public ObservableList() {
        }
        public ObservableList(IEnumerable<T> items) :
            base(new List<T>(items)) {
        }
        public ObservableList(int capacity) :
            base(new List<T>(capacity)) {
        }

        public int IndexOf(T item) {

            return Items.IndexOf(item);

        }
        public void Insert(int index, T item) {

            Items.Insert(index, item);

            OnCollectionChanged(new CollectionChangedEventArgs<T>(CollectionChangedAction.Add, new[] { item }, index));

        }
        public void RemoveAt(int index) {

            T itemAtIndex = index >= 0 && index < Items.Count ?
                Items[index] :
                default;

            Items.RemoveAt(index);

            OnCollectionChanged(new CollectionChangedEventArgs<T>(CollectionChangedAction.Remove, new[] { itemAtIndex }, index));

        }

        // Protected members

        protected new IList<T> Items => (IList<T>)base.Items;

    }

}