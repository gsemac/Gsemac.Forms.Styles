using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets {

    public class SelectorBuilder :
        ISelectorBuilder {

        // Public members

        public void AddId(string name) {

            currentSelector.Add(new IDSelector(name));

        }
        public void AddClass(string name) {

            currentSelector.Add(new ClassSelector(name));

        }
        public void AddTag(string name) {

            currentSelector.Add(new TagSelector(name));

        }

        public void AddSelector() {

            CommitCurrentSelector();

        }
        public void AddDescendantCombinator() {

            CommitCurrentSelector();

            currentCombinator = CombinatorType.Descendant;

        }
        public void AddChildCombinator() {

            CommitCurrentSelector();

            currentCombinator = CombinatorType.Child;

        }
        public void AddAdjacentSiblingCombinator() {

            CommitCurrentSelector();

            currentCombinator = CombinatorType.AdjacentSibling;

        }
        public void AddGeneralSiblingCombinator() {

            CommitCurrentSelector();

            currentCombinator = CombinatorType.GeneralSibling;

        }

        public ISelector Build() {

            CommitCurrentSelector();

            return lastSelector;

        }

        public void Clear() {

            currentSelector.Clear();

            lastSelector = null;

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

        private readonly List<ISelector> currentSelector = new List<ISelector>();
        private ISelector lastSelector = null;
        private CombinatorType currentCombinator = CombinatorType.None;

        private void CommitCurrentSelector() {

            if (currentSelector.Any()) {

                ISelector selector = new Selector(currentSelector);

                switch (currentCombinator) {

                    case CombinatorType.Descendant:

                        throw new NotImplementedException();

                    case CombinatorType.Child:
                        lastSelector = new ChildSelector(lastSelector, selector);
                        break;

                    case CombinatorType.AdjacentSibling:

                        throw new NotImplementedException();

                    case CombinatorType.GeneralSibling:

                        throw new NotImplementedException();

                    default:

                        if (lastSelector is null)
                            lastSelector = selector;
                        else
                            lastSelector = new UnionSelector(new ISelector[] { lastSelector, selector });

                        break;

                }

            }

            currentSelector.Clear();
            currentCombinator = CombinatorType.None;

        }

    }

}