using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class HashCodeBuilder :
        IHashCodeBuilder {

        // Public members

        public void Add(object obj) {

            hashCodes.Add(obj?.GetHashCode() ?? 0);

        }

        public int Build() {

            // https://stackoverflow.com/a/1646913/5383169

            unchecked {

                int hash = 17;

                foreach (int hashCode in hashCodes)
                    hash *= 31 + hashCode;

                return hash;

            }

        }

        public override int GetHashCode() {

            return Build();

        }

        // Private members

        private readonly IList<int> hashCodes = new List<int>();

    }

}