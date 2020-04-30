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
                Console.WriteLine(token.Type.ToString() + ": " + token.Value);
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
        private void SkipCharacter() {

            if (!reader.EndOfStream)
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

            StringBuilder valueBuilder = new StringBuilder();

            while ((char)reader.Peek() != ';' && !reader.EndOfStream)
                valueBuilder.Append((char)reader.Read());

            tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.PropertyValue, valueBuilder.ToString()));

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