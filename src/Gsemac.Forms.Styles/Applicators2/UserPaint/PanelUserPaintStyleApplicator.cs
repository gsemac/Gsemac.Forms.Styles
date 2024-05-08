using System;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators2.UserPaint {

    internal sealed class PanelUserPaintStyleApplicator :
        UserPaintStyleApplicator<Panel> {

        // Public members

        public override void InitializeStyle(Panel panel) {

            if (panel is null)
                throw new ArgumentNullException(nameof(panel));

            // ResizeRedraw needs to be set to true to prevent smearing.
            // https://stackoverflow.com/a/39419274/5383169

            ControlUtilities.SetResizeRedraw(panel, true);


        }

    }

}