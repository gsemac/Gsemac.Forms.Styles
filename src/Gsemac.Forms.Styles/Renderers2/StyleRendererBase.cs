using Gsemac.Forms.Styles.Properties;
using System;

namespace Gsemac.Forms.Styles.Renderers2 {

    public abstract class StyleRendererBase :
        IStyleRenderer {

        // Public members

        public abstract void Render(object obj, IRenderContext context);

    }

    public abstract class StyleRendererBase<T> :
        StyleRendererBase,
        IStyleRenderer<T> {

        // Public members

        public override void Render(object obj, IRenderContext context) {

            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            if (context is null)
                throw new ArgumentNullException(nameof(context));

            ValidateTarget(obj);

            Render((T)obj, context);

        }

        public abstract void Render(T obj, IRenderContext context);

        // Private members

        private void ValidateTarget(object obj) {

            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            if (!typeof(T).IsAssignableFrom(obj.GetType()))
                throw new ArgumentException(string.Format(ExceptionMessages.CannotRenderType, obj.GetType()), nameof(obj));

        }

    }

}