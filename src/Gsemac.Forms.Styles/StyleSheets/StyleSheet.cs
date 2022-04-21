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

        public IEnumerable<IRuleset> GetRulesets(INode2 node) {

            IList<IRuleset> result = new List<IRuleset>();

            foreach (IRuleset ruleset in rulesets.Where(r => r.Selector.IsMatch(node)))
                result.Add(ruleset);

            return result;

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