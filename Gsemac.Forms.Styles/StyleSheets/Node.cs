using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class Node :
        NodeBase {

        // Public members

        public override string Tag { get; }
        public override string Id => string.Empty;
        public override NodeStates States { get; }
        public override INode Parent { get; }

        public Node(string tag, string className, INode parent = null, NodeStates states = NodeStates.None) {

            this.Tag = tag;
            classes = new[] { className };
            States = states;
            this.Parent = parent;

        }

        // Protected members

        protected override IEnumerable<string> GetClasses() {

            return classes;

        }

        // Private members

        private readonly IEnumerable<string> classes = Enumerable.Empty<string>();

    }

}