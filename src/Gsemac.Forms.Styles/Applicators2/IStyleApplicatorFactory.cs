using System;

namespace Gsemac.Forms.Styles.Applicators2 {

    public interface IStyleApplicatorFactory {

        IStyleApplicator Create(Type forType);

    }

}