using Gsemac.Forms.Styles.Applicators2.Properties;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Applicators2.UserPaint {

    internal sealed class UserPaintStyleApplicatorFactory :
        IStyleApplicatorFactory {

        // Public members

        public UserPaintStyleApplicatorFactory() :
            this(UserPaintStyleApplicatorFactoryOptions.Default) {
        }
        public UserPaintStyleApplicatorFactory(IUserPaintStyleApplicatorFactoryOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            this.options = options;

            InitializeApplicatorDictionary();

        }

        public IStyleApplicator Create(Type forType) {

            if (applicators.TryGetValue(forType, out var applicator))
                return applicator;

            switch (forType.FullName) {

                case "System.Windows.Forms.UpDownBase+UpDownButtons":
                    return new UserPaintStyleApplicator<Control>();

                default:
                    return fallbackApplicatorFactory.Create(forType);

            }

        }

        // Private members

        private readonly IUserPaintStyleApplicatorFactoryOptions options;
        private readonly IDictionary<Type, IStyleApplicator> applicators = new Dictionary<Type, IStyleApplicator>();
        private readonly IStyleApplicatorFactory fallbackApplicatorFactory = new PropertyStyleApplicatorFactory();

        private void InitializeApplicatorDictionary() {

            applicators.Add(typeof(Button), new UserPaintStyleApplicator<Button>());
            applicators.Add(typeof(CheckBox), new UserPaintStyleApplicator<CheckBox>());
            applicators.Add(typeof(ComboBox), new UserPaintStyleApplicator<ComboBox>());
            applicators.Add(typeof(GroupBox), new UserPaintStyleApplicator<GroupBox>());
            applicators.Add(typeof(Label), new UserPaintStyleApplicator<Label>());
            applicators.Add(typeof(ListBox), new ListBoxUserPaintStyleApplicator(options.CustomScrollBarsEnabled));
            applicators.Add(typeof(NumericUpDown), new NumericUpDownUserPaintStyleApplicator());
            applicators.Add(typeof(ProgressBar), new UserPaintStyleApplicator<ProgressBar>());
            applicators.Add(typeof(RadioButton), new UserPaintStyleApplicator<RadioButton>());
            applicators.Add(typeof(RichTextBox), new RichTextBoxUserPaintStyleApplicator());
            applicators.Add(typeof(TabControl), new UserPaintStyleApplicator<TabControl>());
            applicators.Add(typeof(TextBox), new TextBoxUserPaintStyleApplicator());

        }

    }

}