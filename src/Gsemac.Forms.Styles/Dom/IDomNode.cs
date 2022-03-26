using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.Dom {

    public interface IDomNode {

        event EventHandler<ClassesChangedEventArgs> ClassAdded;
        event EventHandler<ClassesChangedEventArgs> ClassRemoved;
        event EventHandler<StylesChangedEventArgs> StyleAdded;
        event EventHandler<StylesChangedEventArgs> StyleRemoved;

        string Tag { get; }
        string Id { get; }
        IDomNode Parent { get; set; }
        ICollection<string> Classes { get; }
        ICollection<IDomNode> Children { get; }
        ICollection<IRuleset> Styles { get; }

        IRuleset GetComputedStyle();

    }

}