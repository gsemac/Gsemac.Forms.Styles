using Gsemac.Forms.Styles.Properties;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;

namespace Gsemac.Forms.Styles.Applicators2 {

    public abstract class StyleApplicatorBase :
        IStyleApplicator {

        // Public members

        public abstract void ApplyStyle(object obj, IRuleset ruleset);

        public virtual void InitializeObject(object obj) { }
        public virtual void DeinitializeObject(object obj) { }

    }

    public abstract class StyleApplicatorBase<T> :
        StyleApplicatorBase,
        IStyleApplicator<T> {

        // Public members

        public abstract void ApplyStyle(T obj, IRuleset ruleset);

        public virtual void InitializeObject(T obj) { }
        public virtual void DeinitializeObject(T obj) { }

        public override void InitializeObject(object obj) {

            ValidateTarget(obj);

            InitializeObject((T)obj);

        }
        public override void DeinitializeObject(object obj) {

            ValidateTarget(obj);

            DeinitializeObject((T)obj);

        }
        public override void ApplyStyle(object obj, IRuleset ruleset) {

            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            if (ruleset is null)
                throw new ArgumentNullException(nameof(ruleset));

            ValidateTarget(obj);

            ApplyStyle((T)obj, ruleset);

        }

        // Private members

        private void ValidateTarget(object obj) {

            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            if (!typeof(T).IsAssignableFrom(obj.GetType()))
                throw new ArgumentException(string.Format(ExceptionMessages.CannotApplyStyleToType, obj.GetType()), nameof(obj));

        }

    }

}