using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.StyleSheets {

    [Flags]
    public enum StylesheetOptions {
        None = 0,
        CacheRulesets = 1,
        Default = CacheRulesets
    }

    public interface IStyleSheet :
        IEnumerable<IRuleset>,
        IDisposable {

        IRuleset GetRuleset(INode node, bool inherit = true);

    }

}