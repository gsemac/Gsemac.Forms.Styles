using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.StyleSheets {

    public interface IStyleSheet :
        IEnumerable<IRuleset> {

        IRuleset GetRuleset(INode node, bool inherit = true);

    }

}