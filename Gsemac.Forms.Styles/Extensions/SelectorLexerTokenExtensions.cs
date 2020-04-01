using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.Extensions {

    public static class SelectorLexerTokenExtensions {

        public static string GetName(this ISelectorLexerToken token) {

            switch (token.Type) {

                case SelectorLexerTokenType.Class:
                    return token.Value.TrimStart('.');

                case SelectorLexerTokenType.Id:
                    return token.Value.TrimStart('#');

                default:
                    return token.Value;

            }

        }

    }

}