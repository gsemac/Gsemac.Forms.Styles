namespace Gsemac.Forms.Styles {

    public class StyleManagerOptions :
        IStyleManagerOptions {

        // Public members

        public bool EnableCustomRendering { get; set; } = true;
        public bool EnableDefaultStyles { get; set; } = true;
        public bool RequireNonDefaultStyles { get; set; } = true;

        public static StyleManagerOptions Default => new StyleManagerOptions();

    }

}