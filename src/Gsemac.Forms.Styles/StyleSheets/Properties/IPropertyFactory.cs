namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public interface IPropertyFactory {

        IProperty Create(string propertyName, IPropertyValue[] arguments);

    }

}