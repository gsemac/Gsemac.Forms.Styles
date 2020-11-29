using Gsemac.Collections;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class RulesetCache :
        IRulesetCache {

        // Public members

        public IRuleset this[INode key] {
            get => cacheDict[CreateKey(key)];
            set => cacheDict[CreateKey(key)] = value;
        }

        public ICollection<INode> Keys => throw new NotSupportedException();
        public ICollection<IRuleset> Values => cacheDict.Values;

        public int Count => cacheDict.Count;
        public bool IsReadOnly => false;

        public RulesetCache() :
            this(DefaultCapacity) {
        }
        public RulesetCache(int capacity) {

            cacheDict = new LruDictionary<int, IRuleset>(capacity);

        }

        public void Add(INode key, IRuleset value) {

            cacheDict.Add(CreateKey(key), value);

        }
        public void Add(KeyValuePair<INode, IRuleset> item) {

            Add(item.Key, item.Value);

        }
        public void Clear() {

            cacheDict.Clear();

        }
        public bool Contains(KeyValuePair<INode, IRuleset> item) => throw new NotSupportedException();
        public bool ContainsKey(INode key) {

            return cacheDict.ContainsKey(CreateKey(key));

        }
        public void CopyTo(KeyValuePair<INode, IRuleset>[] array, int arrayIndex) => throw new NotSupportedException();
        public bool Remove(INode key) {

            return cacheDict.Remove(CreateKey(key));

        }
        public bool Remove(KeyValuePair<INode, IRuleset> item) => throw new NotSupportedException();
        public bool TryGetValue(INode key, out IRuleset value) {

            return cacheDict.TryGetValue(CreateKey(key), out value);

        }

        public IEnumerator<KeyValuePair<INode, IRuleset>> GetEnumerator() {

            throw new NotImplementedException();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        // Private members

        private const int DefaultCapacity = 1000;

        private readonly IDictionary<int, IRuleset> cacheDict;

        private int CreateKey(INode nodeKey) {

            return nodeKey.GetHashCode();

        }

    }

}