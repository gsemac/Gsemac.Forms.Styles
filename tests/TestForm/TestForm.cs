using Gsemac.Forms.Styles;
using Gsemac.Forms.Styles.Applicators;
using Gsemac.Forms.Styles.Applicators2;
using Gsemac.Forms.Styles.StyleSheets;
using Gsemac.Forms.Styles.StyleSheets.Extensions;
using System;
using System.Windows.Forms;

namespace ThemeTesting {

    public partial class TestForm :
        Form {

        // Public members

        public TestForm() {

            InitializeComponent();

            INodeStyleApplicatorFactory styleApplicatorFactory = new UserPaintControlStyleApplicatorFactory();
            IStyleManager styleManager = new StyleManager(styleApplicatorFactory);

            styleManager.StyleSheets.Add(LoadStyleSheet());

            styleManager.ApplyStyles();

        }

        private void Button1_Click(object sender, EventArgs e) {

            styleApplicator = new UserPaintStyleApplicator(LoadStyleSheet(),
                StyleApplicatorOptions.AddMessageFilter | StyleApplicatorOptions.DisposeStyleSheet);

            styleApplicator.ApplyStyles();

        }
        private void Button2_Click(object sender, EventArgs e) {

            styleApplicator = new PropertyStyleApplicator(LoadStyleSheet(),
                StyleApplicatorOptions.AddMessageFilter | StyleApplicatorOptions.DisposeStyleSheet);

            styleApplicator.ApplyStyles();

        }
        private void Button4_Click(object sender, EventArgs e) {

            ClearStyles();

        }

        private void button6_Click(object sender, EventArgs e) {

            Form form = new Form();

            form.StartPosition = FormStartPosition.CenterParent;

            form.Controls.Add(new Label() {
                AutoSize = false,
                Text = "New Forms can have styles applied automatically when using StyleApplicatorMessageFilter.",
                Dock = DockStyle.Fill
            });

            form.Show(this);

        }

        // Private members

        Gsemac.Forms.Styles.Applicators.IStyleApplicator styleApplicator;

        private IStyleSheet LoadStyleSheet() {

            //return new StyleSheetFactory().FromFile("DarkUI.css");
            return new StyleSheetFactory().FromFile("Test.css");

        }
        private void ClearStyles() {

            if (styleApplicator != null)
                styleApplicator.ClearStyles();

        }

    }

}