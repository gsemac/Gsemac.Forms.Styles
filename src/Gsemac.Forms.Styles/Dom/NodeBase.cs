using Gsemac.Core;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.Dom {

    public abstract class NodeBase :
        INode {

        // Public members

        public abstract IEnumerable<string> Classes { get; }
        public virtual IEnumerable<string> PseudoClasses => GetPseudoClasses();
        public virtual string PseudoElement => string.Empty;
        public abstract string Tag { get; }
        public abstract string Id { get; }
        public abstract NodeStates States { get; }
        public abstract INode Parent { get; }

        public override int GetHashCode() {

            IHashCodeBuilder hashCodeBuilder = new HashCodeBuilder();

            foreach (string @class in Classes)
                hashCodeBuilder.WithValue(@class);

            hashCodeBuilder.WithValue(Tag);
            hashCodeBuilder.WithValue(Id);
            hashCodeBuilder.WithValue((int)States);
            hashCodeBuilder.WithValue(Parent);

            return hashCodeBuilder.GetHashCode();

        }
        public override bool Equals(object obj) {

            return obj.GetHashCode() == GetHashCode();

        }

        // Private members

        private IEnumerable<string> GetPseudoClasses() {

            if (States.HasFlag(NodeStates.Active))
                yield return ":active";

            if (States.HasFlag(NodeStates.Hover))
                yield return ":hover";

            if (States.HasFlag(NodeStates.Checked))
                yield return ":checked";

            if (States.HasFlag(NodeStates.Focus))
                yield return ":focus";

            if (States.HasFlag(NodeStates.FocusWithin))
                yield return ":focus-within";

            if (States.HasFlag(NodeStates.Disabled))
                yield return ":disabled";

        }

    }

}