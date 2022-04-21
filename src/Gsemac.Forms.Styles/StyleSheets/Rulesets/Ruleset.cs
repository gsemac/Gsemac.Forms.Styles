using Gsemac.Forms.Styles.StyleSheets.Properties;
using Gsemac.Forms.Styles.StyleSheets.Selectors;

namespace Gsemac.Forms.Styles.StyleSheets.Rulesets {

    public class Ruleset :
        RulesetBase {

        // Public members

        public static Ruleset Empty => new Ruleset();

        public Ruleset() {
        }
        public Ruleset(IPropertyFactory propertyFactory) :
            base(propertyFactory) {
        }
        public Ruleset(ISelector selector) :
            base(selector) {
        }
        public Ruleset(ISelector selector, IPropertyFactory propertyFactory) :
            base(selector, propertyFactory) {
        }
        public Ruleset(IRuleset other) :
            base(other) {
        }
        public Ruleset(IRuleset other, IPropertyFactory propertyFactory) :
            base(other, propertyFactory) {
        }

    }

}