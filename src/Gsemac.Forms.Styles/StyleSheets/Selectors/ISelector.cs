using Gsemac.Forms.Styles.StyleSheets.Dom;

namespace Gsemac.Forms.Styles.StyleSheets.Selectors {

    public interface ISelector {

        int Specificity { get; }

        ISelectorMatch Match(INode2 node);

    }

}