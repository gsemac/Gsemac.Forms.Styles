using Gsemac.Forms.Styles.Dom;
using System;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class PseudoClassSelector :
      ISelector {

        // Public members

        public PseudoClassSelector(string className) {

            this.className = className?.TrimStart(':');

        }

        public bool IsMatch(INode2 node) {

            throw new NotImplementedException();

            if (string.IsNullOrEmpty(className))
                return false;

            // return node.PseudoClasses.Any(c => c.TrimStart(':').Equals(className, StringComparison.OrdinalIgnoreCase));

        }

        public override string ToString() {

            return $":{className}";

        }

        // Private members

        private readonly string className;

    }

}