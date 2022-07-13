using Gsemac.Forms.Styles.StyleSheets.Dom;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public interface IComputeContext {

        IRuleset ComputeStyle(INode2 node, IEnumerable<IRuleset> styles);

    }

}