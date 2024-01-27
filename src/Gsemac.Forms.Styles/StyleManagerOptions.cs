namespace Gsemac.Forms.Styles {

    public class StyleManagerOptions :
        IStyleManagerOptions {

        // Public members

        public bool CustomRenderingEnabled { get; set; } = true;
        public bool CustomScrollBarsEnabled { get; set; } = true;
        public bool DefaultStylesEnabled { get; set; } = true;
        public bool RequireNonDefaultStyles { get; set; } = true;

        public static StyleManagerOptions Default => new StyleManagerOptions();

    }

}