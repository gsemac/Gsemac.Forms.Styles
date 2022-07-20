using Gsemac.Forms.Styles.Dom;
using Gsemac.Forms.Styles.StyleSheets.Selectors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Forms.Styles.Selectors.Tests {

    [TestClass]
    public class SelectorTests {

        [TestMethod]
        public void TestSelectorWithMultiplePseudoClasses() {

            string selectorStr = ".class:active:hover";
            ISelector selector = SelectorFactory.Default.Parse(selectorStr);

            UserNode node = new UserNode(string.Empty, new[] { "class" });

            node.AddState(NodeStates.Active);
            node.AddState(NodeStates.Hover);

            Assert.IsTrue(selector.IsMatch(node));

        }

    }

}