namespace Guap.ViewModels
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Guap.Views;

    using MvvmValidation;

    using Xamarin.Forms;

    public class InputMnemonicViewModel : BaseViewModel
    {
        private readonly MnemonicPhraseViewModel _mnemonicPhraseViewModel;

        private readonly Page _context;

        private ValidationErrorCollection _errors;

        private string _error;

        private string _inputMnemonic;

        private bool _isInvalid;

        public ICommand CompleteMnemonicPhraseCommand => new Command(async () => await OnPageCompleteMnemonic());

        public ICommand BackToMnemonicPhraseCommand => new Command(async () => await OnBack());


        public InputMnemonicViewModel(Page context, MnemonicPhraseViewModel model)
        {
            this._mnemonicPhraseViewModel = model;
            this._context = context;
            IsInvalid = false;
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

            // TODO set next page, assign somewhere words list
            await _context.Navigation.PushAsync(new SuccessSignup());
        }

        private async Task OnBack()
        {
            await this._context.Navigation.PopAsync();
        }

        private bool ValidateMnemonicPhrase()
        {
            var validator = new ValidationHelper();
            validator.AddRequiredRule(() => this.InputMnemonic, "Mnemonic phrase is required.");
            validator.AddRule(
                nameof(this.InputMnemonic),
                () => RuleResult.Assert(
                    this.InputMnemonic == string.Join(" ", this._mnemonicPhraseViewModel.Wallet.Words),
                    "The mnemonic phrase you entered is incorrect." + Environment.NewLine + "Typos can cause this."
                    + Environment.NewLine + "Please review your phrase and try again"));

            var result = validator.ValidateAll();

            Errors = result.ErrorList;

            return result.IsValid;
        }
    }
}

