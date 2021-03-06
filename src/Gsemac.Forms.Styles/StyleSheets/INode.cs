﻿using System;
using System.Collections.Generic;

namespace Gsemac.Forms.Styles.StyleSheets {

    [Flags]
    public enum NodeStates {
        None = 0,
        Active = 1,
        Hover = 2,
        Checked = 4,
        Focus = 8,
        FocusWithin = 16,
        Disabled = 32,
        Enabled = 64
    }

    public interface INode {

        string Tag { get; }
        IEnumerable<string> Classes { get; }
        IEnumerable<string> PseudoClasses { get; }
        string PseudoElement { get; }
        string Id { get; }
        NodeStates States { get; }

        INode Parent { get; }

    }

}