namespace Gsemac.Forms.Styles.StyleSheets {

    public class StyleSheetOptions :
        IStyleSheetOptions {

        // Public members

        public bool CacheRulesets { get; set; } = true;
        public bool IgnoreInvalidProperties { get; set; } = false;

        public IFileReader FileReader { get; set; } = new FileSystemFileReader();

        public static StyleSheetOptions Default => new StyleSheetOptions();

    }

}