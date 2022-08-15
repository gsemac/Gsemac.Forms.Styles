using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets.Dom {

    public interface INode2 {

        event EventHandler<NodeCollectionChangedEventArgs> ChildAdded;
        event EventHandler<NodeCollectionChangedEventArgs> ChildRemoved;
        /// <summary>
        /// Occurs when changes are made to the node that may invalidate previously-matching selectors.
        /// </summary>
        event EventHandler<NodeEventArgs> SelectorChanged;
        /// <summary>
        /// Occurs when styles are added or removed from node.
        /// </summary>
        event EventHandler<NodeEventArgs> StylesChanged;

        string Tag { get; }
        string Id { get; }
        INode2 Parent { get; set; }
        ICollection<INode2> Children { get; }
        ICollection<string> Classes { get; }
        ICollection<NodeState> States { get; }
        ICollection<IRuleset> Styles { get; }

        IRuleset GetComputedStyle(IStyleComputationContext context);

    }

}