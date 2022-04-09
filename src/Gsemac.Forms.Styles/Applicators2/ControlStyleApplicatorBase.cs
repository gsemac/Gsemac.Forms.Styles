using System;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators2 {

    public abstract class ControlStyleApplicatorBase<T> :
        StyleApplicatorBase<T> where T : Control {

        // Public members

        public override void InitializeTarget(T obj) {

            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            Control control = obj;

            ControlUtilities.SetDoubleBuffered(control, true);

        }

    }

}