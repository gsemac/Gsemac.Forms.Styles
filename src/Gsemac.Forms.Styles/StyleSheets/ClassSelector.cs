using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class ClassSelector :
        ISelector {

        // Public members

        public ClassSelector(string name) {

            this.className = name?.TrimStart('.');

        }

        public bool IsMatch(INode node) {

            if (string.IsNullOrEmpty(className))
                return false;

            return node.Classes.Any(c => c.TrimStart('.').Equals(className, StringComparison.OrdinalIgnoreCase));

        }

        public override string ToString() {

            return $".{className}";

        }

        // Private members

        private readonly string className;

    }

}