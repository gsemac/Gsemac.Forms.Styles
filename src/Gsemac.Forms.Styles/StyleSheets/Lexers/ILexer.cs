using System;

namespace Gsemac.Forms.Styles.StyleSheets.Lexers {

    internal interface ILexer<T> :
        IDisposable {

        bool EndOfStream { get; }

        bool Read(out T token);
        T Peek();

    }

}