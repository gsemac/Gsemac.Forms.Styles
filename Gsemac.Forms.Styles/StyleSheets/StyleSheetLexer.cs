using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class StyleSheetLexer :
        IStyleSheetLexer {

        // Public members

        public StyleSheetLexer(Stream stream) {

            reader = new StreamReader(stream);

        }

        public bool ReadNextToken(out IStyleSheetLexerToken token) {

            ReadNextTokens();

            if (tokens.Any()) {

                token = tokens.Dequeue();

                return true;

            }
            else {

                token = null;

                return false;

            }


        }

        public void Dispose() {

            Dispose(true);

        }

        // Protected members

        protected virtual void Dispose(bool disposing) {

            if (!disposedValue) {

                if (disposing) {

                    reader.Dispose();

                }

                disposedValue = true;
            }

        }

        // Private members

        private readonly StreamReader reader;
        private bool insideDeclaration = false;
        private bool disposedValue = false;

        private readonly Queue<IStyleSheetLexerToken> tokens = new Queue<IStyleSheetLexerToken>();

        private void ReadNextTokens() {

            if (!reader.EndOfStream) {

                // Skip leading whitespace.

                SkipWhitespace();

                switch ((char)reader.Peek()) {

                    case '{':
                        ReadDeclarationStart();
                        break;

                    case '}':
                        ReadDeclarationEnd();
                        break;

                    case ':':
                        ReadPropertyValueSeparator();
                        SkipWhitespace();
                        ReadPropertyValue();
                        break;

                    case ';':
                        ReadPropertyEnd();
                        break;

                    default:

                        if (insideDeclaration)
                            ReadPropertyName();
                        else
                            ReadSelector();

                        break;

                }

            }

        }

        private void SkipWhitespace() {

            while (char.IsWhiteSpace((char)reader.Peek()) && !reader.EndOfStream)
                reader.Read();

        }

        private void ReadDeclarationStart() {

            StringBuilder valueBuilder = new StringBuilder();

            valueBuilder.Append((char)reader.Read());

            if (insideDeclaration)
                throw new InvalidTokenException(valueBuilder.ToString());

            insideDeclaration = true;

            tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.DeclarationStart, valueBuilder.ToString()));

        }
        private void ReadDeclarationEnd() {

            StringBuilder valueBuilder = new StringBuilder();

            valueBuilder.Append((char)reader.Read());

            if (!insideDeclaration)
                throw new InvalidTokenException(valueBuilder.ToString());

            insideDeclaration = false;

            tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.DeclarationEnd, valueBuilder.ToString()));

        }
        private void ReadPropertyName() {

            StringBuilder valueBuilder = new StringBuilder();

            while (!char.IsWhiteSpace((char)reader.Peek()) && (char)reader.Peek() != ':' && !reader.EndOfStream)
                valueBuilder.Append((char)reader.Read());

            tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.PropertyName, valueBuilder.ToString()));

        }
        private void ReadPropertyValueSeparator() {

            string value = ((char)reader.Read()).ToString();

            tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.PropertyValueSeparator, value));

        }
        private void ReadPropertyValue() {

            StringBuilder buffer = new StringBuilder();

            bool exitLoop = false;

            SkipWhitespace();

            while (!reader.EndOfStream && !exitLoop) {

                char nextChar = (char)reader.Peek();

                switch (nextChar) {

                    case ';':
                    case ')':

                        // We've reached the end of the property or set of function arguments.

                        if (buffer.Length > 0)
                            tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.String, buffer.ToString()));

                        exitLoop = true;

                        break;

                    case ',':

                        // We've reached the end of the current value (of a comma-delimited set of values).

                        if (buffer.Length > 0)
                            tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.String, buffer.ToString().Trim()));

                        buffer.Clear();

                        tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.FunctionArgumentSeparator, ((char)reader.Read()).ToString()));

                        SkipWhitespace();

                        break;

                    case '(':

                        // We've reached the start of function arguments.
                        // The contents of the buffer must have been a function name.

                        tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.Function, buffer.ToString()));

                        buffer.Clear();

                        ReadFunctionArguments();

                        break;

                    default:

                        // Append the next character to the buffer (we're not sure what it is yet).

                        buffer.Append((char)reader.Read());

                        break;

                }

            }

        }
        private void ReadFunctionArguments() {

            // Read the opening delimiter.

            tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.FunctionArgumentsStart, ((char)reader.Read()).ToString()));

            // Read the function arguments.

            ReadPropertyValue();

            // Read the closing delimiter.

            if (!reader.EndOfStream)
                tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.FunctionArgumentsEnd, ((char)reader.Read()).ToString()));

        }
        private void ReadPropertyEnd() {

            string value = ((char)reader.Read()).ToString();

            tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.PropertyEnd, value));

        }
        private void ReadSelector() {

            StringBuilder valueBuilder = new StringBuilder();

            while ((char)reader.Peek() != '{' && !reader.EndOfStream)
                valueBuilder.Append((char)reader.Read());

            tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.Selector, valueBuilder.ToString().Trim()));

        }

    }

}