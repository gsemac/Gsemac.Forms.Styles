using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public abstract class LexerBase<T> :
         IEnumerable<T>,
         IDisposable {

        public abstract bool ReadNextToken(out T token);

        public IEnumerator<T> GetEnumerator() {

            while (ReadNextToken(out T token))
                yield return token;

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        public void Dispose() {

            Dispose(true);

        }

        // Protected members

        protected StreamReader Reader { get; }

        protected LexerBase(Stream stream) {

            Reader = new StreamReader(stream);

        }

        protected void SkipWhitespace() {

            while (char.IsWhiteSpace((char)Reader.Peek()) && !Reader.EndOfStream)
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