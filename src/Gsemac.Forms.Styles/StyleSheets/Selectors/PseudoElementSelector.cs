using Gsemac.Forms.Styles.Dom;
using System;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class PseudoElementSelector :
      ISelector {

        // Public members

        public PseudoElementSelector(string elementName) {

            this.elementName = elementName?.TrimStart(':');

        }

        public bool IsMatch(INode2 node) {

            throw new NotImplementedException();

            if (string.IsNullOrEmpty(elementName))
                return false;

            // return node.PseudoElement.TrimStart(':').Equals(elementName, StringComparison.OrdinalIgnoreCase);

        }

        public override string ToString() {

            return $"::{elementName}";

        }

        // Private members

        private readonly string elementName;

    }

}