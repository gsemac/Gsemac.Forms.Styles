using Gsemac.Drawing;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers2 {

    internal class TabControlRenderer :
        StyleRendererBase<TabControl> {

        // Public members

        public override void Render(TabControl tabControl, IRenderContext context) {

            if (tabControl is null)
                throw new ArgumentNullException(nameof(tabControl));

            if (context is null)
                throw new ArgumentNullException(nameof(context));

            // Paint the background.

            int tabAreaHeight = (tabControl.TabPages.Count > 0 ?
                tabControl.GetTabRect(0).Height :
                20) + 3;

            Rectangle tabControlBodyRect = new Rectangle(
                tabControl.ClientRectangle.X,
                tabControl.ClientRectangle.Y + tabAreaHeight,
                tabControl.ClientRectangle.Width,
                tabControl.ClientRectangle.Height - tabAreaHeight
            );

            context.Clear();

            context.DrawBackground();
            context.DrawBorder(tabControlBodyRect);

            // Paint the tabs.

            PaintTabs(tabControl, context);

        }

        // Private members

        private static void PaintTabs(TabControl tabControl, IRenderContext context) {

            // Draw all the tabs, but save selected tab for last so it can be drawn on top of other tabs.

            if (tabControl.TabPages.Count > 0) {

                for (int i = 0; i < tabControl.TabPages.Count; ++i) {

                    if (tabControl.SelectedIndex == i)
                        continue;

                    DrawTab(tabControl, i, context);

                }

                // Draw the selected tab.

                if (tabControl.SelectedIndex >= 0)
                    DrawTab(tabControl, tabControl.SelectedIndex, context);

            }

        }
        private static void DrawTab(TabControl tabControl, int tabIndex, IRenderContext context) {

            TabPage tabPage = tabControl.TabPages[tabIndex];
            Rectangle tabRect = tabControl.GetTabRect(tabIndex);

            if (tabControl.SelectedIndex == tabIndex) {

                // Draw selected tab.

                Rectangle drawRect = new Rectangle(tabRect.X - 2, tabRect.Y - 1, tabRect.Width + 4, tabRect.Height + 3);
                Rectangle textRect = new Rectangle(tabRect.X, tabRect.Y - 2, tabRect.Width, tabRect.Height);
                Rectangle borderRect = new Rectangle(drawRect.X, drawRect.Y, drawRect.Width, drawRect.Height + (int)context.Style.BorderBottomWidth.Value);

                context.Graphics.SetClip(drawRect);

                context.DrawBackground(drawRect);
                context.DrawBorder(borderRect);
                context.DrawText(textRect, tabPage.Text, tabControl.Font, textFormatFlags: TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

            }
            else {

                // Draw unselected tab.

                Rectangle drawRect = new Rectangle(tabRect.X, tabRect.Y + 1, tabRect.Width, tabRect.Height + 1);

                context.Graphics.SetClip(drawRect);

                context.DrawBackground(drawRect);

                // Highlight the tab if the mouse is hovering over it.

                bool mouseOn = tabRect.Contains(tabControl.PointToClient(Cursor.Position));

                if (mouseOn) {

                    Color hoverColor = ColorUtilities.Tint(context.Style.BackgroundColor, 0.5f);

                    using (Brush brush = new SolidBrush(Color.FromArgb(50, hoverColor)))
                        context.Graphics.FillRectangle(brush, drawRect);

                }

                context.DrawBorder(drawRect);
                context.DrawText(tabRect, tabPage.Text, tabControl.Font, textFormatFlags: TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

            }

        }

    }

}