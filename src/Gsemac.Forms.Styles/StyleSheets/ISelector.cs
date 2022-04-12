using Gsemac.Forms.Styles.Dom;

namespace Gsemac.Forms.Styles.StyleSheets {

    public interface ISelector {

        bool IsMatch(INode2 node);

    }

}