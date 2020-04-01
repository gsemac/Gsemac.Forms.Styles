using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class SelectorLexer :
        LexerBase<ISelectorLexerToken>,
        ISelectorLexer {

        // Public members

        public SelectorLexer(Stream stream) :
            base(stream) {
        }

        public override bool ReadNextToken(out ISelectorLexerToken token) {

            token = null;

            SkipWhitespace();

            while (!Reader.EndOfStream && token is null) {

                switch ((char)Reader.Peek()) {

                    case '#':
                        token = ReadId();
                        break;

                    case '.':
                        token = ReadClass();
                        break;

                    case ' ':
                    case '>':
                    case '+':
                    case '~':
                        token = ReadCombinator();
                        SkipWhitespace();
                        break;

                    case ',':
                        token = ReadComma();
                        SkipWhitespace();
                        break;

                }

            }

            return token != null;

        }

        // Private members

        private ISelectorLexerToken ReadId() {

            StringBuilder valueBuilder = new StringBuilder();

            while (!char.IsWhiteSpace((char)Reader.Peek()) && Reader.Peek() != '.' && !Reader.EndOfStream)
                valueBuilder.Append((char)Reader.Read());

            return new SelectorLexerToken(SelectorLexerTokenType.Id, valueBuilder.ToString());

        }
        private ISelectorLexerToken ReadClass() {

            StringBuilder valueBuilder = new StringBuilder();

            valueBuilder.Append((char)Reader.Read()); // read "."

            while (!char.IsWhiteSpace((char)Reader.Peek()) && Reader.Peek() != '.' && Reader.Peek() != '#' && !Reader.EndOfStream)
                valueBuilder.Append((char)Reader.Read());

            return new SelectorLexerToken(SelectorLexerTokenType.Class, valueBuilder.ToString());

        }
        private ISelectorLexerToken ReadCombinator() {

            StringBuilder valueBuilder = new StringBuilder();

            SkipWhitespace();

            switch ((char)Reader.Peek()) {

                case '>':
                    valueBuilder.Append((char)Reader.Read());
                    return new SelectorLexerToken(SelectorLexerTokenType.ChildCombinator, valueBuilder.ToString());

                case '+':
                    valueBuilder.Append((char)Reader.Read());
                    return new SelectorLexerToken(SelectorLexerTokenType.AdjacentSiblingCombinator, valueBuilder.ToString());

                case '~':
                    valueBuilder.Append((char)Reader.Read());
                    return new SelectorLexerToken(SelectorLexerTokenType.GeneralSiblingCombinator, valueBuilder.ToString());

                default:
                    valueBuilder.Append(' '); // do not consume the character
                    return new SelectorLexerToken(SelectorLexerTokenType.DescendantCombinator, valueBuilder.ToString());

            }

        }
        private ISelectorLexerToken ReadComma() {

            StringBuilder valueBuilder = new StringBuilder();

            valueBuilder.Append((char)Reader.Read());

            return new SelectorLexerToken(SelectorLexerTokenType.Comma, valueBuilder.ToString());

        }

    }

}