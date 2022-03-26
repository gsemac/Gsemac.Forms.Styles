using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Forms.Styles.Dom.Extensions {

    public static class DomNodeExtensions {

        // Public members

        public static IEnumerable<IDomNode> GetParents(this IDomNode node) {

            if (node is null)
                throw new ArgumentNullException(nameof(node));

            IDomNode parent = node.Parent;

            while (parent is object) {

                yield return parent;

                parent = parent.Parent;

            }

        }
        public static IEnumerable<IDomNode> GetSiblings(this IDomNode node) {

            if (node is null)
                throw new ArgumentNullException(nameof(node));

            return node.Parent is null ?
                Enumerable.Empty<IDomNode>() :
                node.Parent.Children.Where(child => child != node);

        }

    }

}