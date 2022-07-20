using Gsemac.Forms.Styles.StyleSheets.Dom;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets.Selectors {

    internal class CompositeSelector :
        ISelector {

        // Public members

        public int Specificity => selectors.Sum(selector => selector.Specificity);

        public CompositeSelector(ISelector selector) {

            selectors = new List<ISelector>() {
                selector
            };

        }
        public CompositeSelector(IEnumerable<ISelector> selectors) {

            this.selectors = selectors.ToArray();

        }

        public bool IsMatch(INode2 node) {

            if (node is null)
                throw new ArgumentNullException(nameof(node));

            return selectors.All(selector => selector.IsMatch(node));

        }

        public ISelectorMatch Match(INode2 node) {

            if (selectors.All(selector => selector.IsMatch(node)))
                return new SelectorMatch(this);

            return SelectorMatch.Failure;

        }

        public override string ToString() {

            return string.Join(string.Empty, selectors.Select(selector => selector.ToString()));

        }

        // Private members

        private readonly IEnumerable<ISelector> selectors = Enumerable.Empty<ISelector>();

    }

}