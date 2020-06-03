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

            applicator.ApplyStyles(this);

        }
        private void Button2_Click(object sender, EventArgs e) {

            ClearStyles();

            applicator = new PropertyStyleApplicator(LoadStyleSheet());

            applicator.ApplyStyles(this);

        }
        private void Button4_Click(object sender, EventArgs e) {

            if (applicator != null)
                applicator.ClearStyles(this);

        }

        // Private members

        IStyleApplicator applicator;

        private IStyleSheet LoadStyleSheet() {

            return StyleSheet.FromFile("TestStyle.css");

        }
        private void ClearStyles() {

            if (applicator != null)
                applicator.ClearStyles(this);

        }

    }

}