namespace Gsemac.Forms.Styles.StyleSheets.Selectors {

    internal static class SelectorUtilities {

        // Public members

        public static int GetSpecificity(WeightCategory weightCategory) {

            switch (weightCategory) {

                case WeightCategory.Id:
                    return 100;

                case WeightCategory.Class:
                    return 10;

                case WeightCategory.Type:
                    return 1;

                default:
                    return 0;

            }

        }

    }

}