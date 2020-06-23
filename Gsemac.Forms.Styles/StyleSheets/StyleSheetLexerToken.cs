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

        public override string ToString() {

            return $"{Type}: {Value}";

        }

    }

}