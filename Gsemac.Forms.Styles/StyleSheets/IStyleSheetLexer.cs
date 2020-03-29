using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public interface IStyleSheetLexer :
        IDisposable {

        bool ReadNextToken(out IStyleSheetLexerToken token);

    }

}