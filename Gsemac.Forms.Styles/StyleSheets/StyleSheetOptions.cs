namespace Gsemac.Forms.Styles.StyleSheets {

    public class StyleSheetOptions :
        IStyleSheetOptions {

        // Public members

        public bool CacheRulesets { get; set; } = true;

        public IFileReader FileReader { get; set; } = new FileSystemFileReader();

    }

}