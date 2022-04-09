using System.Collections.Generic;

namespace Gsemac.Collections.Specialized {

    public class ObservableHashSet<T> :
        ObservableCollectionBase<T>,
        IObservableSet<T> {

        // Public members

        public ObservableHashSet() :
            base(new HashSet<T>()) {
        }
        public ObservableHashSet(IEnumerable<T> collection) :
            base(new HashSet<T>(collection)) {
        }
        public ObservableHashSet(IEqualityComparer<T> comparer) :
            base(new HashSet<T>(comparer)) {
        }
        public ObservableHashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer) :
            base(new HashSet<T>(collection, comparer)) {
        }

        public new bool Add(T item) {

            return ((ISet<T>)this).Add(item);

        }

        public void ExceptWith(IEnumerable<T> other) {

            Items.ExceptWith(other);

        }
        public void IntersectWith(IEnumerable<T> other) {

            Items.IntersectWith(other);

        }
        public bool IsProperSubsetOf(IEnumerable<T> other) {

            return Items.IsProperSubsetOf(other);

        }
        public bool IsProperSupersetOf(IEnumerable<T> other) {

            return Items.IsProperSupersetOf(other);

        }
        public bool IsSubsetOf(IEnumerable<T> other) {

            return Items.IsSubsetOf(other);

        }
        public bool IsSupersetOf(IEnumerable<T> other) {

            return Items.IsSupersetOf(other);

        }
        public bool Overlaps(IEnumerable<T> other) {

            return Items.Overlaps(other);

        }
        public bool SetEquals(IEnumerable<T> other) {

            return Items.SetEquals(other);

        }
        public void SymmetricExceptWith(IEnumerable<T> other) {

            Items.SymmetricExceptWith(other);

        }
        public void UnionWith(IEnumerable<T> other) {

            Items.UnionWith(other);

        }

        void ICollection<T>.Add(T item) {

            ((ISet<T>)this).Add(item);

        }
        bool ISet<T>.Add(T item) {

            if (Items.Contains(item))
                return false;

            Items.Add(item);

            OnCollectionChanged(new CollectionChangedEventArgs<T>(CollectionChangedAction.Add, new[] { item }));

            return true;

        }

        // Protected members

        protected new HashSet<T> Items => (HashSet<T>)base.Items;

    }

}