using Gsemac.Forms.Styles.StyleSheets.Rulesets;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public interface IPropertyInitialValueFactory {

        IPropertyValue GetInitialValue(string propertyName, IRuleset style);
        T GetInitialValue<T>(string propertyName, IRuleset style);

    }

}