using Gsemac.Forms.Styles.StyleSheets.Rulesets;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public interface IPropertyFactory {

        IProperty Create(string propertyName, IPropertyValue[] arguments, IRuleset ruleset);

    }

}