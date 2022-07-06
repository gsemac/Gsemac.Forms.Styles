namespace Gsemac.Forms.Styles.StyleSheets {

    public class StyleSheetOptions :
        IStyleSheetOptions {

        // Public members

        public string ColorScheme { get; set; }
        public bool IgnoreInvalidProperties { get; set; } = false;
        public StyleOrigin Origin { get; set; } = StyleOrigin.User;

        public static StyleSheetOptions Default => new StyleSheetOptions();

    }

}