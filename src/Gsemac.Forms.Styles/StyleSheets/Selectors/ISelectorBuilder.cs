namespace Gsemac.Forms.Styles.StyleSheets.Selectors {

    public interface ISelectorBuilder {

        void AddId(string name);
        void AddClass(string name);
        void AddPseudoClass(string name);
        void AddTag(string name);

        void AddSelector();
        void AddDescendantCombinator();
        void AddChildCombinator();
        void AddAdjacentSiblingCombinator();
        void AddGeneralSiblingCombinator();

        ISelector Build();

    }

}