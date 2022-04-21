namespace Gsemac.Forms.Styles.StyleSheets.Lexers {

    internal class StyleSheetLexerToken :
        IStyleSheetLexerToken {

        // Public members

        public StyleSheetLexerTokenType Type { get; }
        public string Value { get; }

        public StyleSheetLexerToken(StyleSheetLexerTokenType type, string value) {

            Type = type;
            Value = value;

        }

        public override string ToString() {

            return $"{Type}: {Value}";

        }

    }

}