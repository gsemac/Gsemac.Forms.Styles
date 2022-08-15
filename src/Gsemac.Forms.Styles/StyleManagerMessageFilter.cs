using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles {

    internal class StyleManagerMessageFilter :
        IMessageFilter {

        // Public members

        public StyleManagerMessageFilter(IControlStyleManager styleManager) {

            if (styleManager is null)
                throw new ArgumentNullException(nameof(styleManager));

            this.styleManager = styleManager;

        }

        public bool PreFilterMessage(ref Message message) {

            Control control = Control.FromHandle(message.HWnd);

            if (control is Form form && !seenForms.Contains(form)) {

                form.Shown += FormShownEventHandler;
                form.FormClosed += FormClosedEventHandler;
                form.Disposed += FormDisposedEventHandler;

                seenForms.Add(form);

            }

            return false;
        }

        // Private members

        private readonly IControlStyleManager styleManager;
        private readonly HashSet<Form> seenForms = new HashSet<Form>();

        private void FormShownEventHandler(object sender, EventArgs e) {

            if (sender is Form form)
                styleManager.ApplyStyles(form);

        }
        private void FormClosedEventHandler(object sender, EventArgs e) {

            /* Why clear styles when the form is closed?
             * I encountered an issue with ComoboBox, where text in the drop-down area would have a different font when re-opening the form (as if UseCompatibleTextRendering was enabled).
             * This occurred when opening the form through a static instance, where the same instance would be re-opened.
             * This occurred even if styles were not re-applied, meaning closing the form while styles are applied seems to be triggering the problem.
             */

            if (sender is Form form) {

                // Hide the form before clearing styles so that the user doesn't see the styles being cleared.

                form.Visible = false;

                styleManager.ResetStyles(form);

            }

        }
        private void FormDisposedEventHandler(object sender, EventArgs e) {

            if (sender is Form form && seenForms.Contains(form)) {

                RemoveEventHandlers(form);

                seenForms.Remove(form);

            }

        }

        private void RemoveEventHandlers(Form form) {

            form.Shown -= FormShownEventHandler;
            form.Disposed -= FormDisposedEventHandler;

        }

    }

}