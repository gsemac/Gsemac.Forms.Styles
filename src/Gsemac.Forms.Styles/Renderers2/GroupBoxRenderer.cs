using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers2 {

    internal class GroupBoxRenderer :
        StyleRendererBase<GroupBox> {

        // Public members

        public override void Render(GroupBox groupBox, IRenderContext context) {

            if (groupBox is null)
                throw new ArgumentNullException(nameof(groupBox));

            if (context is null)
                throw new ArgumentNullException(nameof(context));

            SizeF textSize = context.Graphics.MeasureString(groupBox.Text, groupBox.Font);

            Rectangle clientRect = groupBox.ClientRectangle;
            Rectangle backgroundRect = new Rectangle(clientRect.X, clientRect.Y + (int)textSize.Height / 2, clientRect.Width, clientRect.Height - (int)textSize.Height / 2);
            Rectangle textRect = new Rectangle(clientRect.X + 6, clientRect.Y, (int)textSize.Width, (int)textSize.Height);
            Rectangle textBackgroundRect = new Rectangle(textRect.X, backgroundRect.Y, textRect.Width, textRect.Height - (int)textSize.Height / 2);

            context.Clear();

            context.DrawBackground(backgroundRect);
            context.DrawBorder(backgroundRect);

            context.DrawBackground(textBackgroundRect);
            //context.DrawBorder(textBackgroundRect);

            context.DrawText(textRect, groupBox.Text, groupBox.Font, TextFormatFlags.Top);

        }

    }

}