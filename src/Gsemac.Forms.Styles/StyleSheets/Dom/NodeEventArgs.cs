using System;

namespace Gsemac.Forms.Styles.StyleSheets.Dom {

    public class NodeEventArgs :
        EventArgs {

        // Public members

        public INode2 CurrentNode { get; }

        public NodeEventArgs(INode2 currentNode) {

            if (currentNode is null)
                throw new ArgumentNullException(nameof(currentNode));

            CurrentNode = currentNode;

        }

    }

}