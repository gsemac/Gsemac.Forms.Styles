using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.Dom {

    public interface INode2 {

        event EventHandler<StyleInvalidatedEventArgs> StyleInvalidated;
        event EventHandler<StylesChangedEventArgs> StylesChanged;

        string Tag { get; }
        string Id { get; }
        INode2 Parent { get; set; }
        ICollection<INode2> Children { get; }
        ICollection<string> Classes { get; }
        ICollection<NodeState> States { get; }
        ICollection<IRuleset> Styles { get; }

        IRuleset GetComputedStyle();

    }

}