using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Collections.Specialized {

    public abstract class ObservableCollectionBase<T> :
        CollectionBase<T>,
        IObservableCollection<T> {

        // Public members

        public event EventHandler<CollectionChangedEventArgs<T>> CollectionChanged;

        public override void Add(T item) {

            base.Add(item);

            OnCollectionChanged(new CollectionChangedEventArgs<T>(CollectionChangedAction.Add, new[] { item }, Items.Count - 1));

        }
        public override bool Remove(T item) {

            bool removed = base.Remove(item);

            if (removed)
                OnCollectionChanged(new CollectionChangedEventArgs<T>(CollectionChangedAction.Remove, new[] { item }));

            return removed;

        }
        public override void Clear() {

            if (Items.Count <= 0)
                return;

            IEnumerable<T> items = Items.ToArray();

            base.Clear();

            OnCollectionChanged(new CollectionChangedEventArgs<T>(CollectionChangedAction.Reset, items));

        }

        // Protected members

        protected ObservableCollectionBase() {
        }
        protected ObservableCollectionBase(ICollection<T> baseCollection) :
             base(baseCollection) {
        }
        protected ObservableCollectionBase(IEnumerable<T> items) :
            base(items) {
        }

        protected void OnCollectionChanged(CollectionChangedEventArgs<T> e) {

            CollectionChanged?.Invoke(this, e);

        }

    }

}