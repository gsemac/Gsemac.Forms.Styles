using Gsemac.Forms.Styles.StyleSheets.Dom;

namespace Gsemac.Forms.Styles.StyleSheets.Selectors {

    public interface ISelector {

        bool IsMatch(INode2 node);

    }

}