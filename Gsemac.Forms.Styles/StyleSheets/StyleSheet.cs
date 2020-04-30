using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class StyleSheet :
        IStyleSheet {

        // Public members

        public IRuleset GetRuleset(INode node, bool inherit = true) {

            IRuleset result = new Ruleset();

            if (inherit && node.Parent != null)
                result.InheritProperties(GetRuleset(node.Parent));

            foreach (IRuleset ruleset in rulesets.Where(r => r.Selector.IsMatch(node)))
                result.AddProperties(ruleset);

            return result;

        }

        public IEnumerator<IRuleset> GetEnumerator() {

            return rulesets.GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        public override string ToString() {

            StringBuilder sb = new StringBuilder();

            foreach (IRuleset ruleSet in rulesets)
                sb.AppendLine(ruleSet.ToString());

            return sb.ToString();

        }

        public static StyleSheet FromStream(Stream stream) {

            StyleSheet result = new StyleSheet();

            result.ReadStream(stream);

            return result;

        }

        // Private members

        private readonly List<IRuleset> rulesets = new List<IRuleset>();

        private StyleSheet() { }

        private void ReadStream(Stream stream) {

            using (IStyleSheetLexer lexer = new StyleSheetLexer(stream)) {

                IRuleset currentRuleSet = null;
                string currentPropertyName = string.Empty;

                while (lexer.ReadNextToken(out IStyleSheetLexerToken token)) {

                    switch (token.Type) {

                        case StyleSheetLexerTokenType.Selector:
                            currentRuleSet = new Ruleset(token.Value);
                            break;

                        case StyleSheetLexerTokenType.DeclarationStart:
                            break;

                        case StyleSheetLexerTokenType.DeclarationEnd:
                            rulesets.Add(currentRuleSet);
                            break;

                        case StyleSheetLexerTokenType.PropertyName:
                            currentPropertyName = token.Value;
                            break;

                        case StyleSheetLexerTokenType.PropertyValue:
                            currentRuleSet.AddProperty(Property.Create(currentPropertyName, token.Value));
                            break;

                    }

                }

            }

        }

    }

}