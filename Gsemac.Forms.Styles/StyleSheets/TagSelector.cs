using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class TagSelector :
        ISelector {

        // Public members

        public string Name { get; }

        public TagSelector(string name) {

            this.Name = name;

        }

        public bool IsMatch(INode node) {

            if (string.IsNullOrEmpty(Name))
                return false;

            if (Name.Equals("*"))
                return true;

            return node.Tag.Equals(Name, StringComparison.OrdinalIgnoreCase);

        }

        public override string ToString() {

            return Name;

        }

    }

}