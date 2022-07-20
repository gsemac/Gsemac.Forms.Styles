using Gsemac.Forms.Styles.StyleSheets.Dom;
using Gsemac.Forms.Styles.StyleSheets.Properties;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets.Rulesets {

    public interface IStyleComputationContext {

        IProperty ComputeProperty(IProperty property, INode2 node, IEnumerable<IRuleset> styles);
        IRuleset ComputeStyle(INode2 node, IEnumerable<IRuleset> styles);

    }

}