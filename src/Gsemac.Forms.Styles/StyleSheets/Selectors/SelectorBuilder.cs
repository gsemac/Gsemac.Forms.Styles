using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets.Selectors {

    public class SelectorBuilder :
        ISelectorBuilder {

        // Public members

        public void WithId(string name) {

            rhsSelector.Add(new IdSelector(name));

        }
        public void WithClass(string name) {

            rhsSelector.Add(new ClassSelector(name));

        }
        public void WithPseudoClass(string name) {

            rhsSelector.Add(new PseudoClassSelector(name));

        }
        public void AddPseudoElement(string name) {

            rhsSelector.Add(new PseudoElementSelector(name));

        }
        public void WithTag(string name) {

            rhsSelector.Add(new TypeSelector(name));

        }
        public void WithUniversal() {

            rhsSelector.Add(new UniversalSelector());

        }

        public void WithSelector() {

            CommitCurrentSelector(isLhsSelector: false);

        }
        public void WithDescendantCombinator() {

            CommitCurrentSelector(true);

            currentCombinator = CombinatorType.Descendant;

        }
        public void WithChildCombinator() {

            CommitCurrentSelector(true);

            currentCombinator = CombinatorType.Child;

        }
        public void WithAdjacentSiblingCombinator() {

            CommitCurrentSelector(true);

            currentCombinator = CombinatorType.AdjacentSibling;

        }
        public void WithGeneralSiblingCombinator() {

            CommitCurrentSelector(true);

            currentCombinator = CombinatorType.GeneralSibling;

        }

        public ISelector Build() {

            CommitCurrentSelector(false);

            return completedSelectors.Count() == 1 ? completedSelectors.First() : new UnionSelector(completedSelectors);

        }

        public void Clear() {

            lhsSelector = null;
            completedSelectors.Clear();
            rhsSelector.Clear();

            currentCombinator = CombinatorType.None;

        }

        public override string ToString() {

            return Build().ToString();

        }

        // Private members

        private enum CombinatorType {
            None,
            Descendant,
            Child,
            AdjacentSibling,
            GeneralSibling
        }

        private ISelector lhsSelector = null;
        private readonly IList<ISelector> rhsSelector = new List<ISelector>();
        private readonly IList<ISelector> completedSelectors = new List<ISelector>();
        private CombinatorType currentCombinator = CombinatorType.None;

        private void CommitCurrentSelector(bool isLhsSelector) {

            if (rhsSelector.Any()) {

                ISelector committingSelector = rhsSelector.Count() == 1 ? rhsSelector.First() : new CompositeSelector(rhsSelector);

                switch (currentCombinator) {

                    case CombinatorType.Descendant:

                        ThrowIfLhsSelectorIsNull();

                        completedSelectors.Add(new DescendantSelector(lhsSelector, committingSelector));

                        lhsSelector = null;

                        break;

                    case CombinatorType.Child:

                        ThrowIfLhsSelectorIsNull();

                        completedSelectors.Add(new ChildSelector(lhsSelector, committingSelector));

                        lhsSelector = null;

                        break;

                    case CombinatorType.AdjacentSibling:

                        throw new NotImplementedException();

                    case CombinatorType.GeneralSibling:

                        throw new NotImplementedException();

                    default:

                        if (!isLhsSelector) {

                            // If this selector is not the left-hand side of a combinator, just add the selector directly to the list.

                            completedSelectors.Add(committingSelector);

                        }
                        else {

                            // Otherwise, set this selector as the left-hand side of the combinator.

                            lhsSelector = committingSelector;

                        }

                        break;

                }

            }

            rhsSelector.Clear();
            currentCombinator = CombinatorType.None;

        }

        private void ThrowIfLhsSelectorIsNull() {

            if (lhsSelector is null)
                throw new InvalidOperationException("The selector on the left-hand side of the combinator was empty.");

        }

    }

}