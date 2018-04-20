namespace Guap.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Guap.Helpers;
    using Guap.Views;

    using MvvmValidation;

    using Xamarin.Forms;

    public class InputMnemonicViewModel : BaseViewModel
    {
     
        private readonly Page _context;

        private ValidationErrorCollection _errors;

        private string _error;

        private string _inputMnemonic;

        private bool _isInvalid;

        private string _headerText;

        private bool _isCustomHeader;

        public ICommand CompleteMnemonicPhraseCommand => new Command(async () => await OnPageCompleteMnemonic());

        public ICommand BackToMnemonicPhraseCommand => new Command(async () => await OnBack());

        public Action<string> SuccessAction { get; set; }

        public List<KeyValuePair<string, Func<string, bool>>> Validators { get; set; }

        public InputMnemonicViewModel(Page context, CommonPageSettings pageSettings)
        {
            this.HeaderText = pageSettings.HeaderText;
            this.IsCustomHeader = pageSettings.IsShowCustomHeader;
            this.Text = pageSettings.Text;
            this.LeftButtonText = pageSettings.LeftButtonText;

            this._context = context;
            IsInvalid = false;
            Validators = new List<KeyValuePair<string,Func<string, bool>>>();
        }

        public string HeaderText
        {
            get
            {
                return _headerText;
            }
            set
            {
                _headerText = value;
                OnPropertyChanged(nameof(HeaderText));
            }
        }

        public string Text { get; set; }

        public string LeftButtonText { get; set; }

        public bool IsCustomHeader
        {
            get
            {
                return _isCustomHeader;
            }
            set
            {
                _isCustomHeader = value;
                OnPropertyChanged(nameof(IsCustomHeader));
            }
        }

        public string InputMnemonic
        {
            get
            {
                return this._inputMnemonic;
            }
            set
            {
                _inputMnemonic = value;
                this.OnPropertyChanged(nameof(this.InputMnemonic));
            }
        }

        public string Error
        {
            get
            {
                return this._error;
            }
            set
            {
                _error = value;
                this.OnPropertyChanged(nameof(this.Error));
            }
        }

        public bool IsInvalid
        {
            get
            {
                return this._isInvalid;
            }
            set
            {
                _isInvalid = value;
                this.OnPropertyChanged(nameof(this.IsInvalid));
            }
        }

        public ValidationErrorCollection Errors
        {
            get
            {
                return _errors;
            }
            set
            {
                _errors = value;
                OnPropertyChanged(nameof(Errors));
            }
        }

        private async Task OnPageCompleteMnemonic()
        {
            if (!this.ValidateMnemonicPhrase())
            {
                Error = this._errors.FirstOrDefault().ErrorText;
                IsInvalid = true;
                return;
            }
            IsInvalid = false;

            this.SuccessAction(InputMnemonic);
        }

        private async Task OnBack()
        {
            await this._context.Navigation.PopAsync();
        }

        private bool ValidateMnemonicPhrase()
        {
            var validator = new ValidationHelper();
            validator.AddRequiredRule(() => this.InputMnemonic, "Mnemonic phrase is required.");
            foreach (var func in Validators)
            {
                validator.AddRule(nameof(this.InputMnemonic), () => RuleResult.Assert(func.Value(InputMnemonic), func.Key));
            }

            var result = validator.ValidateAll();

            Errors = result.ErrorList;

            return result.IsValid;
        }
    }
}

