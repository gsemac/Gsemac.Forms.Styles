using Gsemac.Forms.Styles.StyleSheets.Dom;
using System;

namespace Gsemac.Forms.Styles.StyleSheets.Selectors {

    public class PseudoElementSelector :
      ISelector {

        // Public members

        public int Specificity => SelectorUtilities.GetSpecificity(WeightCategory.Type);

        public PseudoElementSelector(string elementName) {

            this.elementName = FormatElementName(elementName);

        }

        public ISelectorMatch Match(INode2 node) {

            if (node is null)
                throw new ArgumentNullException(nameof(node));

            if (string.IsNullOrEmpty(elementName))
                return SelectorMatch.Failure;

            throw new NotImplementedException();

        }


        public override string ToString() {

            return $"::{elementName}";

        }

        // Private members

        private readonly string elementName;

        private static string FormatElementName(string elementName) {

            if (string.IsNullOrWhiteSpace(elementName))
                return string.Empty;

            if (elementName.StartsWith("::"))
                elementName = elementName.Substring(2, elementName.Length - 2);

            elementName = elementName.Trim();

            return elementName;

        }

    }

}