using Gsemac.Forms.Styles.StyleSheets.Lexers;
using System.IO;

namespace Gsemac.Forms.Styles.StyleSheets.Selectors {

    public class SelectorFactory :
        ISelectorFactory {

        // Public members

        public static SelectorFactory Default => new SelectorFactory();

        public ISelector FromStream(Stream stream) {

            using (IStyleSheetLexer lexer = new StyleSheetLexer(stream))
                return FromLexer(lexer);

        }

        // Internal members

        internal static ISelector FromLexer(IStyleSheetLexer lexer) {

            SelectorBuilder builder = new SelectorBuilder();

            bool exitLoop = false;

            while (!exitLoop && !lexer.EndOfStream) {

                IStyleSheetLexerToken token = lexer.Peek();

                switch (token.Type) {

                    case StyleSheetLexerTokenType.Class:
                        builder.WithClass(token.Value);
                        break;

                    case StyleSheetLexerTokenType.PseudoClass:
                        builder.WithPseudoClass(token.Value);
                        break;

                    case StyleSheetLexerTokenType.PseudoElement:
                        builder.AddPseudoElement(token.Value);
                        break;

                    case StyleSheetLexerTokenType.Id:
                        builder.WithId(token.Value);
                        break;

                    case StyleSheetLexerTokenType.Tag:
                        builder.WithTag(token.Value);
                        break;

                    case StyleSheetLexerTokenType.UniversalSelector:
                        builder.WithUniversal();
                        break;

                    case StyleSheetLexerTokenType.DescendantCombinator:
                        builder.WithDescendantCombinator();
                        break;

                    case StyleSheetLexerTokenType.ChildCombinator:
                        builder.WithChildCombinator();
                        break;

                    case StyleSheetLexerTokenType.AdjacentSiblingCombinator:
                        builder.WithAdjacentSiblingCombinator();
                        break;

                    case StyleSheetLexerTokenType.GeneralSiblingCombinator:
                        builder.WithGeneralSiblingCombinator();
                        break;

                    case StyleSheetLexerTokenType.SelectorSeparator:
                        builder.WithSelector();
                        break;

                    default:
                        exitLoop = true;
                        break;

                }

                if (!exitLoop)
                    lexer.Read(out _);

            }

            return builder.Build();

        }

    }

}