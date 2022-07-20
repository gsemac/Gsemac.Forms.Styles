using Gsemac.Forms.Styles.StyleSheets.Dom;
using System;

namespace Gsemac.Forms.Styles.StyleSheets.Selectors {

    public static class SelectorExtensions {

        // Public members

        public static bool IsMatch(this ISelector selector, INode2 node) {

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            if (node is null)
                throw new ArgumentNullException(nameof(node));

            return selector.Match(node)?.Success ?? false;

        }

    }

}