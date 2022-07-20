namespace Gsemac.Forms.Styles.StyleSheets.Selectors {

    public interface ISelectorBuilder {

        void WithId(string value);
        void WithClass(string value);
        void WithPseudoClass(string value);
        void WithTag(string value);
        void WithUniversal();

        void WithSelector();
        void WithDescendantCombinator();
        void WithChildCombinator();
        void WithAdjacentSiblingCombinator();
        void WithGeneralSiblingCombinator();

        ISelector Build();

    }

}