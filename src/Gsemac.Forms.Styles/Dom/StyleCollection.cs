using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Forms.Styles.Dom {

    internal sealed class StyleCollection :
        ICollection<IRuleset> {

        // Public members

        public event EventHandler<StylesChangedEventArgs> StyleAdded;
        public event EventHandler<StylesChangedEventArgs> StyleRemoved;

        public int Count => items.Count;
        public bool IsReadOnly => false;

        public StyleCollection(IDomNode parentNode) {

            if (parentNode is null)
                throw new ArgumentNullException(nameof(parentNode));

            this.parentNode = parentNode;

        }

        public void Add(IRuleset item) {

            if (item is null)
                throw new ArgumentNullException(nameof(item));

            if (!Contains(item)) {

                items.Add(item);

                OnRulesetAdded(item);

            }

        }
        public bool Remove(IRuleset item) {

            if (item is null)
                return false;

            bool removed = items.Remove(item);

            if (removed)
                OnRulesetRemoved(item);

            return removed;

        }
        public void Clear() {

            IEnumerable<IRuleset> rulesets = items.ToArray();

            items.Clear();

            foreach (IRuleset ruleset in rulesets)
                OnRulesetRemoved(ruleset);

        }

        public bool Contains(IRuleset item) {

            return items.Contains(item);

        }

        public void CopyTo(IRuleset[] array, int arrayIndex) {

            if (array is null)
                throw new ArgumentNullException(nameof(array));

            if (arrayIndex < 0 || arrayIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));

            items.CopyTo(array, arrayIndex);

        }

        public IEnumerator<IRuleset> GetEnumerator() {

            return items.GetEnumerator();

        }

        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        // Private members

        private readonly IDomNode parentNode;
        private readonly HashSet<IRuleset> items = new HashSet<IRuleset>();

        private void OnRulesetAdded(IRuleset ruleset) {

            if (ruleset is null)
                throw new ArgumentNullException(nameof(ruleset));

            StyleAdded?.Invoke(parentNode, new StylesChangedEventArgs(ruleset));

        }
        private void OnRulesetRemoved(IRuleset ruleset) {

            if (ruleset is null)
                throw new ArgumentNullException(nameof(ruleset));

            StyleRemoved?.Invoke(parentNode, new StylesChangedEventArgs(ruleset));

        }

    }

}