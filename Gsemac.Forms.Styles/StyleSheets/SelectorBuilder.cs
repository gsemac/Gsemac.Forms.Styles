using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class SelectorBuilder :
        ISelectorBuilder {

        // Public members

        public void AddId(string name) {

            rhsSelector.Add(new IDSelector(name));

        }
        public void AddClass(string name) {

            rhsSelector.Add(new ClassSelector(name));

        }
        public void AddTag(string name) {

            rhsSelector.Add(new TagSelector(name));

        }

        public void AddSelector() {

            CommitCurrentSelector(false);

        }
        public void AddDescendantCombinator() {

            CommitCurrentSelector(true);

            currentCombinator = CombinatorType.Descendant;

        }
        public void AddChildCombinator() {

            CommitCurrentSelector(true);

            currentCombinator = CombinatorType.Child;

        }
        public void AddAdjacentSiblingCombinator() {

            CommitCurrentSelector(true);

            currentCombinator = CombinatorType.AdjacentSibling;

        }
        public void AddGeneralSiblingCombinator() {

            CommitCurrentSelector(true);

            currentCombinator = CombinatorType.GeneralSibling;

        }

        public ISelector Build() {

            CommitCurrentSelector(false);

            return (completedSelectors.Count() == 1) ? completedSelectors.First() : new UnionSelector(completedSelectors);

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

        private void CommitCurrentSelector(bool isLhsOfCombinator) {

            if (rhsSelector.Any()) {

                ISelector committingSelector = (rhsSelector.Count() == 1) ? rhsSelector.First() : new Selector(rhsSelector);

                switch (currentCombinator) {

                    case CombinatorType.Descendant:

                        throw new NotImplementedException();

                    case CombinatorType.Child:

                        if (lhsSelector is null)
                            throw new InvalidOperationException("The selector on the left-hand side of the combinator was empty.");

                        completedSelectors.Add(new ChildSelector(lhsSelector, committingSelector));

                        lhsSelector = null;

                        break;

                    case CombinatorType.AdjacentSibling:

                        throw new NotImplementedException();

                    case CombinatorType.GeneralSibling:

                        throw new NotImplementedException();

                    default:

                        if (!isLhsOfCombinator) {

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

    }

}