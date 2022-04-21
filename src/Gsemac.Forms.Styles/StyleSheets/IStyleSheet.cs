using Gsemac.Forms.Styles.StyleSheets.Dom;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets {

    public interface IStyleSheet :
        IEnumerable<IRuleset> {

        IEnumerable<IRuleset> GetRulesets(INode2 node);

    }

}