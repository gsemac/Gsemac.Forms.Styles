using Gsemac.Forms.Styles;
using Gsemac.Forms.Styles.StyleSheets;
using System;
using System.Windows.Forms;

namespace ThemeTesting {

    public partial class TestForm :
        Form {

        // Public members

        public TestForm() {

            InitializeComponent();

            InitializeStyleManager();

        }

        private void ApplyUserPaintStylesButton_Click(object sender, EventArgs e) {

            styleManagerOptions.EnableCustomRendering = true;

            styleManager.ApplyStyles();

        }
        private void ApplyPropertiesStylesButton_Click(object sender, EventArgs e) {

            styleManagerOptions.EnableCustomRendering = false;

            styleManager.ApplyStyles();

        }
        private void ClearStylesButton_Click(object sender, EventArgs e) {

            styleManager.ResetStyles();

        }

        private void ShowNewFormButton_Click(object sender, EventArgs e) {

            Form form = new Form {
                StartPosition = FormStartPosition.CenterParent
            };

            form.Controls.Add(new Label() {
                AutoSize = false,
                Text = "New Forms can have styles applied automatically when using StyleApplicatorMessageFilter.",
                Dock = DockStyle.Fill
            });

            form.Show(this);

        }

        // Private members

        private readonly StyleManagerOptions styleManagerOptions = new StyleManagerOptions();
        private IStyleManager styleManager;

        private void InitializeStyleManager() {

            styleManager = new FormsStyleManager(styleManagerOptions);

            InitializeStyleSheets();

        }
        private void InitializeStyleSheets() {

            IStyleSheetFactory styleSheetFactory = StyleSheetFactory.Default;

            //styleManager.StyleSheets.Add(styleSheetFactory.FromFile("Test.css"));
            //styleManager.StyleSheets.Add(styleSheetFactory.FromFile("DarkUI.css"));

        }

    }

}