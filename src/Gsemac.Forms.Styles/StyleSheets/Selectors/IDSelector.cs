using Gsemac.Forms.Styles.StyleSheets.Dom;

namespace Gsemac.Forms.Styles.StyleSheets.Selectors {

    public class IDSelector :
        ISelector {

        // Public members

        public string Name { get; }

        public IDSelector(string name) {

            Name = name?.TrimStart('#');

        }

        public bool IsMatch(INode2 node) {

            if (string.IsNullOrEmpty(Name))
                return false;

            return node.Id.Equals(Name);

        }

        public override string ToString() {

            return $"#{Name}";

        }

    }

}