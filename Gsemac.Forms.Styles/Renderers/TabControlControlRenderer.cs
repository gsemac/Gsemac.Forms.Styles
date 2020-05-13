using Gsemac.Forms.Styles.Extensions;
using Gsemac.Forms.Styles.StyleSheets;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public class TabControlControlRenderer :
        ControlRendererBase<TabControl> {

        // Public members

        public TabControlControlRenderer(IStyleSheetControlRenderer baseRenderer) {

            this.baseRenderer = baseRenderer;

        }

        public override void RenderControl(Graphics graphics, TabControl control) {

            // Paint the background.

            IRuleset ruleset = baseRenderer.GetRuleset(control);

            baseRenderer.PaintBackground(graphics, control, ruleset);

            // Paint the tabs background.

            PaintTabsBackground(graphics, control);

            // Paint the tabs.

            PaintTabs(graphics, control, ruleset);

        }

        // Private members

        private readonly IStyleSheetControlRenderer baseRenderer;

        private void PaintTabsBackground(Graphics graphics, TabControl control) {

            //IRuleset tabsRules = GetRuleset(new Node(string.Empty, "TabHeader"));

            //if (control.TabPages.Count > 0) {

            //    Rectangle tabRect = control.GetTabRect(0);
            //    Rectangle drawRect = new Rectangle(0, 2, control.Width + tabRect.X, tabRect.Height + tabRect.Y - 2);

            //    PaintBackground(graphics, drawRect, tabsRules);

            //}

        }
        private void PaintTabs(Graphics graphics, TabControl control, IRuleset baseRuleset) {

            if (control.TabPages.Count > 0) {

                for (int i = 0; i < control.TabPages.Count; ++i) {

                    TabPage tabPage = control.TabPages[i];
                    Rectangle tabRect = control.GetTabRect(i);
                    UserNode tabNode = new UserNode(control, tabRect);

                    tabNode.AddClass("tab");

                    if (control.SelectedIndex == i) {

                        // Draw selected tab.

                        tabNode.AddState(NodeStates.Checked);

                        IRuleset tabRuleset = baseRenderer.GetRuleset(tabNode);

                        Rectangle drawRect = new Rectangle(tabRect.X - 2, tabRect.Y - 2, tabRect.Width + 2, tabRect.Height + 4);
                        Rectangle textRect = new Rectangle(tabRect.X, tabRect.Y - 2, tabRect.Width, tabRect.Height);

                        baseRenderer.PaintBackground(graphics, drawRect, tabRuleset);
                        baseRenderer.PaintForeground(graphics, tabPage.Text, control.Font, textRect, tabRuleset, textFormatFlags: TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                    }
                    else {

                        // Draw unselected tab.

                        IRuleset tabRuleset = baseRenderer.GetRuleset(tabNode);

                        Rectangle drawRect = new Rectangle(tabRect.X, tabRect.Y, tabRect.Width, tabRect.Height + 2);

                        baseRenderer.PaintBackground(graphics, drawRect, tabRuleset);
                        baseRenderer.PaintForeground(graphics, tabPage.Text, control.Font, tabRect, tabRuleset, textFormatFlags: TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                    }

                }

            }

        }

    }

}