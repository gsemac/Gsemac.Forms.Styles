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

        private void button1_Click(object sender, EventArgs e) {

            ClearStyles();

            applicator = new UserPaintStyleApplicator(LoadStyleSheet());

            applicator.ApplyStyles(this);

        }
        private void button2_Click(object sender, EventArgs e) {

            ClearStyles();

            applicator = new PropertyStyleApplicator(LoadStyleSheet());

            applicator.ApplyStyles(this);

        }
        private void button4_Click(object sender, EventArgs e) {

            applicator.ClearStyles(this);

        }

        // Private members

        IStyleApplicator applicator;

        private IStyleSheet LoadStyleSheet() {

            using (FileStream fstream = new FileStream("theme.txt", FileMode.Open))
                return StyleSheet.FromStream(fstream);

        }
        private void ClearStyles() {

            if (applicator != null)
                applicator.ClearStyles(this);

        }

    }

}