using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers2 {

    public class ControlRendererFactory :
        IStyleRendererFactory {

        // Public members

        public static ControlRendererFactory Default { get; } = new ControlRendererFactory();

        public ControlRendererFactory() {

            InitializeDefaultRenderers();

        }

        public IStyleRenderer Create(Type forType) {

            if (renderers.TryGetValue(forType, out var renderer))
                return renderer;

            return new NullStyleRenderer();

        }

        // Private members

        private readonly IDictionary<Type, IStyleRenderer> renderers = new Dictionary<Type, IStyleRenderer>();

        private void InitializeDefaultRenderers() {

            renderers.Add(typeof(Button), new ButtonRenderer());
            renderers.Add(typeof(CheckBox), new CheckBoxRenderer());
            renderers.Add(typeof(GroupBox), new GroupBoxRenderer());
            renderers.Add(typeof(Label), new LabelRenderer());
            renderers.Add(typeof(RadioButton), new RadioButtonRenderer());
            renderers.Add(typeof(TextBox), new TextBoxRenderer());

        }

    }

}