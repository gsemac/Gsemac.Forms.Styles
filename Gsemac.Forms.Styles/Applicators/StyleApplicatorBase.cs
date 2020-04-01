using Gsemac.Forms.Styles.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators {

    public abstract class StyleApplicatorBase :
        IStyleApplicator {

        // Public members

        public void ApplyStyles(Control control, ControlStyleOptions options = ControlStyleOptions.Default) {

            if (!options.HasFlag(ControlStyleOptions.RulesRequired) || HasStyles(control)) {

                AddControlInfo(control);

                OnApplyStyles(control);

            }
            else {

                OnClearStyles(control);

                RemoveControlInfo(control);

            }

            if (options.HasFlag(ControlStyleOptions.Recursive) && control.HasChildren) {

                foreach (Control childControl in control.Controls)
                    ApplyStyles(childControl, options);

            }

            control.Invalidate();

        }
        public void ClearStyles(Control control, ControlStyleOptions options = ControlStyleOptions.Default) {

            OnClearStyles(control);

            RemoveControlInfo(control);

            if (options.HasFlag(ControlStyleOptions.Recursive) && control.HasChildren) {

                foreach (Control childControl in control.Controls)
                    ClearStyles(childControl, options);

            }

            control.Invalidate();

        }

        // Protected members

        protected delegate void ResetControlHandler(Control control);

        protected class ControlInfo {

            public event ResetControlHandler ResetControl;

            public ControlStyles Styles { get; set; }
            public DrawMode DrawMode { get; set; }
            public BorderStyle BorderStyle { get; set; }

            public void DoResetControl(Control control) {

                ResetControl?.Invoke(control);

            }

        }

        protected abstract bool HasStyles(Control control);
        protected abstract void OnApplyStyles(Control control);
        protected virtual void OnClearStyles(Control control) {
        }

        protected static ControlInfo GetControlInfo(Control control) {

            if (controlInfo.TryGetValue(control, out ControlInfo info))
                return info;

            return null;

        }

        // Private members

        private static readonly Dictionary<Control, ControlInfo> controlInfo = new Dictionary<Control, ControlInfo>();

        private void AddControlInfo(Control control) {

            RemoveControlInfo(control);

            ControlInfo info = new ControlInfo();

            // Store the control's initial styles so they can be restored in the future.

            if (ControlUtilities.GetStyle(control, ControlStyles.AllPaintingInWmPaint))
                info.Styles |= ControlStyles.AllPaintingInWmPaint;

            if (ControlUtilities.GetStyle(control, ControlStyles.UserPaint))
                info.Styles |= ControlStyles.UserPaint;

            if (ControlUtilities.GetStyle(control, ControlStyles.DoubleBuffer))
                info.Styles |= ControlStyles.DoubleBuffer;

            if (TryGetDrawMode(control, out DrawMode drawMode))
                info.DrawMode = drawMode;

            if (TryGetBorderStyle(control, out BorderStyle borderStyle))
                info.BorderStyle = borderStyle;

            control.ControlAdded += ControlAddedEventHandler;

            info.ResetControl += (c) => {
                control.ControlAdded -= ControlAddedEventHandler;
            };

            controlInfo[control] = info;

        }
        private bool RemoveControlInfo(Control control) {

            if (controlInfo.TryGetValue(control, out ControlInfo info)) {

                // Only disable styles that the control didn't have originally.
                // Controls like Panel and TabPage will have UserPaint enabled by default, and it should not be disabled.

                if (!info.Styles.HasFlag(ControlStyles.AllPaintingInWmPaint))
                    ControlUtilities.SetStyle(control, ControlStyles.AllPaintingInWmPaint, false);

                if (!info.Styles.HasFlag(ControlStyles.UserPaint))
                    ControlUtilities.SetStyle(control, ControlStyles.UserPaint, false);

                if (!info.Styles.HasFlag(ControlStyles.DoubleBuffer))
                    ControlUtilities.SetStyle(control, ControlStyles.DoubleBuffer, false);

                TrySetDrawMode(control, info.DrawMode);
                TrySetBorderStyle(control, info.BorderStyle);

                info.DoResetControl(control);

                controlInfo.Remove(control);

                return true;

            }

            return false;

        }

        private bool TryGetDrawMode(Control control, out DrawMode value) {

            PropertyInfo drawModeProperty = control.GetType().GetProperty("DrawMode", BindingFlags.Public | BindingFlags.Instance);

            if (drawModeProperty != null) {

                value = (DrawMode)drawModeProperty.GetValue(control, null);

                return true;

            }
            else {

                value = DrawMode.Normal;

                return false;

            }

        }
        private bool TrySetDrawMode(Control control, DrawMode value) {

            PropertyInfo drawModeProperty = control.GetType().GetProperty("DrawMode", BindingFlags.Public | BindingFlags.Instance);

            if (drawModeProperty != null) {

                drawModeProperty.SetValue(control, value, null);

                return true;

            }
            else {

                return false;

            }

        }
        private bool TryGetBorderStyle(Control control, out BorderStyle value) {

            PropertyInfo drawModeProperty = control.GetType().GetProperty("BorderStyle", BindingFlags.Public | BindingFlags.Instance);

            if (drawModeProperty != null) {

                value = (BorderStyle)drawModeProperty.GetValue(control, null);

                return true;

            }
            else {

                value = BorderStyle.None;

                return false;

            }

        }
        private bool TrySetBorderStyle(Control control, BorderStyle value) {

            PropertyInfo drawModeProperty = control.GetType().GetProperty("BorderStyle", BindingFlags.Public | BindingFlags.Instance);

            if (drawModeProperty != null) {

                drawModeProperty.SetValue(control, value, null);

                return true;

            }
            else {

                return false;

            }

        }

        private void ControlAddedEventHandler(object sender, ControlEventArgs e) {

            ApplyStyles(e.Control);

        }

    }

}