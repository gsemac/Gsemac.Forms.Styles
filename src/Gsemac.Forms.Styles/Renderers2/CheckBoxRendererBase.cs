using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers2 {

    public abstract class CheckBoxRendererBase<TControl> :
        StyleRendererBase<TControl> where TControl : ButtonBase {

        // Public members

        public override void Render(TControl checkBox, IRenderContext context) {

            if (checkBox is null)
                throw new ArgumentNullException(nameof(checkBox));

            if (context is null)
                throw new ArgumentNullException(nameof(context));

            context.Clear();

            context.DrawBackground();

            DrawCheck(checkBox, context);
            DrawText(checkBox, context);

            if (ControlUtilities2.FocusCuesShown(checkBox))
                ControlRenderUtilities.DrawFocusRectangle(context.Graphics, GetFocusRect(checkBox, context), context.Style);

            context.DrawBorder();

        }

        // Protected members

        protected int CheckBoxWidth { get; } = 12;

        protected abstract void DrawCheck(TControl checkBox, IRenderContext context);

        // Private members

        private void DrawText(TControl checkBox, IRenderContext context) {

            if (checkBox is null)
                throw new ArgumentNullException(nameof(checkBox));

            if (context is null)
                throw new ArgumentNullException(nameof(context));

            Rectangle textRect = GetTextRect(context);

            context.DrawText(textRect, checkBox.Text, checkBox.Font, ControlUtilities.GetTextFormatFlags(checkBox.TextAlign));

        }

        private Rectangle GetTextRect(IRenderContext context) {

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
        private Rectangle GetFocusRect(TControl checkBox, IRenderContext context) {

            int focusXOffset = -1;
            int focusYOffset = 2;

            Rectangle textRect = GetTextRect(context);

            Size textSize = TextRenderer.MeasureText(checkBox.Text, checkBox.Font);

            return new Rectangle(
                textRect.X + focusXOffset,
                textRect.Y + focusYOffset,
                textSize.Width,
                textSize.Height
            );

        }

    }

}