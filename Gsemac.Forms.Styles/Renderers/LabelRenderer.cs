﻿using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {
    public class LabelRenderer :
        ControlRendererBase<Label> {

        // Public members

        public override void PaintControl(Label control, ControlPaintArgs e) {

            e.PaintControl();

        }

    }

}