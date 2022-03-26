using Gsemac.Forms.Styles.StyleSheets;
using System;

namespace Gsemac.Forms.Styles.Dom {

    public class StylesChangedEventArgs :
        EventArgs {

        // Public members

        public IRuleset Ruleset { get; }

        public StylesChangedEventArgs(IRuleset ruleset) {

            Ruleset = ruleset;

        }

    }

}