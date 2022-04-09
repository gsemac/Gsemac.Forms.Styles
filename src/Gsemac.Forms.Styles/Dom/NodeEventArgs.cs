using System;

namespace Gsemac.Forms.Styles.Dom {

    public abstract class NodeEventArgs :
        EventArgs {

        // Public members

        public INode2 Node { get; }

        // Protected members

        protected NodeEventArgs(INode2 node) {

            if(node is null)
                throw new ArgumentNullException(nameof(node));

            Node = node;

        }

    }

}