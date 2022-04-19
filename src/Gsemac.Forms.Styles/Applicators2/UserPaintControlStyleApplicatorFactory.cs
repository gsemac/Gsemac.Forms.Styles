using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators2 {

    public class UserPaintControlStyleApplicatorFactory :
        INodeStyleApplicatorFactory {

        // Public members

        public UserPaintControlStyleApplicatorFactory() {

            InitializeApplicatorDictionary();

        }

        public IStyleApplicator Create(Type forType) {

            if (applicators.TryGetValue(forType, out var applicator))
                return applicator;

            return new NullNodeStyleApplicator();

        }

        // Private members

        private readonly IDictionary<Type, IStyleApplicator> applicators = new Dictionary<Type, IStyleApplicator>();

        private void InitializeApplicatorDictionary() {

            applicators.Add(typeof(Button), new UserPaintControlStyleApplicator<Button>());

        }

    }

}