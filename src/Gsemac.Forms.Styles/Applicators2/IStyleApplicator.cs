using Gsemac.Forms.Styles.Dom;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;

namespace Gsemac.Forms.Styles.Applicators2 {

    public interface IStyleApplicator {

        void InitializeObject(object obj);
        void DeinitializeObject(object obj);

        void ApplyStyle(object obj, IRuleset ruleset);

    }

    public interface IStyleApplicator<T> :
        IStyleApplicator {

        void InitializeObject(T obj);
        void DeinitializeObject(T obj);

        void ApplyStyle(T obj, IRuleset ruleset);

    }

}