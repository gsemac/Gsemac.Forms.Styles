using Gsemac.Forms.Styles.StyleSheets.Dom;
using System;

namespace Gsemac.Forms.Styles.StyleSheets.Selectors {

    public class IdSelector :
        ISelector {

        // Public members

        public int Specificity => SelectorUtilities.GetSpecificity(WeightCategory.Id);

        public IdSelector(string id) {

            this.id = FormatId(id);

        }

        public ISelectorMatch Match(INode2 node) {

            if (node is null)
                throw new ArgumentNullException(nameof(node));

            if (!string.IsNullOrEmpty(id) && node.Id.Equals(id))
                return new SelectorMatch(this);

            return SelectorMatch.Failure;

        }

        public override string ToString() {

            return $"#{id}";

        }

        // Private members

        private readonly string id;

        private static string FormatId(string id) {

            if (string.IsNullOrWhiteSpace(id))
                return string.Empty;

            if (id.StartsWith("#"))
                id = id.Substring(1, id.Length - 1);

            id = id.Trim();

            return id;

        }

    }

}