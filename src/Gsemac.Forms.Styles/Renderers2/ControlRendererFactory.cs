using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers2 {

    public class ControlRendererFactory :
        IStyleRendererFactory {

        // Public members

        public static ControlRendererFactory Default => new ControlRendererFactory();

        public ControlRendererFactory() {

            InitializeApplicatorDictionary();

        }

        public IStyleRenderer Create(Type forType) {

            if (renderers.TryGetValue(forType, out var renderer))
                return renderer;

            return new NullStyleRenderer();

        }

        // Private members

        private readonly IDictionary<Type, IStyleRenderer> renderers = new Dictionary<Type, IStyleRenderer>();

        private void InitializeApplicatorDictionary() {

            renderers.Add(typeof(Button), new ButtonRenderer());

        }

    }

}