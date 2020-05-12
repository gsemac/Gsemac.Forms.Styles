using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class ClassSelector :
        ISelector {

        // Public members

        public string Name { get; }

        public ClassSelector(string name) {

            this.Name = name?.TrimStart('.');

        }

        public bool IsMatch(INode node) {

            if (string.IsNullOrEmpty(Name))
                return false;

            return node.Classes.Any(c => c.TrimStart('.').Equals(Name, StringComparison.OrdinalIgnoreCase));

        }

        public override string ToString() {

            return $".{Name}";

        }

    }

}