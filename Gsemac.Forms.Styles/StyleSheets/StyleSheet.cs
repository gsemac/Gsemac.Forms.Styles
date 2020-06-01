using Gsemac.Forms.Styles.Collections;
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

            return GetRuleset(node, inherit, options.HasFlag(StylesheetOptions.CacheRulesets));
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

        public static StyleSheet Parse(string input, StylesheetOptions options = StylesheetOptions.Default) {

            using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(input)))
                return FromStream(stream, options);

        }
        public static StyleSheet FromStream(Stream stream, StylesheetOptions options = StylesheetOptions.Default) {

            StyleSheet result = new StyleSheet(options);

            result.ReadStream(stream);

            return result;

        }
        public static StyleSheet FromFile(string filePath, StylesheetOptions options = StylesheetOptions.Default) {

            using (FileStream fstream = new FileStream(filePath, FileMode.Open))
                return FromStream(fstream, options);

        }

        // Private members

        private readonly StylesheetOptions options = StylesheetOptions.Default;
        private readonly IList<IRuleset> rulesets = new List<IRuleset>();
        private readonly IDictionary<INode, IRuleset> cache = new RulesetCache();

        private StyleSheet(StylesheetOptions options = StylesheetOptions.Default) {

            this.options = options;

        }

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

                        case StyleSheetLexerTokenType.Value:
                        case StyleSheetLexerTokenType.Function:
                            currentRuleset.AddProperty(Property.Create(currentPropertyName, ReadPropertyValues(lexer)));
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
        private object[] ReadPropertyValues(IStyleSheetLexer lexer) {

            List<object> values = new List<object>();

            bool exitLoop = false;

            while (!exitLoop && !lexer.EndOfStream && lexer.Read(out IStyleSheetLexerToken token)) {

                switch (token.Type) {

                    case StyleSheetLexerTokenType.Value:
                        values.Add(token.Value);
                        break;

                    case StyleSheetLexerTokenType.Function:
                        values.Add(ReadFunction(lexer, token.Value));
                        break;

                    case StyleSheetLexerTokenType.PropertyEnd:
                        exitLoop = true;
                        break;

                }

            }

            return values.ToArray();

        }
        private object ReadFunction(IStyleSheetLexer lexer, string functionName) {

            List<object> functionArguments = new List<object>();

            bool exitLoop = false;

            while (!exitLoop && lexer.Read(out IStyleSheetLexerToken token)) {

                switch (token.Type) {

                    case StyleSheetLexerTokenType.Value:
                        functionArguments.Add(token.Value);
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

        private IRuleset GetRuleset(INode node, bool inherit, bool useCache) {

            IRuleset result = null;

            if (useCache) {

                result = GetRulesetFromCache(node, inherit);

            }
            else {

                result = new Ruleset();

                if (inherit && node.Parent != null)
                    result.InheritProperties(GetRuleset(node.Parent, inherit));

                foreach (IRuleset ruleset in rulesets.Where(r => r.Selector.IsMatch(node)))
                    result.AddProperties(ruleset);

            }

            return result;

        }
        private IRuleset GetRulesetFromCache(INode node, bool inherit) {

            if (cache.TryGetValue(node, out IRuleset ruleset)) {

                return ruleset;

            }
            else {

                ruleset = GetRuleset(node, inherit, false);

                if (ruleset != null)
                    cache[node] = ruleset;

                return ruleset;

            }

        }

    }

}