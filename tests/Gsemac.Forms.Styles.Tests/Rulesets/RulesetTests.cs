using Gsemac.Forms.Styles.StyleSheets.Properties;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Forms.Styles.Rulesets.Tests {

    [TestClass]
    public class RulesetTests {

        // Add

        [TestMethod]
        public void TestAddingLonghandPropertyDoesNotAddLonghandProperties() {

            // Adding a longhand property to the ruleset should not add any additional (longhand) properties that were not specified.

            IRuleset ruleset = new Ruleset {
                PropertyFactory.Default.Create(PropertyName.BorderWidth, new BorderWidths(8)),
            };

            Assert.AreEqual(1, ruleset.Count);

        }

        [TestMethod]
        public void TestGetLonghandValueAfterAddingLonghandProperty() {

            // The value should match the value of the property we added.

            IRuleset ruleset = new Ruleset {
                PropertyFactory.Default.Create(PropertyName.BorderBottomWidth, new LineWidth(8)),
            };

            Assert.AreEqual(8, (int)ruleset.BorderBottomWidth.Value);

        }
        [TestMethod]
        public void TestGetLonghandValueAfterAddingShorthandProperty() {

            // The value should match the value of the corresponding longhand of the property we added.

            IRuleset ruleset = new Ruleset {
                PropertyFactory.Default.Create(PropertyName.BorderWidth, new BorderWidths(8)),
            };

            Assert.AreEqual(8, (int)ruleset.BorderBottomWidth.Value);

        }
        [TestMethod]
        public void TestPropertyValueIsUpdatedWhenAddingEquivalentLonghandProperty() {

            // We should always get the latest value of a property, even if it's set by a shorthand property.

            IRuleset ruleset = new Ruleset {
                PropertyFactory.Default.Create(PropertyName.BorderWidth, new BorderWidths(8)),
                PropertyFactory.Default.Create(PropertyName.BorderBottomWidth, new LineWidth(9)),
            };

            Assert.AreEqual(9, (int)ruleset.BorderBottomWidth.Value);

        }
        [TestMethod]
        public void TestPropertyValueIsUpdatedWhenAddingEquivalentShorthandProperty() {

            // We should always get the latest value of a property, even if it's set by a shorthand property.

            IRuleset ruleset = new Ruleset {
                PropertyFactory.Default.Create(PropertyName.BorderBottomWidth, new LineWidth(8)),
                PropertyFactory.Default.Create(PropertyName.BorderWidth, new BorderWidths(9)),
            };

            Assert.AreEqual(9, (int)ruleset.BorderBottomWidth.Value);

        }
        [TestMethod]
        public void TestPropertyValueIsUpdatedWhenRemovingEquivalentLonghandProperty() {

            // If a property is removed, the property value should revert back to its former value.

            IRuleset ruleset = new Ruleset {
                PropertyFactory.Default.Create(PropertyName.BorderWidth, new BorderWidths(8)),
                PropertyFactory.Default.Create(PropertyName.BorderBottomWidth, new LineWidth(9)),
            };

            ruleset.Remove(PropertyName.BorderBottomWidth);

            Assert.AreEqual(8, (int)ruleset.BorderBottomWidth.Value);

        }
        [TestMethod]
        public void TestPropertyValueIsUpdatedWhenRemovingEquivalentShorthandProperty() {

            // If a property is removed, the property value should revert back to its former value.

            IRuleset ruleset = new Ruleset {
                PropertyFactory.Default.Create(PropertyName.BorderWidth, new BorderWidths(8)),
                PropertyFactory.Default.Create(PropertyName.BorderBottomWidth, new LineWidth(9)),
            };

            ruleset.Remove(PropertyName.BorderWidth);

            Assert.AreEqual(9, (int)ruleset.BorderBottomWidth.Value);

        }

    }

}