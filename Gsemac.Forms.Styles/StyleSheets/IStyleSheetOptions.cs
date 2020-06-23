namespace Gsemac.Forms.Styles.StyleSheets {

    public interface IStyleSheetOptions {

        bool CacheRulesets { get; set; }
        bool IgnoreInvalidProperties { get; set; }

        IFileReader FileReader { get; set; }

    }

}