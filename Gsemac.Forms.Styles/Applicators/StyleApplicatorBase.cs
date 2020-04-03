using Gsemac.Forms.Styles.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators {

    public abstract class StyleApplicatorBase :
        IStyleApplicator {

        // Public members

        public void ApplyStyles(Control control, ControlStyleOptions options = ControlStyleOptions.Default) {

            // Save control info for all controls before applying styles, which prevents changes to inherited properties affecting what is saved.
            // This is relevant for the BackColor and ForeColor properties of child controls.

            AddControlInfoRecursive(control, options);

            // Apply styles.

            ApplyStylesRecursive(control, options);

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
            public FlatStyle FlatStyle { get; set; }
            public Color ForeColor { get; set; }
            public Color BackColor { get; set; }
            public bool UseVisualStyleBackColor { get; set; }

            public void DoResetControl(Control control) {

                ResetControl?.Invoke(control);

            }

        }

        protected abstract bool HasStyles(Control control);
        protected abstract void OnApplyStyles(Control control);
        protected virtual void OnClearStyles(Control control) { }

        protected static ControlInfo GetControlInfo(Control control) {

            if (controlInfo.TryGetValue(control, out ControlInfo info))
                return info;

            return null;

        }

        protected static bool GetStyle(Control control, ControlStyles styles) {

            return (bool)control.GetType()
                .GetMethod("GetStyle", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(control, new object[] { styles });

        }
        protected static void SetStyle(Control control, ControlStyles styles, bool value) {

            control.GetType()
                .GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(control, new object[] { styles, value });

        }

        // Private members

        private static readonly Dictionary<Control, ControlInfo> controlInfo = new Dictionary<Control, ControlInfo>();

        private void AddControlInfo(Control control) {

            RemoveControlInfo(control);

            ControlInfo info = new ControlInfo();

            // Store the control's initial styles so they can be restored in the future.

            if (GetStyle(control, ControlStyles.AllPaintingInWmPaint))
                info.Styles |= ControlStyles.AllPaintingInWmPaint;

            if (GetStyle(control, ControlStyles.UserPaint))
                info.Styles |= ControlStyles.UserPaint;

            if (GetStyle(control, ControlStyles.DoubleBuffer))
                info.Styles |= ControlStyles.DoubleBuffer;

            if (TryGetDrawMode(control, out DrawMode drawMode))
                info.DrawMode = drawMode;

            if (TryGetBorderStyle(control, out BorderStyle borderStyle))
                info.BorderStyle = borderStyle;

            if (TryGetFlatStyle(control, out FlatStyle flatStyle))
                info.FlatStyle = flatStyle;

            info.ForeColor = control.ForeColor;
            info.BackColor = control.BackColor;

            // This property is important for restoring the visual style of controls like Buttons and TabPages.

            if (TryGetUseVisualStyleBackColor(control, out bool useVisualStyleBackColor))
                info.UseVisualStyleBackColor = useVisualStyleBackColor;

            control.ControlAdded += ControlAddedEventHandler;

            info.ResetControl += (c) => {
                control.ControlAdded -= ControlAddedEventHandler;
            };

            controlInfo[control] = info;

        }
        private bool RemoveControlInfo(Control control) {

            if (controlInfo.TryGetValue(control, out ControlInfo info)) {

                // Remove event handlers before doing anything else to prevent them from modifying the control's appearance.

                info.DoResetControl(control);

                // Only disable styles that the control didn't have originally.
                // Controls like Panel and TabPage will have UserPaint enabled by default, and it should not be disabled.

                if (!info.Styles.HasFlag(ControlStyles.AllPaintingInWmPaint))
                    SetStyle(control, ControlStyles.AllPaintingInWmPaint, false);

                if (!info.Styles.HasFlag(ControlStyles.UserPaint))
                    SetStyle(control, ControlStyles.UserPaint, false);

                if (!info.Styles.HasFlag(ControlStyles.DoubleBuffer))
                    SetStyle(control, ControlStyles.DoubleBuffer, false);

                TrySetDrawMode(control, info.DrawMode);
                TrySetBorderStyle(control, info.BorderStyle);
                TrySetFlatStyle(control, info.FlatStyle);

                control.ForeColor = info.ForeColor;
                control.BackColor = info.BackColor;

                TrySetUseVisualStyleBackColor(control, info.UseVisualStyleBackColor);

                controlInfo.Remove(control);

                return true;

            }

            return false;

        }

        private void AddControlInfoRecursive(Control control, ControlStyleOptions options) {

            if (!options.HasFlag(ControlStyleOptions.RulesRequired) || HasStyles(control))
                AddControlInfo(control);
            else
                RemoveControlInfo(control);

            if (options.HasFlag(ControlStyleOptions.Recursive) && control.HasChildren)
                foreach (Control childControl in control.Controls)
                    AddControlInfoRecursive(childControl, options);

        }
        private void ApplyStylesRecursive(Control control, ControlStyleOptions options) {

            if (!options.HasFlag(ControlStyleOptions.RulesRequired) || HasStyles(control))
                OnApplyStyles(control);
            else
                OnClearStyles(control);

            if (options.HasFlag(ControlStyleOptions.Recursive) && control.HasChildren)
                foreach (Control childControl in control.Controls)
                    ApplyStylesRecursive(childControl, options);

            control.Invalidate();

        }

        private bool TryGetDrawMode(Control control, out DrawMode value) {

            PropertyInfo property = control.GetType().GetProperty("DrawMode", BindingFlags.Public | BindingFlags.Instance);

            if (property != null) {

                value = (DrawMode)property.GetValue(control, null);

                return true;

            }
            else {

                value = DrawMode.Normal;

                return false;

            }

        }
        private bool TrySetDrawMode(Control control, DrawMode value) {

            PropertyInfo property = control.GetType().GetProperty("DrawMode", BindingFlags.Public | BindingFlags.Instance);

            if (property != null) {

                property.SetValue(control, value, null);

                return true;

            }
            else {

                return false;

            }

        }
        private bool TryGetBorderStyle(Control control, out BorderStyle value) {

            PropertyInfo property = control.GetType().GetProperty("BorderStyle", BindingFlags.Public | BindingFlags.Instance);

            if (property != null) {

                value = (BorderStyle)property.GetValue(control, null);

                return true;

            }
            else {

                value = BorderStyle.None;

                return false;

            }

        }
        private bool TrySetBorderStyle(Control control, BorderStyle value) {

            PropertyInfo property = control.GetType().GetProperty("BorderStyle", BindingFlags.Public | BindingFlags.Instance);

            if (property != null) {

                property.SetValue(control, value, null);

                return true;

            }
            else {

                return false;

            }

        }
        private bool TryGetFlatStyle(Control control, out FlatStyle value) {

            PropertyInfo property = control.GetType().GetProperty("FlatStyle", BindingFlags.Public | BindingFlags.Instance);

            if (property != null) {

                value = (FlatStyle)property.GetValue(control, null);

                return true;

            }
            else {

                value = FlatStyle.Standard;

                return false;

            }

        }
        private bool TrySetFlatStyle(Control control, FlatStyle value) {

            PropertyInfo property = control.GetType().GetProperty("FlatStyle", BindingFlags.Public | BindingFlags.Instance);

            if (property != null) {

                property.SetValue(control, value, null);

                return true;

            }
            else {

                return false;

            }

        }
        private bool TryGetUseVisualStyleBackColor(Control control, out bool value) {

            PropertyInfo property = control.GetType().GetProperty("UseVisualStyleBackColor", BindingFlags.Public | BindingFlags.Instance);

            if (property != null) {

                value = (bool)property.GetValue(control, null);

                return true;

            }
            else {

                value = true;

                return false;

            }

        }
        private bool TrySetUseVisualStyleBackColor(Control control, bool value) {

            PropertyInfo property = control.GetType().GetProperty("UseVisualStyleBackColor", BindingFlags.Public | BindingFlags.Instance);

            if (property != null) {

                property.SetValue(control, value, null);

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