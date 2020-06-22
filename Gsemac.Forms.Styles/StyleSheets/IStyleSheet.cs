using System;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets {

    public interface IStyleSheet :
        IEnumerable<IRuleset>,
        IDisposable {

        IRuleset GetRuleset(INode node, bool inherit = true);

    }

}