using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Gsemac.Forms.Styles.Collections {

    public class LruDictionary<TKey, TValue> :
        IDictionary<TKey, TValue> {

        // Public members

        public int Capacity { get; set; }

        public ICollection<TKey> Keys => new ReadOnlyCollection<TKey>(values.Select(i => i.Key));
        public ICollection<TValue> Values => new ReadOnlyCollection<TValue>(values.Select(i => i.Value));

        public int Count => values.Count();
        public bool IsReadOnly => false;

        public TValue this[TKey key] {
            get => GetValue(key);
            set => SetValue(key, value);
        }

        public LruDictionary(int capacity) {

            this.Capacity = capacity;

        }

        public bool ContainsKey(TKey key) {

            return nodeDict.ContainsKey(key);

        }
        public void Add(TKey key, TValue value) {

            SetValue(key, value);

        }
        public bool Remove(TKey key) {

            if (nodeDict.TryGetValue(key, out LinkedListNode<DictItem> node)) {

                values.Remove(node);

                nodeDict.Remove(key);

                return true;

            }
            else {

                return false;

            }

        }
        public bool TryGetValue(TKey key, out TValue value) {

            if (nodeDict.TryGetValue(key, out LinkedListNode<DictItem> node)) {

                value = node.Value.Value;

                SetMostRecent(node);

                return true;

            }
            else {

                value = default;

                return false;

            }

        }
        public void Add(KeyValuePair<TKey, TValue> item) {

            SetValue(item.Key, item.Value);

        }
        public void Clear() {

            values.Clear();

            nodeDict.Clear();

        }
        public bool Contains(KeyValuePair<TKey, TValue> item) {

            return nodeDict.ContainsKey(item.Key);

        }
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {

            values.Select(i => new KeyValuePair<TKey, TValue>(i.Key, i.Value)).ToArray()
                .CopyTo(array, arrayIndex);

        }
        public bool Remove(KeyValuePair<TKey, TValue> item) {

            return Remove(item.Key);

        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {

            return values.Select(i => new KeyValuePair<TKey, TValue>(i.Key, i.Value))
                .GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        // Private members

        private class DictItem {

            public TKey Key { get; }
            public TValue Value { get; }

            public DictItem(TKey key, TValue value) {

                this.Key = key;
                this.Value = value;

            }

        }

        private readonly LinkedList<DictItem> values = new LinkedList<DictItem>();
        private readonly IDictionary<TKey, LinkedListNode<DictItem>> nodeDict = new Dictionary<TKey, LinkedListNode<DictItem>>();

        private void SetMostRecent(LinkedListNode<DictItem> node) {

            values.Remove(node);

            values.AddFirst(node);

        }
        private void EvictOldest() {

            if (values.Any()) {

                LinkedListNode<DictItem> nodeToRemove = values.Last;

                bool removedFromDict = nodeDict.Remove(nodeToRemove.Value.Key);

                Debug.Assert(removedFromDict);

                values.Remove(nodeToRemove);

            }

        }

        private TValue GetValue(TKey key) {

            if (TryGetValue(key, out TValue value))
                return value;
            else
                throw new KeyNotFoundException("The given key was not present in the dictionary.");

        }
        private void SetValue(TKey key, TValue value) {

            if (!Remove(key) && Count >= Capacity)
                EvictOldest();

            nodeDict[key] = values.AddFirst(new DictItem(key, value));

            Debug.Assert(Count <= Capacity);

        }

    }

}