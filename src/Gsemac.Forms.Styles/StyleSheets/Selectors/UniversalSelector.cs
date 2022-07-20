using Gsemac.Forms.Styles.StyleSheets.Dom;
using System;

namespace Gsemac.Forms.Styles.StyleSheets.Selectors {

    public class UniversalSelector :
        ISelector {

        // Public members

        public int Specificity => SelectorUtilities.GetSpecificity(WeightCategory.None);

        public ISelectorMatch Match(INode2 node) {

            if (node is null)
                throw new ArgumentNullException(nameof(node));

            return new SelectorMatch(this);

        }

        public override string ToString() {

            return "*";

        }

    }

}