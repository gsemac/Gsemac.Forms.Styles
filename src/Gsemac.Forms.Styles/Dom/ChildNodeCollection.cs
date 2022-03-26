using System;
using System.Collections;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.Dom {

    internal class ChildNodeCollection :
        ICollection<IDomNode> {

        // Public members

        public int Count => items.Count;
        public bool IsReadOnly => false;

        public ChildNodeCollection(IDomNode parentNode) {

            if (parentNode is null)
                throw new ArgumentNullException(nameof(parentNode));

            this.parentNode = parentNode;

        }

        public void Add(IDomNode item) {

            if (item is null)
                throw new ArgumentNullException(nameof(item));

            item.Parent = parentNode;

            items.Add(item);

        }
        public bool Remove(IDomNode item) {

            if (item is null)
                return false;

            if (items.Remove(item)) {

                item.Parent = null;

                return true;

            }
            else {

                return false;

            }

        }
        public void Clear() {

            foreach (IDomNode node in items)
                node.Parent = null;

            items.Clear();

        }

        public bool Contains(IDomNode item) {

            if (item is null)
                return false;

            return items.Contains(item);

        }

        public void CopyTo(IDomNode[] array, int arrayIndex) {

            if (array is null)
                throw new ArgumentNullException(nameof(array));

            if (arrayIndex < 0 || arrayIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));

            items.CopyTo(array, arrayIndex);

        }

        public IEnumerator<IDomNode> GetEnumerator() {

            return items.GetEnumerator();

        }

        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        // Private members

        private readonly IDomNode parentNode;
        private readonly List<IDomNode> items = new List<IDomNode>();

    }

}
