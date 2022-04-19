using System;

namespace Gsemac.Forms.Styles.Applicators2 {

    public interface INodeStyleApplicatorFactory {

        IStyleApplicator Create(Type forType);

    }

}