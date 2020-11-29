using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public interface ILexer<T> :
        IDisposable {

        bool EndOfStream { get; }

        bool Read(out T token);
        T Peek();

    }

}