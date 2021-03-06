﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Gsemac.Forms.Styles.StyleSheets.Tests {

    [TestClass]
    public class StyleSheetsTests {

        [TestMethod]
        public void TestPropertyWithLeadingWhitespaceBeforeValue() {

            string styleSheetStr = "class { border-width:     10px; }";
            IStyleSheet styleSheet = StyleSheet.Parse(styleSheetStr);
            IRuleset ruleset = styleSheet.GetRuleset(new UserNode("class", Enumerable.Empty<string>()));

            Assert.AreEqual(10.0, ruleset.GetProperty(PropertyType.BorderWidth).Value);

        }
        [TestMethod]
        public void TestPropertyWithFollowingWhitespaceAfterValue() {

            string styleSheetStr = "class { border-width: 10px     ; }";
            IStyleSheet styleSheet = StyleSheet.Parse(styleSheetStr);
            IRuleset ruleset = styleSheet.GetRuleset(new UserNode("class", Enumerable.Empty<string>()));

            Assert.AreEqual(10.0, ruleset.GetProperty(PropertyType.BorderWidth).Value);

        }
        [TestMethod]
        public void TestPropertyWithNoLeadingWhitespaceBeforeValue() {

            string styleSheetStr = "class { border-width:10px; }";
            IStyleSheet styleSheet = StyleSheet.Parse(styleSheetStr);
            IRuleset ruleset = styleSheet.GetRuleset(new UserNode("class", Enumerable.Empty<string>()));

            Assert.AreEqual(10.0, ruleset.GetProperty(PropertyType.BorderWidth).Value);

        }
        [TestMethod]
        public void TestPropertyWithNoEndingDelimiter() {

            string styleSheetStr = "class { border-width: 10px }";
            IStyleSheet styleSheet = StyleSheet.Parse(styleSheetStr);
            IRuleset ruleset = styleSheet.GetRuleset(new UserNode("class", Enumerable.Empty<string>()));

            Assert.AreEqual(10.0, ruleset.GetProperty(PropertyType.BorderWidth).Value);

        }

    }

}