using System;

namespace Gsemac.Forms.Styles.Applicators2 {

    public interface IStyleApplicatorFactory {

        IStyleApplicator2 Create(Type forType);

    }

}