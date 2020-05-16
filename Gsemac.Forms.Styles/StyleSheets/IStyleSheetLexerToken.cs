namespace Gsemac.Forms.Styles.StyleSheets {

    public enum StyleSheetLexerTokenType {

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

    public interface IStyleSheetLexerToken {

        StyleSheetLexerTokenType Type { get; }
        string Value { get; }

    }

}