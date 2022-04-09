using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Forms.Styles.Dom.Extensions {

    public static class DomNodeExtensions {

        // Public members

        public static IEnumerable<INode2> GetParents(this INode2 node) {

            if (node is null)
                throw new ArgumentNullException(nameof(node));

            INode2 parent = node.Parent;

            while (parent is object) {

                yield return parent;

                parent = parent.Parent;

            }

        }
        public static IEnumerable<INode2> GetSiblings(this INode2 node) {

            if (node is null)
                throw new ArgumentNullException(nameof(node));

            return node.Parent is null ?
                Enumerable.Empty<INode2>() :
                node.Parent.Children.Where(child => child != node);

        }

    }

}