using Gsemac.Forms.Styles.StyleSheets.Dom;
using System;

namespace Gsemac.Forms.Styles.StyleSheets.Selectors {

    public class ChildSelector :
        ISelector {

        // Public members

        public int Specificity => parentSelector.Specificity + childSelector.Specificity;

        public ChildSelector(ISelector parentSelector, ISelector childSelector) {

            if (parentSelector is null)
                throw new ArgumentNullException(nameof(parentSelector));

            if (childSelector is null)
                throw new ArgumentNullException(nameof(childSelector));

            this.parentSelector = parentSelector;
            this.childSelector = childSelector;

        }

        public ISelectorMatch Match(INode2 node) {

            if (node is null)
                throw new ArgumentNullException(nameof(node));

            if (node.Parent is object && childSelector.IsMatch(node) && parentSelector.IsMatch(node.Parent))
                return new SelectorMatch(this);

            return SelectorMatch.Failure;

        }

        public override string ToString() {

            return $"{parentSelector} > {childSelector}";

        }

        // Private members

        private readonly ISelector parentSelector;
        private readonly ISelector childSelector;

    }

}