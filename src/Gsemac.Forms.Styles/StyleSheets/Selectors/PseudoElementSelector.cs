using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class PseudoElementSelector :
      ISelector {

        // Public members

        public PseudoElementSelector(string name) {

            this.elementName = name?.TrimStart(':');

        }

        public bool IsMatch(INode node) {

            if (string.IsNullOrEmpty(elementName))
                return false;

            return node.PseudoElement.TrimStart(':').Equals(elementName, StringComparison.OrdinalIgnoreCase);

        }

        public override string ToString() {

            return $"::{elementName}";

        }

        // Private members

        private readonly string elementName;

    }

}