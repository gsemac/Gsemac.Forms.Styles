using System;

namespace Gsemac.Forms.Styles.StyleSheets.Selectors {

    internal class SelectorMatch :
        ISelectorMatch {

        // Public members

        public bool Success { get; } = false;

        public ISelector Selector { get; } = Selectors.Selector.Empty;
        public int Specificity => Selector.Specificity;

        public static SelectorMatch Failure => new SelectorMatch();

        public SelectorMatch(ISelector selector) {

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            Success = true;
            Selector = selector;

        }

        // Private members

        public SelectorMatch() { }

    }

}