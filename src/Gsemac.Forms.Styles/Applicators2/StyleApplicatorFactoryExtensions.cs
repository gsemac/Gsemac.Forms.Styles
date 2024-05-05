using System;

namespace Gsemac.Forms.Styles.Applicators2 {

    public static class StyleApplicatorFactoryExtensions {

        // Public members

        public static IStyleApplicator<T> Create<T>(this IStyleApplicatorFactory styleApplicatorFactory) {

            if (styleApplicatorFactory is null)
                throw new ArgumentNullException(nameof(styleApplicatorFactory));

            return (IStyleApplicator<T>)styleApplicatorFactory.Create(typeof(T));

        }

    }

}