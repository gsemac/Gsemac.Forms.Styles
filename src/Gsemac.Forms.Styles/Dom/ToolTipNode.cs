using System;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Dom {

    internal sealed class ToolTipNode :
        StyleSheets.Dom.NodeBase {

        // Public members

        public ToolTip ToolTip { get; }

        public ToolTipNode(ToolTip toolTip) :
            base("ToolTip") {

            if (toolTip is null)
                throw new ArgumentNullException(nameof(toolTip));

            ToolTip = toolTip;

            Classes.Add("ToolTip");

        }

    }

}