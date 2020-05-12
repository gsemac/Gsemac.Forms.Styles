using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class ChildSelector :
        ISelector {

        // Public members

        public ChildSelector(ISelector parentSelector, ISelector childSelector) {

            this.parentSelector = parentSelector;
            this.childSelector = childSelector;

        }

        public bool IsMatch(INode node) {

            if (parentSelector is null || childSelector is null)
                return false;

            if (node.Parent is null)
                return false;

            return parentSelector.IsMatch(node.Parent) && childSelector.IsMatch(node);

        }

        public override string ToString() {

            return $"{parentSelector.ToString()} > {childSelector.ToString()}";

        }

        // Private members

        private readonly ISelector parentSelector;
        private readonly ISelector childSelector;

    }

}