using Gsemac.Forms.Styles.StyleSheets.Dom;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets.Selectors {

    public class UnionSelector :
        ISelector {

        // Public members

        public int Specificity => selectors.Any() ? selectors.Max(selector => selector.Specificity) : 0;

        public UnionSelector(IEnumerable<ISelector> selectors) {

            this.selectors = selectors;

        }

        public bool IsMatch(INode2 node) {

            return selectors.Any(selector => selector.IsMatch(node));

        }

        public ISelectorMatch Match(INode2 node) {

            if (node is null)
                throw new ArgumentNullException(nameof(node));

            ISelectorMatch match = selectors.Select(selector => selector.Match(node))
                .Where(m => m.Success)
                .OrderByDescending(m => m.Specificity)
                .FirstOrDefault();

            return match is object ?
                match :
                SelectorMatch.Failure;

        }

        public override string ToString() {

            return string.Join(", ", selectors.Select(selector => selector.ToString()));

        }

        // Private members

        private readonly IEnumerable<ISelector> selectors;

    }

}