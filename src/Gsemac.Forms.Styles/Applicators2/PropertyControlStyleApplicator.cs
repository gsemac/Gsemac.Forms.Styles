using Gsemac.Forms.Styles.StyleSheets;
using Gsemac.Forms.Styles.StyleSheets.Extensions;
using System;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators2 {

    public class PropertyControlStyleApplicator<T> :
        ControlStyleApplicatorBase<T> where T : Control {

        // Public members

        public override void ApplyStyle(T obj, IRuleset style) {

            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            if (style is null)
                throw new ArgumentNullException(nameof(style));

            Control control = obj;

            if (style.BackgroundColor.HasValue())
                control.BackColor = style.BackgroundColor.Value;

            if (style.Color.HasValue())
                control.ForeColor = style.Color.Value;

        }

    }

}