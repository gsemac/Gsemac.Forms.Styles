using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers2 {

    internal abstract class CheckBoxRendererBase<TControl> :
        StyleRendererBase<TControl> where TControl : ButtonBase {

        // Public members

        public override void Render(TControl control, IRenderContext context) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            if (context is null)
                throw new ArgumentNullException(nameof(context));

            context.Clear();

            context.DrawBackground();

            DrawCheck(control, context);
            DrawText(control, context);

            if (ControlUtilities2.FocusCuesShown(control))
                ControlRenderUtilities.DrawFocusRectangle(context.Graphics, GetFocusRectangle(control, context), context.Style);

            context.DrawBorder();

        }

        // Protected members

        protected int CheckBoxWidth { get; } = 12;

        protected abstract void DrawCheck(TControl control, IRenderContext context);

        protected virtual Rectangle GetTextRectangle(TControl control, IRenderContext context) {

            int textXOffset = 4;
            int textYOffset = -1;

            Rectangle textRect = new Rectangle(
                context.ClientRectangle.X + CheckBoxWidth + textXOffset,
                context.ClientRectangle.Y + textYOffset,
                context.ClientRectangle.Width,
                context.ClientRectangle.Height
            );

            return textRect;

        }
        protected virtual Rectangle GetFocusRectangle(TControl control, IRenderContext context) {

            int focusXOffset = -1;
            int focusYOffset = 2;

            Rectangle textRect = GetTextRectangle(control, context);

            Size textSize = TextRenderer.MeasureText(control.Text, control.Font);

            return new Rectangle(
                textRect.X + focusXOffset,
                textRect.Y + focusYOffset,
                textSize.Width,
                textSize.Height
            );

        }

        // Private members

        private void DrawText(TControl control, IRenderContext context) {

            if (control is null)
                throw new ArgumentNullException(nameof(control));

            if (context is null)
                throw new ArgumentNullException(nameof(context));

            Rectangle textRect = GetTextRectangle(control, context);

            context.DrawText(textRect, control.Text, control.Font, ControlUtilities.GetTextFormatFlags(control.TextAlign));

        }

    }

}