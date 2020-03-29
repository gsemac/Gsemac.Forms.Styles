using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class StyleSheetLexerToken :
        IStyleSheetLexerToken {

        // Public members

        public StyleSheetLexerTokenType Type { get; }
        public string Value { get; }

        public StyleSheetLexerToken(StyleSheetLexerTokenType type, string value) {

            this.Type = type;
            this.Value = value;

        }

    }

}