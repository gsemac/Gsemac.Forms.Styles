namespace Gsemac.Forms.Styles.Applicators {

    public class StyleApplicationOptions :
        IStyleApplicationOptions {

        // Public members

        public bool RequireStyle { get; set; } = true;
        public bool Recursive { get; set; } = true;

        public static StyleApplicationOptions Default => new StyleApplicationOptions();

        public StyleApplicationOptions() { }
        public StyleApplicationOptions(IStyleApplicationOptions options) {

            RequireStyle = options.RequireStyle;
            Recursive = options.Recursive;

        }

    }

}