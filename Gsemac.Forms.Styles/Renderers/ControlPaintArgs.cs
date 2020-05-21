﻿using Gsemac.Forms.Styles.Extensions;
using Gsemac.Forms.Styles.StyleSheets;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public class ControlPaintArgs {

        public const TextFormatFlags DefaultTextFormatFlags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter;

        public Control Control { get; }
        public Graphics Graphics { get; }
        public IStyleSheet StyleSheet { get; }
        public IStyleRenderer StyleRenderer { get; }

        public ControlPaintArgs(Control control, Graphics graphics, IStyleSheet styleSheet, IStyleRenderer styleRenderer) {

            this.Control = control;
            this.Graphics = graphics;
            this.StyleSheet = styleSheet;
            this.StyleRenderer = styleRenderer;

        }

        public void Clear() {

            ColorProperty parentBackgroundColor = null;

            if (Control.Parent != null)
                parentBackgroundColor = StyleSheet.GetRuleset(Control.Parent).BackgroundColor;

            Graphics.Clear(parentBackgroundColor?.Value ?? Control.Parent?.BackColor ?? Color.Transparent);

        }

        public void PaintBackground() {

            Clear();

            PaintBackground(Control.ClientRectangle);

        }
        public void PaintBackground(Rectangle drawRect) {

            StyleRenderer.PaintBackground(Graphics, drawRect, StyleSheet.GetRuleset(Control));

        }
        public void PaintText(TextFormatFlags textFormatFlags = DefaultTextFormatFlags) {

            PaintText(Control.ClientRectangle, textFormatFlags);

        }
        public void PaintText(Rectangle textRect, TextFormatFlags textFormatFlags = DefaultTextFormatFlags) {

            StyleRenderer.PaintText(Graphics, textRect, StyleSheet.GetRuleset(Control), Control.Text, Control.Font, textFormatFlags);

        }
        public void PaintBorder() {

            StyleRenderer.PaintBorder(Graphics, Control.ClientRectangle, StyleSheet.GetRuleset(Control));

        }

        public void PaintControl() {

            PaintBackground();
            PaintText();
            PaintBorder();

        }

        public void ClipToBorder() {

            IRuleset ruleset = StyleSheet.GetRuleset(Control);

            if (ruleset.Any(p => p.IsBorderRadiusProperty())) {

                Graphics.SetClip(GraphicsExtensions.CreateRoundedRectangle(Control.ClientRectangle,
                    (int)(ruleset.BorderTopLeftRadius?.Value ?? 0),
                    (int)(ruleset.BorderTopRightRadius?.Value ?? 0),
                    (int)(ruleset.BorderBottomLeftRadius?.Value ?? 0),
                    (int)(ruleset.BorderBottomRightRadius?.Value ?? 0)));

            }
            else {

                Graphics.SetClip(Control.ClientRectangle);

            }

        }

    }

}