using Gsemac.Forms.Styles.Applicators;
using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Windows.Forms;

namespace ThemeTesting {

    public partial class TestForm :
        Form {

        // Public members

        public TestForm() {

            InitializeComponent();

        }

        private void Button1_Click(object sender, EventArgs e) {

            ClearStyles();

            applicator = new UserPaintStyleApplicator(LoadStyleSheet());

            ApplyStyles();

        }
        private void Button2_Click(object sender, EventArgs e) {

            ClearStyles();

            applicator = new PropertyStyleApplicator(LoadStyleSheet());

            ApplyStyles();

        }
        private void Button4_Click(object sender, EventArgs e) {

            ClearStyles();

        }

        private void button6_Click(object sender, EventArgs e) {

            using (Form form = new Form()) {

                form.StartPosition = FormStartPosition.CenterParent;

                form.Controls.Add(new Label() {
                    AutoSize = false,
                    Text = "New Forms can have styles applied automatically when using StyleApplicatorMessageFilter.",
                    Dock = DockStyle.Fill
                });

                form.ShowDialog(this);

            }

        }

        // Private members

        IStyleApplicator applicator;
        IMessageFilter messageFilter;

        private IStyleSheet LoadStyleSheet() {

            return StyleSheet.FromFile("DarkUI.css");

        }
        private void ApplyStyles() {

            messageFilter = new StyleApplicatorMessageFilter(applicator);

            applicator.ApplyStyles(this);

            Application.AddMessageFilter(messageFilter);

        }
        private void ClearStyles() {

            if (applicator != null)
                applicator.ClearStyles(this);

            Application.RemoveMessageFilter(messageFilter);

        }

    }

}