using Gsemac.Forms.Styles.StyleSheets.Properties;
using System;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets.Rulesets.Extensions {

    public static class RulesetExtensions {

        public static void InheritPropertiesFrom(this IRuleset ruleset, IRuleset other) {

            if (ruleset is null)
                throw new ArgumentNullException(nameof(ruleset));

            if (other is null)
                throw new ArgumentNullException(nameof(other));

            foreach (IProperty property in other.Where(p => p.IsInheritable))
                ruleset.Add(property);

        }

    }

}