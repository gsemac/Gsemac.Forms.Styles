using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class StyleSheetLexer :
        LexerBase<IStyleSheetLexerToken>,
        IStyleSheetLexer {

        // Public members

        public StyleSheetLexer(Stream stream) :
            base(stream) {
        }

        public override bool Read(out IStyleSheetLexerToken token) {

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
        public override IStyleSheetLexerToken Peek() {

            if (!tokens.Any())
                ReadNextTokens();

            return tokens.Any() ? tokens.Peek() : null;

        }

        // Private members

        private readonly char[] reservedChars = { ' ', '>', '+', '~', ',', '.', '#', '{', '}', ':' };
        private readonly Queue<IStyleSheetLexerToken> tokens = new Queue<IStyleSheetLexerToken>();
        private bool insideDeclaration = false;
        private bool readingFunctionArguments = false;

        private void ReadNextTokens() {

            if (!Reader.EndOfStream) {

                // Skip leading whitespace.

                SkipWhitespace();

                switch ((char)Reader.Peek()) {

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

                    case '/':
                        ReadComment();
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
        private string ReadNextChars(int count) {

            StringBuilder valueBuilder = new StringBuilder();

            for (int i = 0; !Reader.EndOfStream && i < count; ++i)
                valueBuilder.Append((char)Reader.Read());

            return valueBuilder.ToString();

        }

        private void ReadDeclarationStart() {

            StringBuilder valueBuilder = new StringBuilder();

            valueBuilder.Append((char)Reader.Read());

            if (insideDeclaration)
                throw new UnexpectedTokenException(valueBuilder.ToString());

            insideDeclaration = true;

            tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.DeclarationStart, valueBuilder.ToString()));

        }
        private void ReadDeclarationEnd() {

            StringBuilder valueBuilder = new StringBuilder();

            valueBuilder.Append((char)Reader.Read());

            if (!insideDeclaration)
                throw new UnexpectedTokenException(valueBuilder.ToString());

            insideDeclaration = false;

            tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.DeclarationEnd, valueBuilder.ToString()));

        }
        private void ReadPropertyName() {

            StringBuilder valueBuilder = new StringBuilder();

            while (!Reader.EndOfStream && !char.IsWhiteSpace((char)Reader.Peek()) && (char)Reader.Peek() != ':')
                valueBuilder.Append((char)Reader.Read());

            tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.PropertyName, valueBuilder.ToString()));

        }
        private void ReadPropertyValueSeparator() {

            string value = ((char)Reader.Read()).ToString();

            tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.PropertyValueSeparator, value));

        }
        private void ReadPropertyValue() {

            StringBuilder buffer = new StringBuilder();

            bool exitLoop = false;

            SkipWhitespace();

            while (!Reader.EndOfStream && !exitLoop) {

                char nextChar = (char)Reader.Peek();

                if (char.IsWhiteSpace(nextChar))
                    nextChar = ' ';

                switch (nextChar) {

                    case ';':
                    case ')':
                    case '}':

                        // We've reached the end of the property or set of function arguments.

                        if (buffer.Length > 0)
                            tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.Value, buffer.ToString().Trim()));

                        exitLoop = true;

                        break;

                    case ',':
                    case ' ':

                        if (readingFunctionArguments && char.IsWhiteSpace(nextChar)) {

                            // Allow whitespace in function arguments (they are delimited by commas instead).
                            // For example, "to right" should be considered a single token.

                            buffer.Append((char)Reader.Read());

                        }
                        else {

                            // We've reached the end of the current value (of a comma-delimited set of values).

                            if (buffer.Length > 0)
                                tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.Value, buffer.ToString().Trim()));

                            buffer.Clear();

                            if (nextChar == ',')
                                tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.FunctionArgumentSeparator, ((char)Reader.Read()).ToString()));

                            SkipWhitespace();

                        }

                        break;

                    case '(':

                        // We've reached the start of function arguments.
                        // The contents of the buffer must have been a function name.

                        tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.Function, buffer.ToString()));

                        buffer.Clear();

                        ReadFunctionArguments();

                        break;

                    case '/':

                        ReadComment();

                        break;

                    default:

                        // Append the next character to the buffer (we're not sure what it is yet).

                        buffer.Append((char)Reader.Read());

                        break;

                }

            }

        }
        private void ReadFunctionArguments() {

            // Read the opening delimiter.

            tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.FunctionArgumentsStart, ((char)Reader.Read()).ToString()));

            // Read the function arguments.

            readingFunctionArguments = true;

            ReadPropertyValue();

            readingFunctionArguments = false;

            // Read the closing delimiter.

            if (!Reader.EndOfStream)
                tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.FunctionArgumentsEnd, ((char)Reader.Read()).ToString()));

        }
        private void ReadPropertyEnd() {

            string value = ((char)Reader.Read()).ToString();

            tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.PropertyEnd, value));

        }

        private void ReadSelector() {

            bool exitLoop = false;
            bool possibleDescendantCombinator = false;

            SkipWhitespace();

            while (!exitLoop && !Reader.EndOfStream) {

                switch ((char)Reader.Peek()) {

                    case '#':

                        if (possibleDescendantCombinator)
                            tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.DescendantCombinator, " "));

                        possibleDescendantCombinator = false;

                        ReadId();

                        break;

                    case '.':

                        if (possibleDescendantCombinator)
                            tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.DescendantCombinator, " "));

                        possibleDescendantCombinator = false;

                        ReadClass();

                        break;

                    case ':':
                        ReadPseudoClassOrPseudoElement();
                        break;

                    case ' ':

                        possibleDescendantCombinator = true;

                        SkipWhitespace();

                        break;

                    case '>':
                    case '+':
                    case '~':

                        possibleDescendantCombinator = false;

                        ReadCombinator();
                        SkipWhitespace();

                        break;

                    case ',':

                        possibleDescendantCombinator = false;

                        ReadSelectorSeparator();
                        SkipWhitespace();

                        break;

                    case '/':
                        ReadComment();
                        break;

                    case '{':
                        exitLoop = true;
                        break;

                    default:

                        if (possibleDescendantCombinator)
                            tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.DescendantCombinator, " "));

                        possibleDescendantCombinator = false;

                        ReadTag();

                        break;

                }

            }

        }
        private void ReadId() {

            StringBuilder valueBuilder = new StringBuilder();

            valueBuilder.Append((char)Reader.Read()); // read "#"

            while (!Reader.EndOfStream) {

                char nextChar = (char)Reader.Peek();

                if (char.IsWhiteSpace(nextChar) || reservedChars.Contains(nextChar))
                    break;

                valueBuilder.Append((char)Reader.Read());

            }

            if (valueBuilder.Length > 0)
                tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.Id, valueBuilder.ToString()));

        }
        private void ReadClass() {

            StringBuilder valueBuilder = new StringBuilder();

            valueBuilder.Append((char)Reader.Read()); // read "."

            while (!Reader.EndOfStream) {

                char nextChar = (char)Reader.Peek();

                if (char.IsWhiteSpace(nextChar) || reservedChars.Contains(nextChar))
                    break;

                valueBuilder.Append((char)Reader.Read());

            }

            if (valueBuilder.Length > 0)
                tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.Class, valueBuilder.ToString()));

        }
        private void ReadPseudoClassOrPseudoElement() {

            StringBuilder valueBuilder = new StringBuilder();

            // Read the opening colon.

            valueBuilder.Append((char)Reader.Read());

            while (!Reader.EndOfStream) {

                char nextChar = (char)Reader.Peek();

                if (char.IsWhiteSpace(nextChar) || reservedChars.Contains(nextChar))
                    break;

                valueBuilder.Append((char)Reader.Read());

            }

            if (valueBuilder.Length > 0) {

                string value = valueBuilder.ToString();

                StyleSheetLexerTokenType type = value.StartsWith("::") ?
                    StyleSheetLexerTokenType.PseudoElement :
                    StyleSheetLexerTokenType.PseudoClass;

                tokens.Enqueue(new StyleSheetLexerToken(type, value));

            }

        }
        private void ReadTag() {

            StringBuilder valueBuilder = new StringBuilder();

            while (!Reader.EndOfStream) {

                char nextChar = (char)Reader.Peek();

                if (char.IsWhiteSpace(nextChar) || reservedChars.Contains(nextChar))
                    break;

                valueBuilder.Append((char)Reader.Read());

            }

            if (valueBuilder.Length > 0)
                tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.Tag, valueBuilder.ToString()));

        }
        private void ReadCombinator() {

            if (!Reader.EndOfStream) {

                switch ((char)Reader.Peek()) {

                    case '>':
                        tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.ChildCombinator, ((char)Reader.Read()).ToString()));
                        break;

                    case '+':
                        tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.AdjacentSiblingCombinator, ((char)Reader.Read()).ToString()));
                        break;

                    case '~':
                        tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.GeneralSiblingCombinator, ((char)Reader.Read()).ToString()));
                        break;

                    default:
                        tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.DescendantCombinator, ((char)Reader.Peek()).ToString())); // do not consume the character
                        break;

                }

            }

        }
        private void ReadSelectorSeparator() {

            tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.SelectorSeparator, ((char)Reader.Read()).ToString()));

        }

        private void ReadComment() {

            const string commentStartMarker = "/*";
            const string commendEndMarker = "*/";

            string value = ReadNextChars(2);

            if (value != commentStartMarker)
                throw new UnexpectedTokenException(value);

            tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.CommentStart, commentStartMarker));

            StringBuilder valueBuilder = new StringBuilder();

            bool commentIsClosed = false;

            while (!Reader.EndOfStream) {

                valueBuilder.Append((char)Reader.Read());

                if (valueBuilder.Length >= 2 && valueBuilder.ToString(valueBuilder.Length - 2, 2) == commendEndMarker) {

                    commentIsClosed = true;

                    break;

                }

            }

            if (valueBuilder.Length > 0) {

                string comment = valueBuilder.ToString();

                if (commentIsClosed)
                    comment = comment.Substring(0, comment.Length - 2);

                tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.Comment, comment));

                if (commentIsClosed)
                    tokens.Enqueue(new StyleSheetLexerToken(StyleSheetLexerTokenType.CommentEnd, commendEndMarker));

            }

        }

    }

}