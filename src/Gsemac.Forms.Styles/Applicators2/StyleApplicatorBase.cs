using Gsemac.Forms.Styles.Properties;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;

namespace Gsemac.Forms.Styles.Applicators2 {

    public abstract class StyleApplicatorBase :
        IStyleApplicator {

        // Public members

        public abstract void ApplyStyle(object obj, IRuleset ruleset);

        public virtual void InitializeStyle(object obj) { }
        public virtual void DeinitializeStyle(object obj) { }

    }

    public abstract class StyleApplicatorBase<T> :
        StyleApplicatorBase,
        IStyleApplicator<T> {

        // Public members

        public abstract void ApplyStyle(T obj, IRuleset ruleset);

        public virtual void InitializeStyle(T obj) { }
        public virtual void DeinitializeStyle(T obj) { }

        public override void InitializeStyle(object obj) {

            ValidateStyleTarget(obj);

            InitializeStyle((T)obj);

        }
        public override void DeinitializeStyle(object obj) {

            ValidateStyleTarget(obj);

            DeinitializeStyle((T)obj);

        }
        public override void ApplyStyle(object obj, IRuleset ruleset) {

            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            if (ruleset is null)
                throw new ArgumentNullException(nameof(ruleset));

            ValidateStyleTarget(obj);

            ApplyStyle((T)obj, ruleset);

        }

        // Private members

        private void ValidateStyleTarget(object obj) {

            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            if (!typeof(T).IsAssignableFrom(obj.GetType()))
                throw new ArgumentException(string.Format(ExceptionMessages.CannotApplyStyleToType, obj.GetType()), nameof(obj));

        }

    }

}