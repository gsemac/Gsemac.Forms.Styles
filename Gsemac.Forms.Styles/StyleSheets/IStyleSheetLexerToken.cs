using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public enum StyleSheetLexerTokenType {
        Selector,
        DeclarationStart, // "{"
        DeclarationEnd, // "}"
        PropertyName,
        PropertyValueSeparator, // ":"
        PropertyEnd, // ";"
        Color,
        String,
        Number,
        Units, // "px", "em", etc.
        Function, // "rgb", "url", etc.
        FunctionArgumentsStart, // "("
        FunctionArgumentSeparator, // ","
        FunctionArgumentsEnd // ")"
    }

    public interface IStyleSheetLexerToken {

        StyleSheetLexerTokenType Type { get; }
        string Value { get; }

    }

}