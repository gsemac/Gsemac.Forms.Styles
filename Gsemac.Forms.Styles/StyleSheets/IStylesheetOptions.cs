namespace Gsemac.Forms.Styles.StyleSheets {

    public interface IStylesheetOptions {

        bool CacheRulesets { get; set; }

        IFileReader FileReader { get; set; }

    }

}