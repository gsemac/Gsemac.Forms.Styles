using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class Node :
        NodeBase {

        // Public members

        public override string Id => string.Empty;
        public override NodeStates States { get; }

        public Node(string className, NodeStates states = NodeStates.None) {

            classes = new[] { className };
            States = states;

        }

        // Protected members

        protected override IEnumerable<string> GetClasses() {

            return classes;

        }

        // Private members

        private readonly IEnumerable<string> classes = Enumerable.Empty<string>();

    }

}