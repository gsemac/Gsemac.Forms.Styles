using Gsemac.Forms.Styles.StyleSheets.Dom;
using System;

namespace Gsemac.Forms.Styles.StyleSheets.Selectors {

    public class TypeSelector :
        ISelector {

        // Public members

        public string Name { get; }

        public TypeSelector(string name) {

            Name = name;

        }

        public bool IsMatch(INode2 node) {

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