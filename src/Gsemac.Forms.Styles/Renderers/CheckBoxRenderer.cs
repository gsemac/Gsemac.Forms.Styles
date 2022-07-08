﻿using Gsemac.Forms.Styles.StyleSheets.Extensions;
using Gsemac.Forms.Styles.StyleSheets.Properties;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System.Drawing;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public class CheckBoxRenderer :
        ControlRendererBase<CheckBox> {

        // Public members

        public override void PaintControl(CheckBox control, ControlPaintArgs e) {

            e.PaintBackground();
            e.PaintBorder();

            PaintCheck(control, e);

            IRuleset ruleset = e.StyleSheet.GetRuleset(control);

            Rectangle clientRect = control.ClientRectangle;
            Rectangle drawRect = new Rectangle(clientRect.X + CheckWidth + 3, clientRect.Y - 1, clientRect.Width, clientRect.Height);

            e.StyleRenderer.PaintText(e.Graphics, drawRect, ruleset, control.Text, control.Font, ControlUtilities.GetTextFormatFlags(control.TextAlign));

        }

        // Private members

        private const int CheckWidth = 13;

        private void PaintCheck(CheckBox control, ControlPaintArgs e) {

            //INode controlNode = new ControlNode(control);
            //UserNode checkNode = new UserNode(string.Empty, new[] { "Check" });

            //checkNode.SetParent(controlNode);
            //checkNode.SetStates(controlNode.States);

            //IRuleset parentRuleset = e.StyleSheet.GetRuleset(control);
            //IRuleset ruleset = e.StyleSheet.GetRuleset(checkNode, inherit: false);

            //if (!ruleset.Any())
            //    ruleset = CreateDefaultCheckRuleset();

            //ruleset.InheritPropertiesFrom(parentRuleset);

            //Rectangle clientRect = e.Control.ClientRectangle;
            //Rectangle checkRect = new Rectangle(clientRect.X, clientRect.Y + (int)(clientRect.Height / 2.0f - CheckWidth / 2.0f) - 1, CheckWidth, CheckWidth);

            //e.StyleRenderer.PaintBackground(e.Graphics, checkRect, ruleset);
            //e.StyleRenderer.PaintBorder(e.Graphics, checkRect, ruleset);

            //// Draw the checkmark.

            //if (control.Checked) {

            //    using (Pen pen = new Pen(ruleset.Color?.Value ?? SystemColors.ControlText)) {

            //        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            //        pen.Alignment = PenAlignment.Center;
            //        pen.Width = 2.0f;
            //        pen.StartCap = LineCap.Square;
            //        pen.EndCap = LineCap.Square;

            //        e.Graphics.DrawLine(pen, checkRect.X + 3, checkRect.Y + checkRect.Height / 2.0f, checkRect.X + checkRect.Width / 2.0f - 1, checkRect.Y + checkRect.Height - 5);
            //        e.Graphics.DrawLine(pen, checkRect.X + checkRect.Width / 2.0f - 1, checkRect.Y + checkRect.Height - 5, checkRect.X + checkRect.Width - 4, checkRect.Y + 3);

            //    }

            //}

        }

        private IRuleset CreateDefaultCheckRuleset() {

            IRuleset ruleset = new Ruleset {
                PropertyFactory.Default.Create(PropertyName.BackgroundColor, PropertyValue.Create("white")),
                PropertyFactory.Default.Create(PropertyName.BorderColor, PropertyValue.Create("#707070")),
                PropertyFactory.Default.Create(PropertyName.BorderWidth, PropertyValue.Create("1px")),
                PropertyFactory.Default.Create(PropertyName.Color, PropertyValue.Create("black")),
            };

            return ruleset;

        }

    }

}