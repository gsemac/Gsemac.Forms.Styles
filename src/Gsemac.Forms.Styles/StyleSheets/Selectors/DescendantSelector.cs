using Gsemac.Forms.Styles.Dom;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class DescendantSelector :
        ISelector {

        // Public members

        public DescendantSelector(ISelector parentSelector, ISelector descendantSelector) {

            this.parentSelector = parentSelector;
            this.descendantSelector = descendantSelector;

        }

        public bool IsMatch(INode2 node) {

            if (parentSelector is null || descendantSelector is null)
                return false;

            if (node.Parent is null)
                return false;

            return descendantSelector.IsMatch(node) && MatchesNodeOrParentNode(parentSelector, node.Parent);

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