using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators2 {

    public class PropertyStyleApplicatorFactory :
        IStyleApplicatorFactory {

        // Public members

        public PropertyStyleApplicatorFactory() {

            applicators.Add(typeof(Button), new ButtonPropertyControlStyleApplicator());

        }

        public IStyleApplicator2 Create(Type forType) {

            if (applicators.TryGetValue(forType, out var applicator))
                return applicator;

            if (typeof(Control).IsAssignableFrom(forType))
                return new PropertyControlStyleApplicator<Control>();

            return new NullStyleApplicator();

        }

        // Private members

        private readonly IDictionary<Type, IStyleApplicator2> applicators = new Dictionary<Type, IStyleApplicator2>();

    }

}