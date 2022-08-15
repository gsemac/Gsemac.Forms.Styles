using System;

namespace Gsemac.Forms.Styles.StyleSheets.Dom {

    public class NodeCollectionChangedEventArgs :
        NodeEventArgs {

        // Public members

        public INode2 AffectedNode { get; }

        public NodeCollectionChangedEventArgs(INode2 node, INode2 changedNode) :
            base(node) {

            if (changedNode is null)
                throw new ArgumentNullException(nameof(changedNode));

            AffectedNode = changedNode;

        }

    }

}