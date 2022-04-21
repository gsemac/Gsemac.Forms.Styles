namespace Gsemac.Forms.Styles.StyleSheets.Lexers {

    internal interface IStyleSheetLexerToken {

        StyleSheetLexerTokenType Type { get; }
        string Value { get; }

    }

}