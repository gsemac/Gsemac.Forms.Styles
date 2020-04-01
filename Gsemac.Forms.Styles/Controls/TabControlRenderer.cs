﻿using Gsemac.Forms.Styles.Controls;
using Gsemac.Forms.Styles.StyleSheets;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Controls {

    public class TabControlRenderer :
        ControlRendererBase {

        // Public members

        public TabControlRenderer(IStyleSheet styleSheet) :
            base(styleSheet) {
        }

        public void RenderControl(Graphics graphics, TabControl control) {

            Rectangle clientRect = control.ClientRectangle;
            Rectangle drawRect = new Rectangle(clientRect.X, clientRect.Y + 2, clientRect.Width, clientRect.Height - 2);

            if (control.Parent != null)
                PaintBackground(graphics, clientRect, GetRuleset(control.Parent));

            PaintBackground(graphics, drawRect, GetRuleset(control));

            // Paint the tabs background.

            PaintTabsBackground(graphics, control);

            // Paint the tabs.

            PaintTabs(graphics, control);

        }

        // Private members

        private void PaintTabsBackground(Graphics graphics, TabControl control) {

            IRuleset tabsRules = GetRuleset(control, new Node("TabHeader"));

            if (control.TabPages.Count > 0) {

                Rectangle tabRect = control.GetTabRect(0);
                Rectangle drawRect = new Rectangle(0, 2, control.Width + tabRect.X, tabRect.Height + tabRect.Y - 2);

                PaintBackground(graphics, drawRect, tabsRules);

            }

        }
        private void PaintTabs(Graphics graphics, TabControl control) {

            if (control.TabPages.Count > 0) {

                IRuleset baseRules = GetRuleset(control);

                IRuleset tabRules = GetRuleset(baseRules, new Node("Tab"));
                IRuleset tabCheckedRules = GetRuleset(baseRules, new Node("Tab", NodeStates.Checked));
                IRuleset tabHoverRules = GetRuleset(baseRules, new Node("Tab", NodeStates.Hover));

                ColorProperty tabBackgroundColor = tabRules.GetProperty(PropertyType.BackgroundColor) as ColorProperty;
                ColorProperty tabColor = tabRules.GetProperty(PropertyType.Color) as ColorProperty;
                ColorProperty tabCheckedBackgroundColor = tabCheckedRules.GetProperty(PropertyType.BackgroundColor) as ColorProperty;
                ColorProperty tabCheckedColor = tabCheckedRules.GetProperty(PropertyType.Color) as ColorProperty ?? tabColor;
                ColorProperty tabHoverBackgroundColor = tabHoverRules.GetProperty(PropertyType.BackgroundColor) as ColorProperty;

                Point mousePos = control.PointToClient(Cursor.Position);
                Rectangle mouseRect = new Rectangle(mousePos.X, mousePos.Y, 1, 1);

                for (int i = 0; i < control.TabPages.Count; ++i) {

                    TabPage tabPage = control.TabPages[i];
                    Rectangle tabRect = control.GetTabRect(i);
                    Rectangle drawRect = new Rectangle(tabRect.X, tabRect.Y, tabRect.Width, tabRect.Height + 2);

                    if (i == 0)
                        drawRect = new Rectangle(drawRect.X + 2, drawRect.Y, drawRect.Width - 2, drawRect.Height);

                    if (control.SelectedIndex == i) {

                        // Draw selected tab.

                        drawRect = new Rectangle(drawRect.X, drawRect.Y - 2, drawRect.Width, drawRect.Height + 2);

                        if (mouseRect.IntersectsWith(tabRect) && tabHoverBackgroundColor != null) {

                            using (Brush brush = new SolidBrush(tabHoverBackgroundColor.Value))
                                graphics.FillRectangle(brush, drawRect);

                        }
                        else {

                            if (tabCheckedBackgroundColor != null)
                                using (Brush brush = new SolidBrush(tabCheckedBackgroundColor.Value))
                                    graphics.FillRectangle(brush, drawRect);

                        }

                        TextRenderer.DrawText(graphics, tabPage.Text, control.Font, drawRect, tabCheckedColor?.Value ?? SystemColors.ControlText, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                    }
                    else {

                        // Draw non-selected tab.

                        if (mouseRect.IntersectsWith(tabRect) && tabHoverBackgroundColor != null) {

                            using (Brush brush = new SolidBrush(tabHoverBackgroundColor.Value))
                                graphics.FillRectangle(brush, drawRect);

                        }
                        else {

                            if (tabBackgroundColor != null)
                                using (Brush brush = new SolidBrush(tabBackgroundColor.Value))
                                    graphics.FillRectangle(brush, drawRect);

                        }

                        TextRenderer.DrawText(graphics, tabPage.Text, control.Font, drawRect, tabColor?.Value ?? SystemColors.ControlText, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                    }

                }

            }

        }

    }

}