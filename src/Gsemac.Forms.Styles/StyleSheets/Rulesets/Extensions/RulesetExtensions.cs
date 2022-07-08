﻿using Gsemac.Forms.Styles.StyleSheets.Properties;
using System;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets.Rulesets.Extensions {

    public static class RulesetExtensions {

        public static void Inherit(this IRuleset ruleset, IRuleset other) {

            if (ruleset is null)
                throw new ArgumentNullException(nameof(ruleset));

            if (other is null)
                throw new ArgumentNullException(nameof(other));

            foreach (IProperty property in other.Where(p => p.Inherited))
                ruleset.Add(property);

        }

    }

}