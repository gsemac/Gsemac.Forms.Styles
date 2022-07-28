using Gsemac.Forms.Styles.StyleSheets.Dom;
using System;

namespace Gsemac.Forms.Styles.StyleSheets.Selectors {

    public class PseudoClassSelector :
      ISelector {

        // Public members

        public int Specificity => SelectorUtilities.GetSpecificity(WeightCategory.Class);

        public PseudoClassSelector(string className) {

            this.className = FormatClassName(className);

        }

        public ISelectorMatch Match(INode2 node) {

            if (node is null)
                throw new ArgumentNullException(nameof(node));

            // Pseudo-class names are not case-sensitive.

            bool success = false;

            if (Enum.TryParse(className, ignoreCase: true, out NodeState nodeState))
                success = node.States.Contains(nodeState);

            return success ?
                new SelectorMatch(this) :
                SelectorMatch.Failure;

        }

        public override string ToString() {

            return $":{className}";

        }

        // Private members

        private readonly string className;

        private static string FormatClassName(string className) {

            if (string.IsNullOrWhiteSpace(className))
                return string.Empty;

            if (className.StartsWith(":"))
                className = className.Substring(1, className.Length - 1);

            className = className.Trim().ToLowerInvariant();

            return className;

        }

    }

}