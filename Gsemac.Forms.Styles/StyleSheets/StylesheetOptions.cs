namespace Gsemac.Forms.Styles.StyleSheets {

    public class StylesheetOptions :
        IStylesheetOptions {

        // Public members

        public bool CacheRulesets { get; set; } = true;

        public IFileReader FileReader { get; set; } = new FileSystemFileReader();

    }

}