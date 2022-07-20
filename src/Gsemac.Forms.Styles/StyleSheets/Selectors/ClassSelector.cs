using Gsemac.Forms.Styles.StyleSheets.Dom;
using System;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets.Selectors {

    public class ClassSelector :
        ISelector {

        // Public members

        public int Specificity => SelectorUtilities.GetSpecificity(WeightCategory.Class);

        public ClassSelector(string className) {

            this.className = FormatClassName(className);

        }

        public ISelectorMatch Match(INode2 node) {

            if (node is null)
                throw new ArgumentNullException(nameof(node));

            // Class names are case-sensitive.
            // https://stackoverflow.com/a/12533957/5383169 (BoltClock)

            if (!string.IsNullOrEmpty(className) && node.Classes.Any(c => FormatClassName(c).Equals(className)))
                return new SelectorMatch(this);

            return SelectorMatch.Failure;

        }

        public override string ToString() {

            return $".{className}";

        }

        // Private members

        private readonly string className;

        private static string FormatClassName(string className) {

            if (string.IsNullOrWhiteSpace(className))
                return string.Empty;

            if (className.StartsWith("."))
                className = className.Substring(1, className.Length - 1);

            className = className.Trim();

            return className;

        }

    }

}