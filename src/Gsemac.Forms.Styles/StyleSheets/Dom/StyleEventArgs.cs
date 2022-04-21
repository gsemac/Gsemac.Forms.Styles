using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;

namespace Gsemac.Forms.Styles.StyleSheets.Dom {

    public class StyleEventArgs :
        EventArgs {

        // Public members

        public IRuleset Style { get; }

        public StyleEventArgs(IRuleset style) {

            if (style is null)
                throw new ArgumentNullException(nameof(style));

            Style = style;

        }

    }

}