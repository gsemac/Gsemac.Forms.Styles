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

                IRuleset currentRuleset = null;
                string currentPropertyName = string.Empty;

                while (!lexer.EndOfStream) {

                    IStyleSheetLexerToken token = lexer.Peek();

                    switch (token.Type) {

                        case StyleSheetLexerTokenType.DeclarationEnd:
                            rulesets.Add(currentRuleset);
                            break;

                        case StyleSheetLexerTokenType.PropertyName:
                            currentPropertyName = token.Value;
                            break;

                        case StyleSheetLexerTokenType.String:
                        case StyleSheetLexerTokenType.Function:
                            currentRuleset.AddProperty(Property.Create(currentPropertyName, ReadPropertyValue(lexer)));
                            continue;

                        case StyleSheetLexerTokenType.Tag:
                        case StyleSheetLexerTokenType.Class:
                        case StyleSheetLexerTokenType.Id:
                            currentRuleset = new Ruleset(ReadSelector(lexer));
                            continue;

                    }

                    // Consume the current token.

                    lexer.Read(out _);

                }

            }

        }

        private ISelector ReadSelector(IStyleSheetLexer lexer) {

            return Selector.FromLexer(lexer);

        }
        private object ReadPropertyValue(IStyleSheetLexer lexer) {

            object result = null;

            if (lexer.Read(out IStyleSheetLexerToken token)) {

                switch (token.Type) {

                    case StyleSheetLexerTokenType.String:
                        result = token.Value;
                        break;

                    case StyleSheetLexerTokenType.Function:
                        result = ReadFunction(lexer, token.Value);
                        break;

                }

            }

            return result;

        }
        private object ReadFunction(IStyleSheetLexer lexer, string functionName) {

            List<object> functionArguments = new List<object>();

            bool exitLoop = false;

            while (!exitLoop && lexer.Read(out IStyleSheetLexerToken token)) {

                switch (token.Type) {

                    case StyleSheetLexerTokenType.String:
                        functionArguments.Add(token.Value);
                        break;

                    case StyleSheetLexerTokenType.Number:
                        functionArguments.Add(PropertyUtilities.ParseNumber(token.Value));
                        break;

                    case StyleSheetLexerTokenType.Function:
                        functionArguments.Add(ReadFunction(lexer, token.Value));
                        break;

                    case StyleSheetLexerTokenType.FunctionArgumentsStart:
                        break;

                    case StyleSheetLexerTokenType.FunctionArgumentSeparator:
                        break;

                    case StyleSheetLexerTokenType.FunctionArgumentsEnd:
                        exitLoop = true;
                        break;

                    default:
                        throw new UnexpectedTokenException(token.Type.ToString());

                }

            }

            switch ((functionName ?? "").Trim().ToLowerInvariant()) {

                case "rgb":
                    return PropertyUtilities.Rgb(Convert.ToInt32(functionArguments[0]), Convert.ToInt32(functionArguments[1]), Convert.ToInt32(functionArguments[2]));

                default:
                    throw new InvalidFunctionException(functionName);

            }

        }

    }

}