using Gsemac.Drawing;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers2 {

    public class ToolStripRenderer :
        System.Windows.Forms.ToolStripRenderer {

        // Public members

        public IRuleset Ruleset { get; set; }

        public ToolStripRenderer() { }

        // Protected members

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e) {

            if (Ruleset is null) {

                base.OnRenderToolStripBackground(e);

            }
            else {

                IRenderContext context = new RenderContext(e.Graphics, e.ToolStrip.ClientRectangle, Ruleset);

                context.DrawBackground();
                context.DrawBorder();

            }

        }
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e) {

            if (Ruleset is null) {

                base.OnRenderMenuItemBackground(e);

            }
            else {

                Rectangle backgroundRect = new Rectangle(2, 0, e.Item.Width - 3, e.Item.Height);
                IRenderContext context = new RenderContext(e.Graphics, backgroundRect, Ruleset);

                context.DrawBackground();
                context.DrawBorder();

                if (e.Item.Selected || e.Item.Pressed) {

                    Color highlightColor = ColorUtilities.Tint(Ruleset.BackgroundColor, 0.5f);

                    using (Brush brush = new SolidBrush(Color.FromArgb(50, highlightColor)))
                        e.Graphics.FillRectangle(brush, backgroundRect);

                }

            }

        }
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e) {

            if (Ruleset is null) {

                base.OnRenderItemText(e);

            }
            else {

                IRenderContext context = new RenderContext(e.Graphics, e.TextRectangle, Ruleset);

                context.DrawText(e.Text, e.TextFont, e.TextFormat);

            }

        }
        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e) {

            // This method is called when rendering the border around the ToolStripDropDown.

            if (Ruleset is null) {

                base.OnRenderImageMargin(e);

            }
            else {

                IRenderContext context = new RenderContext(e.Graphics, e.ToolStrip.ClientRectangle, Ruleset);

                context.DrawBackground();
                context.DrawBorder();

            }

        }
        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e) {

            if (Ruleset is null) {

                base.OnRenderSeparator(e);

            }
            else {

                Rectangle separatorRect = e.Vertical ?
                    new Rectangle(e.Item.Width / 2, 4, 1, e.Item.Height - 8) :
                    new Rectangle(0, e.Item.Height / 2, e.Item.Width, 1);

                using (Brush brush = new SolidBrush(Ruleset.AccentColor))
                    e.Graphics.FillRectangle(brush, separatorRect);

            }

        }

    }

}