using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers2 {

    public sealed class ControlRendererFactory :
        IStyleRendererFactory {

        // Public members

        public static ControlRendererFactory Default { get; } = new ControlRendererFactory();

        public ControlRendererFactory() {

            InitializeDefaultRenderers();

        }

        public IStyleRenderer Create(Type forType) {

            if (forType is null)
                throw new ArgumentNullException(nameof(forType));

            if (renderers.TryGetValue(forType, out var renderer))
                return renderer;

            switch (forType.FullName) {

                case "System.Windows.Forms.UpDownBase+UpDownButtons":
                    return new UpDownButtonsRenderer();

                default:
                    return new NullStyleRenderer();

            }

        }

        // Private members

        private readonly IDictionary<Type, IStyleRenderer> renderers = new Dictionary<Type, IStyleRenderer>();

        private void InitializeDefaultRenderers() {

            renderers.Add(typeof(Button), new ButtonRenderer());
            renderers.Add(typeof(CheckBox), new CheckBoxRenderer());
            renderers.Add(typeof(ComboBox), new ComboBoxRenderer());
            renderers.Add(typeof(GroupBox), new GroupBoxRenderer());
            renderers.Add(typeof(Label), new LabelRenderer());
            renderers.Add(typeof(ListBox), new ListBoxRenderer());
            renderers.Add(typeof(NumericUpDown), new NumericUpDownRenderer());
            renderers.Add(typeof(ProgressBar), new ProgressBarRenderer());
            renderers.Add(typeof(RadioButton), new RadioButtonRenderer());
            renderers.Add(typeof(RichTextBox), new RichTextBoxRenderer());
            renderers.Add(typeof(TabControl), new TabControlRenderer());
            renderers.Add(typeof(TextBox), new TextBoxRenderer());

        }

    }

}