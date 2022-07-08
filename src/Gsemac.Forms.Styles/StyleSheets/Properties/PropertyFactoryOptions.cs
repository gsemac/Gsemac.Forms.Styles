namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public class PropertyFactoryOptions :
        IPropertyFactoryOptions {

        // Public members

        public bool AllowUndefinedProperties { get; set; } = true;

        public static PropertyFactoryOptions Default => new PropertyFactoryOptions();

    }

}