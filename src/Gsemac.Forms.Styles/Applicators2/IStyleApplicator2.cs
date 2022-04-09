using Gsemac.Forms.Styles.StyleSheets;

namespace Gsemac.Forms.Styles.Applicators2 {

    public interface IStyleApplicator2 {

        void InitializeTarget(object obj);
        void DeinitializeTarget(object obj);

        void ApplyStyle(object obj, IRuleset style);

    }

    public interface IStyleApplicator<T> :
        IStyleApplicator2 {

        void InitializeTarget(T obj);
        void DeinitializeTarget(T obj);

        void ApplyStyle(T obj, IRuleset style);

    }

}