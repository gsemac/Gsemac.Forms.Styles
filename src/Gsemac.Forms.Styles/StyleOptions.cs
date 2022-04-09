namespace Gsemac.Forms.Styles {

    public class StyleOptions :
        IStyleOptions {

        // Public members

        public bool RequireExplicitStyles { get; set; }
        public bool ApplyToChildren { get; set; }

        public static StyleOptions Default => new StyleOptions() {
            ApplyToChildren = true,
            RequireExplicitStyles = true,
        };

        public StyleOptions() { }
        public StyleOptions(IStyleOptions options) {

            RequireExplicitStyles = options.RequireExplicitStyles;
            ApplyToChildren = options.ApplyToChildren;

        }

    }

}