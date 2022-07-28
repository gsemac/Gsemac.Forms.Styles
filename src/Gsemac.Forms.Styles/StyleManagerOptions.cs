namespace Gsemac.Forms.Styles {

    public class StyleManagerOptions :
        IStyleManagerOptions {

        // Public members

        public bool EnableCustomRendering { get; set; } = true;

        public static StyleManagerOptions Default => new StyleManagerOptions();

    }

}