using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets {

    public abstract class NodeBase :
        INode {

        // Public members

        public IEnumerable<string> Classes => GetClasses().Concat(GetPseudoClasses());
        public abstract string Tag { get; }
        public abstract string Id { get; }
        public abstract NodeStates States { get; }
        public abstract INode Parent { get; }

        public override int GetHashCode() {

            IHashCodeBuilder hashCodeBuilder = new HashCodeBuilder();

            foreach (string @class in GetClasses())
                hashCodeBuilder.Add(@class);

            hashCodeBuilder.Add(Tag);
            hashCodeBuilder.Add(Id);
            hashCodeBuilder.Add((int)States);
            hashCodeBuilder.Add(Parent);

            return hashCodeBuilder.GetHashCode();

        }
        public override bool Equals(object obj) {

            return obj.GetHashCode() == GetHashCode();

        }

        // Protected members

        protected abstract IEnumerable<string> GetClasses();

        // Private members

        private IEnumerable<string> GetPseudoClasses() {

            foreach (string className in GetClasses()) {

                if (States.HasFlag(NodeStates.Active))
                    yield return $"{className}:active";

                if (States.HasFlag(NodeStates.Hover))
                    yield return $"{className}:hover";

                if (States.HasFlag(NodeStates.Checked))
                    yield return $"{className}:checked";

                if (States.HasFlag(NodeStates.Focus))
                    yield return $"{className}:focus";

                if (States.HasFlag(NodeStates.Disabled))
                    yield return $"{className}:disabled";

            }

        }

    }

}