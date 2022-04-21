using Gsemac.Forms.Styles.Properties;
using Gsemac.Forms.Styles.StyleSheets.Lexers;
using Gsemac.Forms.Styles.StyleSheets.Properties;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using Gsemac.Forms.Styles.StyleSheets.Selectors;
using System;
using System.Collections.Generic;
using System.IO;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class StyleSheetFactory :
        IStyleSheetFactory {

        // Public members

        public static StyleSheetFactory Default => new StyleSheetFactory();

        public StyleSheetFactory() :
            this(PropertyFactory.Default) {
        }
        public StyleSheetFactory(IPropertyFactory propertyFactory) {

            if (propertyFactory is null)
                throw new ArgumentNullException(nameof(propertyFactory));

            this.propertyFactory = propertyFactory;

        }

        public IStyleSheet FromStream(Stream stream, IStyleSheetOptions options) {

            IList<IRuleset> rulesets = new List<IRuleset>();

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
                        case StyleSheetLexerTokenType.Function: {

                                IPropertyValue[] propertyValues = ReadPropertyValues(lexer);
                                IProperty property = null;

                                try {

                                    property = propertyFactory.Create(currentPropertyName, propertyValues);

                                }
                                catch (Exception ex) {

                                    if (!options.IgnoreInvalidProperties)
                                        throw ex;

                                }

                                if (property != null)
                                    currentRuleset.Add(property);

                            }

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

            return new StyleSheet(rulesets);

        }

        // Private members

        private readonly IPropertyFactory propertyFactory;

        private ISelector ReadSelector(IStyleSheetLexer lexer) {

            return Selector.FromLexer(lexer);

        }
        private IPropertyValue[] ReadPropertyValues(IStyleSheetLexer lexer) {

            List<IPropertyValue> values = new List<IPropertyValue>();

            bool exitLoop = false;

            while (!exitLoop && !lexer.EndOfStream) {

                IStyleSheetLexerToken token = lexer.Peek();

                switch (token.Type) {

                    case StyleSheetLexerTokenType.Value:

                        lexer.Read(out _); // Eat the token

                        if (PropertyValue.TryParse(token.Value, out PropertyValue parsedPropertyValue))
                            values.Add(parsedPropertyValue);

                        break;

                    case StyleSheetLexerTokenType.Function:

                        lexer.Read(out _); // Eat the token

                        values.Add(ReadFunction(lexer, token.Value));

                        break;

                    case StyleSheetLexerTokenType.PropertyEnd:

                        lexer.Read(out _); // Eat the token

                        exitLoop = true;

                        break;

                    case StyleSheetLexerTokenType.DeclarationEnd:

                        // Don't eat the token, so that it can be seen by ReadStream.
                        // Encountering this token means the style sheet is malformed, but we want to handle it gracefully.

                        exitLoop = true;

                        break;

                }

            }

            return values.ToArray();

        }
        private IPropertyValue ReadFunction(IStyleSheetLexer lexer, string functionName) {

            List<IPropertyValue> functionArgs = new List<IPropertyValue>();

            bool exitLoop = false;

            while (!exitLoop && lexer.Read(out IStyleSheetLexerToken token)) {

                switch (token.Type) {

                    case StyleSheetLexerTokenType.Value:

                        if (PropertyValue.TryParse(token.Value, out PropertyValue parsedPropertyValue))
                            functionArgs.Add(parsedPropertyValue);

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
                        throw new UnexpectedTokenException(string.Format(ExceptionMessages.UnexpectedTokenTypeWithType, token.Type));

                }

            }

            IPropertyValue returnValue = PropertyFunctions.EvaluateFunction(functionName, functionArgs.ToArray());

            return returnValue;

        }

    }

}