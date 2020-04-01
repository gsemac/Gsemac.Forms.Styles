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

            token = null;

            while (!reader.EndOfStream && token is null) {

                // Skip leading whitespace.

                SkipWhitespace();

                switch ((char)reader.Peek()) {

                    case '{':
                        token = ReadOpenDeclaration();
                        break;

                    case '}':
                        token = ReadCloseDeclaration();
                        break;

                    case ':':
                        SkipCharacter();
                        SkipWhitespace();
                        token = ReadPropertyValue();
                        break;

                    case ';':
                        SkipCharacter();
                        break;

                    default:

                        if (insideDeclaration)
                            token = ReadPropertyName();
                        else
                            token = ReadSelector();

                        break;

                }

            }

            return token != null;

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

        private void SkipWhitespace() {

            while (char.IsWhiteSpace((char)reader.Peek()) && !reader.EndOfStream)
                reader.Read();

        }
        private void SkipCharacter() {

            if (!reader.EndOfStream)
                reader.Read();

        }
        private IStyleSheetLexerToken ReadOpenDeclaration() {

            StringBuilder valueBuilder = new StringBuilder();

            valueBuilder.Append((char)reader.Read());

            if (insideDeclaration)
                throw new InvalidTokenException(valueBuilder.ToString());

            insideDeclaration = true;

            return new StyleSheetLexerToken(StyleSheetLexerTokenType.OpenDeclaration, valueBuilder.ToString());

        }
        private IStyleSheetLexerToken ReadCloseDeclaration() {

            StringBuilder valueBuilder = new StringBuilder();

            valueBuilder.Append((char)reader.Read());

            if (!insideDeclaration)
                throw new InvalidTokenException(valueBuilder.ToString());

            insideDeclaration = false;

            return new StyleSheetLexerToken(StyleSheetLexerTokenType.CloseDeclaration, valueBuilder.ToString());

        }
        private IStyleSheetLexerToken ReadPropertyName() {

            StringBuilder valueBuilder = new StringBuilder();

            while (!char.IsWhiteSpace((char)reader.Peek()) && (char)reader.Peek() != ':' && !reader.EndOfStream)
                valueBuilder.Append((char)reader.Read());

            return new StyleSheetLexerToken(StyleSheetLexerTokenType.PropertyName, valueBuilder.ToString());

        }
        private IStyleSheetLexerToken ReadPropertyValue() {

            StringBuilder valueBuilder = new StringBuilder();

            while ((char)reader.Peek() != ';' && !reader.EndOfStream)
                valueBuilder.Append((char)reader.Read());

            return new StyleSheetLexerToken(StyleSheetLexerTokenType.PropertyValue, valueBuilder.ToString());

        }
        private IStyleSheetLexerToken ReadSelector() {

            StringBuilder valueBuilder = new StringBuilder();

            while ((char)reader.Peek() != '{' && !reader.EndOfStream)
                valueBuilder.Append((char)reader.Read());

            return new StyleSheetLexerToken(StyleSheetLexerTokenType.Selector, valueBuilder.ToString().Trim());

        }

    }

}