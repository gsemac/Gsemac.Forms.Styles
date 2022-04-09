namespace Gsemac.Forms.Styles.StyleSheets {

    public class IDSelector :
        ISelector {

        // Public members

        public string Name { get; }

        public IDSelector(string name) {

            this.Name = name?.TrimStart('#');

        }

        public bool IsMatch(INode node) {

            if (string.IsNullOrEmpty(Name))
                return false;

            return node.Id.Equals(Name);

        }

        public override string ToString() {

            return $"#{Name}";

        }

    }

}