using Gsemac.Core;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public class EquivalentPropertyEqualityComparer :
        IEqualityComparer<IProperty> {

        // Public members

        public bool Equals(IProperty x, IProperty y) {

            return GetHashCode(x).Equals(GetHashCode(y));

        }
        public int GetHashCode(IProperty obj) {

            if (obj is null)
                return 0;

            IHashCodeBuilder hashCodeBuilder = new HashCodeBuilder();

            // Names are case-insensitive, but some values are not (e.g. URLs).

            hashCodeBuilder.WithValue(obj.Name.ToLowerInvariant());
            hashCodeBuilder.WithValue(obj.Value);

            return hashCodeBuilder.Build();

        }

    }

}