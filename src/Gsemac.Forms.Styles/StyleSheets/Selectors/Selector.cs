using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class Selector :
        ISelector {

        // Public members

        public Selector(ISelector selector) {

            this.selectors = new List<ISelector>() {
                selector
            };

        }
        public Selector(IEnumerable<ISelector> selectors) {

            this.selectors = selectors.ToArray();

        }

        public bool IsMatch(INode node) {

            return selectors.All(selector => selector.IsMatch(node));

        }

        public override string ToString() {

            return string.Join(string.Empty, selectors.Select(selector => selector.ToString()));

        }

        public static ISelector Parse(string selector) {

            using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(selector)))
                return FromStream(stream);

        }
        public static ISelector FromStream(Stream stream) {

            using (IStyleSheetLexer lexer = new StyleSheetLexer(stream))
                return FromLexer(lexer);

        }
        public static ISelector FromLexer(IStyleSheetLexer lexer) {

            SelectorBuilder builder = new SelectorBuilder();

            bool exitLoop = false;

            while (!exitLoop && !lexer.EndOfStream) {

                IStyleSheetLexerToken token = lexer.Peek();

                switch (token.Type) {

                    case StyleSheetLexerTokenType.Class:
                        builder.AddClass(token.Value);
                        break;

                    case StyleSheetLexerTokenType.PseudoClass:
                        builder.AddPseudoClass(token.Value);
                        break;

                    case StyleSheetLexerTokenType.PseudoElement:
                        builder.AddPseudoElement(token.Value);
                        break;

                    case StyleSheetLexerTokenType.Id:
                        builder.AddId(token.Value);
                        break;

                    case StyleSheetLexerTokenType.Tag:
                        builder.AddTag(token.Value);
                        break;

                    case StyleSheetLexerTokenType.DescendantCombinator:
                        builder.AddDescendantCombinator();
                        break;

                    case StyleSheetLexerTokenType.ChildCombinator:
                        builder.AddChildCombinator();
                        break;

                    case StyleSheetLexerTokenType.AdjacentSiblingCombinator:
                        builder.AddAdjacentSiblingCombinator();
                        break;

                    case StyleSheetLexerTokenType.GeneralSiblingCombinator:
                        builder.AddGeneralSiblingCombinator();
                        break;

                    case StyleSheetLexerTokenType.SelectorSeparator:
                        builder.AddSelector();
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

        // Private members

        private readonly IEnumerable<ISelector> selectors;

    }

}