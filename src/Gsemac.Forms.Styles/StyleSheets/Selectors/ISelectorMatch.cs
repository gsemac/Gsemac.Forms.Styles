namespace Gsemac.Forms.Styles.StyleSheets.Selectors {

    public interface ISelectorMatch {

        bool Success { get; }

        ISelector Selector { get; }
        int Specificity { get; }

    }

}