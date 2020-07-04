using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators {

    public class StyleApplicatorMessageFilter :
        IMessageFilter {

        // Public members

        public StyleApplicatorMessageFilter(IStyleApplicator styleApplicator) {

            this.styleApplicator = styleApplicator;

        }

        public bool PreFilterMessage(ref Message message) {

            Control control = Control.FromHandle(message.HWnd);

            if (control is Form form && !seenForms.Contains(form)) {

                form.Shown += FormShownEventHandler;
                form.FormClosed += FormClosedEventHandler;

                seenForms.Add(form);

            }

            return false;
        }

        // Private members

        private readonly IStyleApplicator styleApplicator;
        private readonly HashSet<Form> seenForms = new HashSet<Form>();

        private void FormShownEventHandler(object sender, EventArgs e) {

            if (sender is Form form) {

                styleApplicator.ApplyStyles(form);

            }

        }
        private void FormClosedEventHandler(object sender, EventArgs e) {

            if (sender is Form form && seenForms.Contains(form)) {

                form.Shown -= FormShownEventHandler;
                form.FormClosed -= FormClosedEventHandler;

                seenForms.Remove(form);

            }

        }

    }

}