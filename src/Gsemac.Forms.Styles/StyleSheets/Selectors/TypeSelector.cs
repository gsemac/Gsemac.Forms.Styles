using Gsemac.Forms.Styles.StyleSheets.Dom;
using System;

namespace Gsemac.Forms.Styles.StyleSheets.Selectors {

    public class TypeSelector :
        ISelector {

        // Public members

        public int Specificity => SelectorUtilities.GetSpecificity(WeightCategory.Type);

        public TypeSelector(string type) {

            this.type = type;

        }

        public ISelectorMatch Match(INode2 node) {

            if (node is null)
                throw new ArgumentNullException(nameof(node));

            // Tags are not case-sensitive.

            if (!string.IsNullOrEmpty(type) && node.Tag.Equals(type, StringComparison.OrdinalIgnoreCase))
                return new SelectorMatch(this);

            return SelectorMatch.Failure;

        }

        public override string ToString() {

            return type;

        }

        // Private members

        private readonly string type;

    }

}