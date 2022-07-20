using Gsemac.Forms.Styles.StyleSheets.Dom;
using System;

namespace Gsemac.Forms.Styles.StyleSheets.Selectors {

    public class DescendantSelector :
        ISelector {

        // Public members

        public int Specificity => parentSelector.Specificity + descendantSelector.Specificity;

        public DescendantSelector(ISelector parentSelector, ISelector descendantSelector) {

            if (parentSelector is null)
                throw new ArgumentNullException(nameof(parentSelector));

            if (descendantSelector is null)
                throw new ArgumentNullException(nameof(descendantSelector));

            this.parentSelector = parentSelector;
            this.descendantSelector = descendantSelector;

        }

        public ISelectorMatch Match(INode2 node) {

            if (node is null)
                throw new ArgumentNullException(nameof(node));

            if (node.Parent is object && descendantSelector.IsMatch(node) && MatchesNodeOrParentNode(parentSelector, node.Parent))
                return new SelectorMatch(this);

            return SelectorMatch.Failure;

        }

        public override string ToString() {

            return $"{parentSelector} {descendantSelector}";

        }

        // Private members

        private readonly ISelector parentSelector;
        private readonly ISelector descendantSelector;

        private bool MatchesNodeOrParentNode(ISelector selector, INode2 node) {

            bool result = selector.IsMatch(node);

            if (!result && node.Parent != null)
                result = MatchesNodeOrParentNode(selector, node.Parent);

            return result;

        }

    }

}