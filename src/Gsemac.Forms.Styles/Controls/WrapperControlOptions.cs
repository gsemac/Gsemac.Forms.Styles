namespace Gsemac.Forms.Styles.Controls {

    internal class WrapperControlOptions :
        IWrapperControlOptions {

        // Public members

        public bool ForwardPaintEventsToChildControl { get; set; } = false;
        public bool OverrideScrollbars { get; set; } = false;

        public static WrapperControlOptions Default => new WrapperControlOptions();

    }

}