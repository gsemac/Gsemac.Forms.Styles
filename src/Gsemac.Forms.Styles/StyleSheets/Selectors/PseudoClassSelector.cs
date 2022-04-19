using Gsemac.Forms.Styles.Dom;
using System;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class PseudoClassSelector :
      ISelector {

        // Public members

        public PseudoClassSelector(string className) {

            this.className = className?.TrimStart(':');

        }

        public bool IsMatch(INode2 node) {
            Console.WriteLine(node.States.Contains(NodeState.Hover));
            switch (className) {

                case "focus":
                    return node.States.Contains(NodeState.Focus);

                case "hover":
                    return node.States.Contains(NodeState.Hover);

                default:
                    return false;

            }

        }

        public override string ToString() {

            return $":{className}";

        }

        // Private members

        private readonly string className;

    }

}