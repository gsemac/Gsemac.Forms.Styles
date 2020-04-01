using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public interface ISelectorLexer :
        IDisposable {

        bool ReadNextToken(out ISelectorLexerToken token);

    }

}