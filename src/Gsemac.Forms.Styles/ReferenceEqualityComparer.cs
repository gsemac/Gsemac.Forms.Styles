using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Gsemac.Forms.Styles {

    public sealed class ReferenceEqualityComparer :
        IEqualityComparer,
        IEqualityComparer<object> {

        // Public members

        public static ReferenceEqualityComparer Instance => new ReferenceEqualityComparer();

        // Private members

        private ReferenceEqualityComparer() { }

        public new bool Equals(object x, object y) {

            return ReferenceEquals(x, y);

        }
        public int GetHashCode(object obj) {

            // Calls GetHasCode on obj non-virtually.
            // https://stackoverflow.com/a/1890230/5383169

            return RuntimeHelpers.GetHashCode(obj);

        }

    }

}