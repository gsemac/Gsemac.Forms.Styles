using Gsemac.Core;
using Gsemac.Forms.Styles.StyleSheets.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets.Rulesets {

    public class EquivalentRulesetEqualityComparer :
        IEqualityComparer<IRuleset> {

        // Public members

        public bool Equals(IRuleset x, IRuleset y) {

            return GetHashCode(x).Equals(GetHashCode(y));

        }
        public int GetHashCode(IRuleset obj) {

            if (obj is null)
                return 0;

            IEqualityComparer<IProperty> propertyComparer = new EquivalentPropertyEqualityComparer();
            IHashCodeBuilder hashCodeBuilder = new HashCodeBuilder();

            foreach (IProperty property in obj.OrderBy(p => p.Name, StringComparer.OrdinalIgnoreCase))
                hashCodeBuilder.WithValue(propertyComparer.GetHashCode(property));

            return hashCodeBuilder.Build();

        }

    }

}