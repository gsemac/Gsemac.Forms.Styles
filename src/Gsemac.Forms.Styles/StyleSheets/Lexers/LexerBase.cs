using System.Collections.Generic;
using System.IO;

namespace Gsemac.Forms.Styles.StyleSheets.Lexers {

    internal abstract class LexerBase<T> :
         ILexer<T> {

        public bool EndOfStream => EqualityComparer<T>.Default.Equals(Peek(), default);

        public abstract bool Read(out T token);
        public abstract T Peek();

        public void Dispose() {

            Dispose(true);

        }

        // Protected members

        protected StreamReader Reader { get; }

        protected LexerBase(Stream stream) {

            Reader = new StreamReader(stream);

        }

        protected void SkipWhitespace() {

            while (!Reader.EndOfStream && char.IsWhiteSpace((char)Reader.Peek()))
                Reader.Read();

        }
        protected void SkipCharacter() {

            if (!Reader.EndOfStream)
                Reader.Read();

        }

        protected virtual void Dispose(bool disposing) {

            if (!disposedValue) {

                if (disposing) {

                    Reader.Dispose();

                }

                disposedValue = true;
            }

        }

        // Private members

        private bool disposedValue = false;

    }

}