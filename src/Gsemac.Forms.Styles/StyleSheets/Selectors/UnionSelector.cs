using Gsemac.Forms.Styles.Dom;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class UnionSelector :
        ISelector {

        // Public members

        public UnionSelector(IEnumerable<ISelector> selectors) {

            this.selectors = selectors;

        }

        public bool IsMatch(INode2 node) {

            return selectors.Any(selector => selector.IsMatch(node));

        }

        public override string ToString() {

            return string.Join(", ", selectors.Select(selector => selector.ToString()));

        }

        // Private members

        private readonly IEnumerable<ISelector> selectors;

    }
}