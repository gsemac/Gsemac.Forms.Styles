using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public enum StyleSheetLexerTokenType {

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
        FunctionArgumentsEnd, // ")"

        Class, // ".name"
        Id, // "#name"
        Tag, // "name", "*"
        DescendantCombinator, // " "
        ChildCombinator, // ">"
        AdjacentSiblingCombinator, // "+"
        GeneralSiblingCombinator, // "~"
        SelectorSeparator, // ","

    }

    public interface IStyleSheetLexerToken {

        StyleSheetLexerTokenType Type { get; }
        string Value { get; }

    }

}