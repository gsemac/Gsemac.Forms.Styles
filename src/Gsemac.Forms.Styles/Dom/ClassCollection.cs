using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Forms.Styles.Dom {

    internal sealed class ClassCollection :
        ICollection<string> {

        // Public members

        public event EventHandler<ClassesChangedEventArgs> ClassAdded;
        public event EventHandler<ClassesChangedEventArgs> ClassRemoved;

        public int Count => items.Count;
        public bool IsReadOnly => false;

        public ClassCollection(IDomNode parentNode) {

            if (parentNode is null)
                throw new ArgumentNullException(nameof(parentNode));

            this.parentNode = parentNode;

        }

        public void Add(string item) {

            if (string.IsNullOrEmpty(item))
                return;

            item = item.Trim();

            if (!Contains(item)) {

                items.Add(item);

                OnClassAdded(item);

            }

        }
        public bool Remove(string item) {

            if (string.IsNullOrEmpty(item))
                return false;

            item = item.Trim();

            bool removed = items.Remove(item);

            if (removed)
                OnClassRemoved(item);

            return removed;

        }
        public void Clear() {

            IEnumerable<string> classes = items.ToArray();

            items.Clear();

            foreach (string className in classes)
                OnClassRemoved(className);

        }

        public bool Contains(string item) {

            return items.Contains(item);

        }

        public void CopyTo(string[] array, int arrayIndex) {

            if (array is null)
                throw new ArgumentNullException(nameof(array));

            if (arrayIndex < 0 || arrayIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));

            items.CopyTo(array, arrayIndex);

        }

        public IEnumerator<string> GetEnumerator() {

            return items.GetEnumerator();

        }

        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        // Private members

        private readonly IDomNode parentNode;
        private readonly HashSet<string> items = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        private void OnClassAdded(string className) {

            if (className is null)
                throw new ArgumentNullException(nameof(className));

            ClassAdded?.Invoke(parentNode, new ClassesChangedEventArgs(className));

        }
        private void OnClassRemoved(string className) {

            if (className is null)
                throw new ArgumentNullException(nameof(className));

            ClassRemoved?.Invoke(parentNode, new ClassesChangedEventArgs(className));

        }

    }

}