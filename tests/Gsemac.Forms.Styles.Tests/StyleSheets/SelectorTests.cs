﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Forms.Styles.StyleSheets.Tests {

    [TestClass]
    public class SelectorTests {

        [TestMethod]
        public void TestSelectorWithMultiplePseudoClasses() {

            string selectorStr = ".class:active:hover";
            ISelector selector = Selector.Parse(selectorStr);

            UserNode node = new UserNode(string.Empty, new[] { "class" });

            node.AddState(NodeStates.Active);
            node.AddState(NodeStates.Hover);

            Assert.IsTrue(selector.IsMatch(node));

        }

    }

}