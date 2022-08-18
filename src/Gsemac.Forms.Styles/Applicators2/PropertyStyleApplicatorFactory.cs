using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators2 {

    public class PropertyStyleApplicatorFactory :
        IStyleApplicatorFactory {

        // Public members

        public PropertyStyleApplicatorFactory() {

            InitializeApplicatorDictionary();

        }

        public IStyleApplicator Create(Type forType) {

            if (applicators.TryGetValue(forType, out var applicator))
                return applicator;

            if (typeof(Control).IsAssignableFrom(forType))
                return new ControlPropertyStyleApplicator<Control>();

            return new NullStyleApplicator();

        }

        // Private members

        private readonly IDictionary<Type, IStyleApplicator> applicators = new Dictionary<Type, IStyleApplicator>();

        private void InitializeApplicatorDictionary() {

            applicators.Add(typeof(Button), new ButtonPropertyStyleApplicator());
            applicators.Add(typeof(ListBox), new ListBoxPropertyStyleApplicator());
            applicators.Add(typeof(NumericUpDown), new NumericUpDownPropertyStyleApplicator());
            applicators.Add(typeof(TextBox), new TextBoxPropertyStyleApplicator());

        }

    }

}