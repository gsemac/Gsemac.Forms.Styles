﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators2 {

    public class UserPaintStyleApplicatorFactory :
        IStyleApplicatorFactory {

        // Public members

        public UserPaintStyleApplicatorFactory() {

            InitializeApplicatorDictionary();

        }

        public IStyleApplicator Create(Type forType) {

            if (applicators.TryGetValue(forType, out var applicator))
                return applicator;

            return fallbackApplicatorFactory.Create(forType);

        }

        // Private members

        private readonly IDictionary<Type, IStyleApplicator> applicators = new Dictionary<Type, IStyleApplicator>();
        private readonly IStyleApplicatorFactory fallbackApplicatorFactory = new PropertyStyleApplicatorFactory();

        private void InitializeApplicatorDictionary() {

            applicators.Add(typeof(Button), new UserPaintStyleApplicator<Button>());

        }

    }

}