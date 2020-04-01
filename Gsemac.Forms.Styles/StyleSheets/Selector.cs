using Gsemac.Forms.Styles.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class Selector :
        ISelector {

        // Public members

        public Selector(string selector) {

            this.selector = selector;

            ParseSelector(selector);

        }

        public bool IsMatch(INode node) {

            return selectors.Any(selector => IsMatch(node, selector));

        }

        public override string ToString() {

            return selector;

        }

        // Private members

        private readonly string selector;
        private readonly List<List<ISelectorLexerToken>> selectors = new List<List<ISelectorLexerToken>>();

        private void ParseSelector(string selector) {

            selectors.Clear();

            selectors.Add(new List<ISelectorLexerToken>());

            using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(selector)))
            using (SelectorLexer lexer = new SelectorLexer(stream)) {

                foreach (ISelectorLexerToken token in lexer) {

                    switch (token.Type) {

                        case SelectorLexerTokenType.Comma:

                            selectors.Add(new List<ISelectorLexerToken>());
                            break;

                        default:

                            selectors.Last().Add(token);
                            break;

                    }

                }

            }

        }
        private bool IsMatch(INode node, IEnumerable<ISelectorLexerToken> selector) {

            // We'll handle the selector in reverse, since we're starting with a child node rather than a parent node.

            bool isMatch = true;

            foreach (ISelectorLexerToken token in selector.Reverse()) {
                Console.WriteLine(token.Value);
                switch (token.Type) {

                    case SelectorLexerTokenType.Class:

                        // Check if the node has at least one matching class.

                        if (!node.Classes.Any(c => c.Equals(token.GetName(), StringComparison.OrdinalIgnoreCase)) && !token.Value.Equals("*"))
                            isMatch = false;

                        break;

                    case SelectorLexerTokenType.Id:

                        // Check if the node's ID matches.

                        if (!node.Id.Equals(token.GetName(), StringComparison.OrdinalIgnoreCase))
                            isMatch = false;

                        break;

                }

                if (!isMatch)
                    break;

            }

            return isMatch;

        }

    }

}