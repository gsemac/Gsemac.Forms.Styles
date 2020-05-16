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

            GetControlRenderer(control).RenderControl(graphics, control);

        }

        // Private members

        private readonly StylesheetControlRendererOptions options = StylesheetControlRendererOptions.Default;
        private readonly Dictionary<Control, IDictionary<NodeStates, IRuleset>> rulesetCache = new Dictionary<Control, IDictionary<NodeStates, IRuleset>>();

        private IControlRenderer GetControlRenderer(Control control) {

            switch (control) {

                case Button _:
                    return new ButtonControlRenderer(this);

                case CheckBox _:
                    return new CheckBoxControlRenderer(this);

                case ComboBox _:
                    return new ComboBoxControlRenderer(this);

                case Label _:
                    return new LabelControlRenderer(this);

                case ListBox _:
                    return new ListBoxControlRenderer(this);

                case NumericUpDown _:
                    return new NumericUpDownControlRenderer(this);

                case RadioButton _:
                    return new RadioButtonControlRenderer(this);

                case TabControl _:
                    return new TabControlControlRenderer(this);

                case TextBox _:
                    return new TextBoxControlRenderer(this);

                case GroupBox _:
                    return new GroupBoxControlRenderer(this);

                default:

                    // Some controls can't be checked for directly because they are internal types.

                    switch (control.GetType().FullName) {

                        case "System.Windows.Forms.UpDownBase+UpDownButtons":
                            return new UpDownButtonsControlRenderer(this);

                        default:
                            return new GenericControlRenderer(this);

                    }

            }

        }
        private IRuleset GetRulesetFromCache(Control control, bool inherit) {

            INode node = new ControlNode(control);

            if (!(rulesetCache.TryGetValue(control, out IDictionary<NodeStates, IRuleset> cachedRulesets) && cachedRulesets.TryGetValue(node.States, out IRuleset ruleset)))
                ruleset = GetRuleset(node, inherit);

            if (cachedRulesets is null) {

                cachedRulesets = new Dictionary<NodeStates, IRuleset>();

                rulesetCache[control] = cachedRulesets;

            }

            if (ruleset != null)
                cachedRulesets[node.States] = ruleset;

            return ruleset;

        }

    }

}