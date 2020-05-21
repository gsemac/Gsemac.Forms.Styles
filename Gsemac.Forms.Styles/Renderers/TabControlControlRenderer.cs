using Gsemac.Forms.Styles.StyleSheets;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public class TabControlControlRenderer :
        ControlRendererBase<TabControl> {

        // Public members

        public override void PaintControl(TabControl control, ControlPaintArgs e) {

            // Paint the background.

            e.PaintBackground();
            e.PaintBorder();

            // Paint the tabs.

            PaintTabs(control, e);

        }

        // Private members

        private void PaintTabs(TabControl control, ControlPaintArgs e) {

            if (control.TabPages.Count > 0) {

                for (int i = 0; i < control.TabPages.Count; ++i) {

                    TabPage tabPage = control.TabPages[i];
                    Rectangle tabRect = control.GetTabRect(i);
                    UserNode tabNode = new UserNode(tabRect, control.PointToClient(Cursor.Position));

                    tabNode.AddClass("tab");

                    if (i == 0)
                        tabRect = new Rectangle(tabRect.X + 2, tabRect.Y, tabRect.Width - 2, tabRect.Height);

                    if (control.SelectedIndex == i) {

                        // Draw selected tab.

                        tabNode.AddState(NodeStates.Checked);

                        IRuleset tabRuleset = e.StyleSheet.GetRuleset(tabNode);

                        Rectangle drawRect = new Rectangle(tabRect.X - 2, tabRect.Y - 2, tabRect.Width + 2, tabRect.Height + 4);
                        Rectangle textRect = new Rectangle(tabRect.X, tabRect.Y - 2, tabRect.Width, tabRect.Height);

                        if (i == 0)
                            drawRect = new Rectangle(drawRect.X + 2, drawRect.Y, drawRect.Width - 2, drawRect.Height);

                        e.StyleRenderer.PaintBackground(e.Graphics, drawRect, tabRuleset);
                        e.StyleRenderer.PaintBorder(e.Graphics, drawRect, tabRuleset);
                        e.StyleRenderer.PaintText(e.Graphics, textRect, tabRuleset, tabPage.Text, control.Font, textFormatFlags: TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                    }
                    else {

                        // Draw unselected tab.

                        IRuleset tabRuleset = e.StyleSheet.GetRuleset(tabNode);

                        Rectangle drawRect = new Rectangle(tabRect.X, tabRect.Y, tabRect.Width, tabRect.Height + 2);

                        e.StyleRenderer.PaintBackground(e.Graphics, drawRect, tabRuleset);
                        e.StyleRenderer.PaintBorder(e.Graphics, drawRect, tabRuleset);
                        e.StyleRenderer.PaintText(e.Graphics, tabRect, tabRuleset, tabPage.Text, control.Font, textFormatFlags: TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                    }

                }

            }

        }

    }

}