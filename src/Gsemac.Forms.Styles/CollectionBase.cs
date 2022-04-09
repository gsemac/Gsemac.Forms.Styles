using Gsemac.Collections.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Gsemac.Collections {

    public abstract class CollectionBase<T> :
        ICollection<T> {

        // Public members

        public int Count => Items.Count;
        public bool IsReadOnly => Items.IsReadOnly;

        public virtual void Add(T item) {

            Items.Add(item);

        }
        public virtual bool Remove(T item) {

            return Items.Remove(item);

        }
        public virtual void Clear() {

            Items.Clear();

        }

        public bool Contains(T item) {

            return Items.Contains(item);

        }

        public void CopyTo(T[] array, int arrayIndex) {

            Items.CopyTo(array, arrayIndex);

        }

        public IEnumerator<T> GetEnumerator() {

            return Items.GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return Items.GetEnumerator();

        }

        // Protected members

        protected ICollection<T> Items { get; }

        protected CollectionBase() {

            Items = new List<T>();

        }
        protected CollectionBase(ICollection<T> baseCollection) {

            if (baseCollection is null)
                throw new ArgumentNullException(nameof(baseCollection));

            Items = baseCollection;

        }
        protected CollectionBase(IEnumerable<T> items) :
            this() {

            if (items is null)
                throw new ArgumentNullException(nameof(items));

            Items.AddRange(items);

        }

    }

}