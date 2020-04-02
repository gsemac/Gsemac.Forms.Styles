using Gsemac.Forms.Styles.Applicators;
using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.IO;
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

            applicator.ApplyStyles(this, ControlStyleOptions.Default & ~ControlStyleOptions.RulesRequired);

        }
        private void Button2_Click(object sender, EventArgs e) {

            ClearStyles();

            applicator = new PropertyStyleApplicator(LoadStyleSheet());

            applicator.ApplyStyles(this);

        }
        private void Button4_Click(object sender, EventArgs e) {

            applicator.ClearStyles(this);

        }

        // Private members

        IStyleApplicator applicator;

        private IStyleSheet LoadStyleSheet() {

            using (FileStream fstream = new FileStream("TestStyle.css", FileMode.Open))
                return StyleSheet.FromStream(fstream);

        }
        private void ClearStyles() {

            if (applicator != null)
                applicator.ClearStyles(this);

        }

    }

}