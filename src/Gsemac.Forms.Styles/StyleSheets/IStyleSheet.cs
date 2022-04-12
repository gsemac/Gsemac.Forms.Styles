using Gsemac.Forms.Styles.Dom;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets {

    public interface IStyleSheet :
        IEnumerable<IRuleset> {

        IEnumerable<IRuleset> GetRulesets(INode2 node);

    }

}