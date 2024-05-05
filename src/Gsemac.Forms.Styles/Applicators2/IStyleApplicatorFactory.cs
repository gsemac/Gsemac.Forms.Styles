using System;

namespace Gsemac.Forms.Styles.Applicators2 {

    internal interface IStyleApplicatorFactory {

        IStyleApplicator Create(Type forType);

    }

}