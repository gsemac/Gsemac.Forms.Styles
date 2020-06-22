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

            return GetRuleset(node, inherit, options.CacheRulesets);
        }

        public IEnumerator<IRuleset> GetEnumerator() {

            return rulesets.GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        public void Dispose() {

            Dispose(true);

        }

        public override string ToString() {

            StringBuilder sb = new StringBuilder();

            foreach (IRuleset ruleSet in rulesets)
                sb.AppendLine(ruleSet.ToString());

            return sb.ToString();

        }

        public static IStyleSheet Parse(string input) {

            return Parse(input, new StylesheetOptions());

        }
        public static IStyleSheet Parse(string input, IStylesheetOptions options) {

            using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(input)))
                return FromStream(stream, options);

        }
        public static IStyleSheet FromStream(Stream stream) {

            return FromStream(stream, new StylesheetOptions());

        }
        public static IStyleSheet FromStream(Stream stream, IStylesheetOptions options) {

            StyleSheet result = new StyleSheet(options);

            result.ReadStream(stream);

            return result;

        }
        public static IStyleSheet FromFile(string filePath) {

            // The FileReader is set such that files are read relative to the stylesheet.

            return FromFile(filePath, new StylesheetOptions() {
                FileReader = new FileSystemFileReader() {
                    RootPath = Path.GetDirectoryName(filePath)
                }
            });

        }
        public static IStyleSheet FromFile(string filePath, IStylesheetOptions options) {

            using (FileStream fstream = new FileStream(filePath, FileMode.Open))
                return FromStream(fstream, options);

        }

        // Protected members

        protected virtual void Dispose(bool disposing) {

            if (disposing) {

                foreach (IDisposable disposable in disposableResources)
                    disposable.Dispose();

                disposableResources.Clear();

            }

        }

        // Private members

        private readonly IStylesheetOptions options = new StylesheetOptions();
        private readonly IList<IRuleset> rulesets = new List<IRuleset>();
        private readonly IDictionary<INode, IRuleset> cache = new RulesetCache();
        private readonly IList<IDisposable> disposableResources = new List<IDisposable>();

        private StyleSheet(IStylesheetOptions options) {

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
        private StyleObject[] ReadPropertyValues(IStyleSheetLexer lexer) {

            List<StyleObject> values = new List<StyleObject>();

            bool exitLoop = false;

            while (!exitLoop && !lexer.EndOfStream && lexer.Read(out IStyleSheetLexerToken token)) {

                switch (token.Type) {

                    case StyleSheetLexerTokenType.Value:
                        values.Add(new StyleObject(token.Value));
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
        private StyleObject ReadFunction(IStyleSheetLexer lexer, string functionName) {

            List<StyleObject> functionArgs = new List<StyleObject>();

            bool exitLoop = false;

            while (!exitLoop && lexer.Read(out IStyleSheetLexerToken token)) {

                switch (token.Type) {

                    case StyleSheetLexerTokenType.Value:
                        functionArgs.Add(new StyleObject(token.Value));
                        break;

                    case StyleSheetLexerTokenType.Function:
                        functionArgs.Add(ReadFunction(lexer, token.Value));
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

            StyleObject returnValue = PropertyUtilities.EvaluateFunction(functionName, functionArgs.ToArray(), options.FileReader);

            if (returnValue.Type == StyleObjectType.Image)
                disposableResources.Add(returnValue.GetImage());

            return returnValue;

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