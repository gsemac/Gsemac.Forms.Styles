using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class PseudoClassSelector :
      ISelector {

        // Public members

        public PseudoClassSelector(string name) {

            this.className = name?.TrimStart(':');

        }

        public bool IsMatch(INode node) {

            if (string.IsNullOrEmpty(className))
                return false;

            return node.PseudoClasses.Any(c => c.TrimStart(':').Equals(className, StringComparison.OrdinalIgnoreCase));

        }

        public override string ToString() {

            return $":{className}";

        }

        // Private members

        private readonly string className;

    }

}