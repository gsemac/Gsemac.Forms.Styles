using Gsemac.Collections.Specialized;
using System;

namespace Gsemac.Forms.Styles.Dom {

    internal class ChildNodeCollection :
        ObservableList<INode2> {

        // Public members

        public ChildNodeCollection(INode2 parentNode) {

            if (parentNode is null)
                throw new ArgumentNullException(nameof(parentNode));

            this.parentNode = parentNode;

        }

        public override void Add(INode2 item) {

            if (item is null)
                throw new ArgumentNullException(nameof(item));

            item.Parent = parentNode;

            base.Add(item);

        }
        public override bool Remove(INode2 item) {

            if (item is null)
                return false;

            if (base.Remove(item)) {

                item.Parent = null;

                return true;

            }
            else {

                return false;

            }

        }
        public override void Clear() {

            foreach (INode2 node in Items)
                node.Parent = null;

            base.Clear();

        }

        // Private members

        private readonly INode2 parentNode;

    }

}
