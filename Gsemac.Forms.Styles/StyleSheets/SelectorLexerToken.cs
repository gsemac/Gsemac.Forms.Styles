using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class SelectorLexerToken :
        ISelectorLexerToken {

        // Public members

        public SelectorLexerTokenType Type { get; }
        public string Value { get; }

        public SelectorLexerToken(SelectorLexerTokenType type, string value) {

            this.Type = type;
            this.Value = value;

        }

    }

}