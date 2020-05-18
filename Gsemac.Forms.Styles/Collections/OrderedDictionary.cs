using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.Collections {

    public class OrderedDictionary<TKey, TValue> :
        IDictionary<TKey, TValue> {

        // Public members

        public TValue this[TKey key] {
            get => dict[key];
            set => SetValue(key, value);
        }

        public ICollection<TKey> Keys => new ReadOnlyCollection<TKey>(orderedKeys);
        public ICollection<TValue> Values => new ReadOnlyCollection<TValue>(orderedKeys.Select(key => dict[key]).ToList());
        public int Count => dict.Count;
        public bool IsReadOnly => false;

        public void Add(TKey key, TValue value) {

            orderedKeys.Add(key);

            dict.Add(key, value);

        }
        public void Add(KeyValuePair<TKey, TValue> item) {

            Add(item.Key, item.Value);

        }
        public void Clear() {

            orderedKeys.Clear();

            dict.Clear();

        }
        public bool Contains(KeyValuePair<TKey, TValue> item) {

            return dict.Contains(item);

        }
        public bool ContainsKey(TKey key) {

            return dict.ContainsKey(key);

        }
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {

            dict.ToArray().CopyTo(array, arrayIndex);

        }
        public bool Remove(TKey key) {

            orderedKeys.Remove(key);

            return dict.Remove(key);

        }
        public bool Remove(KeyValuePair<TKey, TValue> item) {

            return Remove(item.Key);

        }
        public bool TryGetValue(TKey key, out TValue value) {

            return dict.TryGetValue(key, out value);

        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {

            return dict.GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        // Private members

        private readonly List<TKey> orderedKeys = new List<TKey>();
        private readonly Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();

        private void SetValue(TKey key, TValue value) {

            if (ContainsKey(key))
                Remove(key);

            Add(key, value);

        }

    }

}