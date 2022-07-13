using Gsemac.Forms.Styles.StyleSheets.Selectors;

namespace Gsemac.Forms.Styles.StyleSheets.Rulesets {

    public class Ruleset :
        RulesetBase {

        // Public members

        public static Ruleset Empty => new Ruleset();

        public Ruleset() {
        }
        public Ruleset(ISelector selector) :
            base(selector) {
        }
        public Ruleset(ISelector selector, StyleOrigin origin) :
           base(selector, origin) {
        }
        public Ruleset(IRuleset other) :
            base(other) {
        }

    }

}