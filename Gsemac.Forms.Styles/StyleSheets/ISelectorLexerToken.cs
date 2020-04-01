using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public enum SelectorLexerTokenType {
        Class,
        Id,
        DescendantCombinator, // " "
        ChildCombinator, // ">"
        AdjacentSiblingCombinator, // "+"
        GeneralSiblingCombinator, // "~",
        Comma
    }

    public interface ISelectorLexerToken {

        SelectorLexerTokenType Type { get; }
        string Value { get; }

    }

}