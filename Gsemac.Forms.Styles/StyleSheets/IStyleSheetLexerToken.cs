using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public enum StyleSheetLexerTokenType {
        Selector,
        OpenDeclaration,
        CloseDeclaration,
        PropertyName,
        PropertyValue
    }

    public interface IStyleSheetLexerToken {

        StyleSheetLexerTokenType Type { get; }
        string Value { get; }

    }

}