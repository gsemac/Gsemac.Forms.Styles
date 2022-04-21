namespace Gsemac.Forms.Styles.StyleSheets.Lexers {

    internal enum StyleSheetLexerTokenType {

        DeclarationStart, // "{"
        DeclarationEnd, // "}"
        PropertyName,
        PropertyValueSeparator, // ":"
        PropertyEnd, // ";"
        Value,
        Function, // "rgb", "url", etc.
        FunctionArgumentsStart, // "("
        FunctionArgumentSeparator, // ","
        FunctionArgumentsEnd, // ")"

        Class, // ".name"
        PseudoClass, // ":focus"
        PseudoElement, // "::selection"
        Id, // "#name"
        Tag, // "name", "*"
        DescendantCombinator, // " "
        ChildCombinator, // ">"
        AdjacentSiblingCombinator, // "+"
        GeneralSiblingCombinator, // "~"
        SelectorSeparator, // ","

        CommentStart, // "/*"
        Comment, // comment
        CommentEnd, // "*/"

    }

}