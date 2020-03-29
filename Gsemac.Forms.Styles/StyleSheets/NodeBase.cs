using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets {

    public abstract class NodeBase :
        INode {

        // Public members

        public IEnumerable<string> Classes => GetClasses().Concat(GetPseudoClasses());
        public abstract string Id { get; }
        public abstract NodeStates States { get; }

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

            }

        }

    }

}