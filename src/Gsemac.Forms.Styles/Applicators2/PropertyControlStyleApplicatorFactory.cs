using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators2 {

    public class PropertyControlStyleApplicatorFactory :
        INodeStyleApplicatorFactory {

        // Public members

        public PropertyControlStyleApplicatorFactory() {

            InitializeApplicatorDictionary();

        }

        public IStyleApplicator Create(Type forType) {

            if (applicators.TryGetValue(forType, out var applicator))
                return applicator;

            if (typeof(Control).IsAssignableFrom(forType))
                return new PropertyControlStyleApplicator<Control>();

            return new NullNodeStyleApplicator();

        }

        // Private members

        private readonly IDictionary<Type, IStyleApplicator> applicators = new Dictionary<Type, IStyleApplicator>();

        private void InitializeApplicatorDictionary() {

            applicators.Add(typeof(Button), new ButtonPropertyControlStyleApplicator());

        }

    }

}