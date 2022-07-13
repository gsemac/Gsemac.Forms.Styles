using Gsemac.Collections.Extensions;
using Gsemac.Forms.Styles.StyleSheets.Dom;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public class ComputeContext :
        IComputeContext {

        // Public members

        public ComputeContext(IPropertyFactory propertyFactory) {

            this.propertyFactory = propertyFactory;

        }

        public IRuleset ComputeStyle(INode2 node, IEnumerable<IRuleset> styles) {

            // Begin by building up the initial set of rules.

            IRuleset ruleset = new Ruleset();

            foreach (IRuleset style in styles)
                ruleset.AddRange(style);

            if (node.Parent is object)
                ruleset.Inherit(node.Parent.GetComputedStyle(this));

            // TODO: Resolve all variable references.

            return ruleset;

        }

        // Private members

        public readonly IPropertyFactory propertyFactory;

    }

}