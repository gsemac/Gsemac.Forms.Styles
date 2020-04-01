using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class Selector :
        ISelector {

        // Public members

        public Selector(string selector) {

            this.selector = selector;

            ParseSelector(selector);

        }

        public bool IsMatch(INode node) {

            return selectors.Any(selector => IsMatch(node, selector));

        }

        public override string ToString() {

            return selector;

        }

        // Private members

        private readonly string selector;
        private readonly List<IEnumerable<string>> selectors = new List<IEnumerable<string>>();

        private void ParseSelector(string selector) {

            // Split the selector into its comma-delimited parts.

            IEnumerable<string> commaDelimitedParts = selector.Split(',')
                    .Select(part => part.Trim())
                    .Where(part => !string.IsNullOrWhiteSpace(part))
                    .Distinct();

            // Split each part into its separate components.
            // e.g. "#class1.class2" -> { "#class1", "class2" }

            foreach (string part in commaDelimitedParts)
                selectors.Add(part.Split('.').Where(p => !string.IsNullOrWhiteSpace(p)));

        }
        private bool IsMatch(INode node, IEnumerable<string> selector) {

            // Wildcards match anything.

            if (selector.Any(part => part.Equals("*")))
                return true;

            // For any IDs in the selector, make sure that they match the node's ID.

            IEnumerable<string> ids = selector.Where(part => part.StartsWith("#"))
                .Select(part => part.TrimStart('#'));

            if (!ids.All(id => id.Equals(node.Id, StringComparison.OrdinalIgnoreCase)))
                return false;

            // For any classes in the selector, make sure that the node has at least one match.

            IEnumerable<string> classes = selector.Where(part => !part.StartsWith("#"));

            if (!classes.All(@class => node.Classes.Any(nodeClass => nodeClass.Equals(@class, StringComparison.OrdinalIgnoreCase))))
                return false;

            return true;

        }

    }

}