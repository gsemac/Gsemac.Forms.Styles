namespace Gsemac.Forms.Styles.StyleSheets {

    public interface IStyleSheetOptions {

        string ColorScheme { get; }
        bool IgnoreInvalidProperties { get; }
        StyleOrigin Origin { get; }

    }

}