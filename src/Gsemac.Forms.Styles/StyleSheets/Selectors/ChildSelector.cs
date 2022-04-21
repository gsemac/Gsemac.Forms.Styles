using Gsemac.Forms.Styles.StyleSheets.Dom;

namespace Gsemac.Forms.Styles.StyleSheets.Selectors {

    public class ChildSelector :
        ISelector {

        // Public members

        public ChildSelector(ISelector parentSelector, ISelector childSelector) {

            this.parentSelector = parentSelector;
            this.childSelector = childSelector;

        }

        public bool IsMatch(INode2 node) {

            if (parentSelector is null || childSelector is null)
                return false;

            if (node.Parent is null)
                return false;

            return childSelector.IsMatch(node) && parentSelector.IsMatch(node.Parent);

        }

        public override string ToString() {

            return $"{parentSelector} > {childSelector}";

        }

        // Private members

        private readonly ISelector parentSelector;
        private readonly ISelector childSelector;

    }

}