using Gsemac.Forms.Styles.Dom;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class ClassSelector :
        ISelector {

        // Public members

        public ClassSelector(string name) {

            this.className = name?.TrimStart('.');

        }

        public bool IsMatch(INode2 node) {

            if (string.IsNullOrEmpty(className))
                return false;

            return node.Classes.Any(c => c.TrimStart('.').Equals(className));

        }

        public override string ToString() {

            return $".{className}";

        }

        // Private members

        private readonly string className;

    }

}