namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public interface ILonghandPropertyDefinition :
        IPropertyDefinition {

        LonghandPropertyValueFactory ValueFactory { get; }

    }

}