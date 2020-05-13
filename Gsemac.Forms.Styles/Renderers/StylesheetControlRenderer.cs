using Gsemac.Forms.Styles.Extensions;
using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    [Flags]
    public enum StylesheetControlRendererOptions {
        None = 0,
        CacheRulesets = 1,
        Default = CacheRulesets
    }

    public sealed class StylesheetControlRenderer :
        StylesheetControlRendererBase {

        // Public members

        public StylesheetControlRenderer(IStyleSheet styleSheet, StylesheetControlRendererOptions options = StylesheetControlRendererOptions.Default) :

            base(styleSheet) {

            this.options = options;

        }

        public override IRuleset GetRuleset(Control control, bool inherit = true) {

            if (options.HasFlag(StylesheetControlRendererOptions.CacheRulesets))
                return GetRulesetFromCache(control, inherit);
            else
                return base.GetRuleset(control, inherit);

        }

        public override void RenderControl(Graphics graphics, Control control) {

            switch (control) {

                case Button button:

                    new ButtonControlRenderer(this).RenderControl(graphics, button);

                    break;

                case CheckBox checkBox:

                    new CheckBoxControlRenderer(this).RenderControl(graphics, checkBox);

                    break;

                case ComboBox comboBox:

                    new ComboBoxControlRenderer(this).RenderControl(graphics, comboBox);

                    break;

                case Label label:

                    new LabelControlRenderer(this).RenderControl(graphics, label);

                    break;

                case ListBox listBox:

                    new ListBoxControlRenderer(this).RenderControl(graphics, listBox);

                    break;

                case NumericUpDown numericUpDown:

                    new NumericUpDownControlRenderer(this).RenderControl(graphics, numericUpDown);

                    break;

                case RadioButton radioButton:

                    new RadioButtonControlRenderer(this).RenderControl(graphics, radioButton);

                    break;

                case TabControl tabControl:

                    new TabControlControlRenderer(this).RenderControl(graphics, tabControl);

                    break;

                case TextBox textBox:

                    new TextBoxControlRenderer(this).RenderControl(graphics, textBox);

                    break;

                default:

                    // These controls can't be checked for directly because they are internal types.

                    switch (control.GetType().FullName) {

                        case "System.Windows.Forms.UpDownBase+UpDownButtons":

                            new UpDownButtonsControlRenderer(this).RenderControl(graphics, control);

                            break;

                        default:

                            RenderGenericControl(graphics, control);

                            break;

                    }

                    break;

            }

        }

        // Private members

        private class CachedRulesets {

            public IDictionary<NodeStates, IRuleset> Cache { get; } = new Dictionary<NodeStates, IRuleset>();

        }

        private readonly StylesheetControlRendererOptions options = StylesheetControlRendererOptions.Default;
        private readonly Dictionary<Control, CachedRulesets> rulesetCache = new Dictionary<Control, CachedRulesets>();

        private void RenderGenericControl(Graphics graphics, Control control) {

            this.PaintBackground(graphics, control);

        }

        private IRuleset GetRulesetFromCache(Control control, bool inherit) {

            INode node = new ControlNode(control);

            if (!(rulesetCache.TryGetValue(control, out CachedRulesets cachedRulesets) && cachedRulesets.Cache.TryGetValue(node.States, out IRuleset ruleset)))
                ruleset = GetRuleset(node, inherit);

            if (cachedRulesets is null) {

                cachedRulesets = new CachedRulesets();

                rulesetCache[control] = cachedRulesets;

            }

            if (ruleset != null)
                cachedRulesets.Cache[node.States] = ruleset;

            return ruleset;

        }

    }

}