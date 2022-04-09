using Gsemac.Forms.Styles.Properties;
using Gsemac.Forms.Styles.StyleSheets;
using System;

namespace Gsemac.Forms.Styles.Applicators2 {

    public abstract class StyleApplicatorBase :
        IStyleApplicator2 {

        // Public members

        public abstract void ApplyStyle(object obj, IRuleset style);

        public virtual void InitializeTarget(object obj) { }
        public virtual void DeinitializeTarget(object obj) { }

    }

    public abstract class StyleApplicatorBase<T> :
        StyleApplicatorBase,
        IStyleApplicator<T> {

        // Public members

        public abstract void ApplyStyle(T obj, IRuleset style);

        public virtual void InitializeTarget(T obj) { }
        public virtual void DeinitializeTarget(T obj) { }

        public override void InitializeTarget(object obj) {

            ValidateTarget(obj);

            InitializeTarget((T)obj);

        }
        public override void DeinitializeTarget(object obj) {

            ValidateTarget(obj);

            DeinitializeTarget((T)obj);

        }
        public override void ApplyStyle(object obj, IRuleset style) {

            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            if (style is null)
                throw new ArgumentNullException(nameof(style));

            ValidateTarget(obj);

            ApplyStyle((T)obj, style);

        }

        // Private members

        private void ValidateTarget(object obj) {

            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            if (!typeof(T).IsAssignableFrom(obj.GetType()))
                throw new ArgumentException(string.Format(ExceptionMessages.CannotApplyStyleToTypeWithType, obj.GetType()), nameof(obj));

        }

    }

}