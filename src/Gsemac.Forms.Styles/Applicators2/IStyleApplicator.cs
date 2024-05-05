using Gsemac.Forms.Styles.StyleSheets.Rulesets;

namespace Gsemac.Forms.Styles.Applicators2 {

    internal interface IStyleApplicator {

        void InitializeStyle(object obj);
        void DeinitializeStyle(object obj);

        void ApplyStyle(object obj, IRuleset ruleset);

    }

    internal interface IStyleApplicator<T> :
        IStyleApplicator {

        void InitializeStyle(T obj);
        void DeinitializeStyle(T obj);

        void ApplyStyle(T obj, IRuleset ruleset);

    }

}