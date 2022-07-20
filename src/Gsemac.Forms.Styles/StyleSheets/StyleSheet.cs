using Gsemac.Forms.Styles.StyleSheets.Dom;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class StyleSheet :
        IStyleSheet {

        // Public members

        public StyleSheet(IEnumerable<IRuleset> rulesets) {

            if (rulesets is null)
                throw new ArgumentNullException(nameof(rulesets));

            this.rulesets = new List<IRuleset>(rulesets);

        }

        public IEnumerable<IRuleset> GetStyles(INode2 node) {

            if (node is null)
                throw new ArgumentNullException(nameof(node));

            return rulesets
                .Select(ruleset => Tuple.Create(ruleset, ruleset.Selector.Match(node)))
                .Where(pair => pair.Item2.Success)
                .OrderBy(pair => pair.Item1.Origin)
                .ThenBy(pair => pair.Item2.Specificity)
                .Select(pair => pair.Item1)
                .ToArray();

        }

        public IEnumerator<IRuleset> GetEnumerator() {

            return rulesets.GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        public override string ToString() {

            StringBuilder sb = new StringBuilder();

            foreach (IRuleset ruleSet in rulesets)
                sb.AppendLine(ruleSet.ToString());

            return sb.ToString();

        }

        // Private members

        private readonly IList<IRuleset> rulesets = new List<IRuleset>();

    }

}