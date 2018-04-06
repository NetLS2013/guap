namespace Guap.ViewModels
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Guap.Models;
    using Guap.Service;

    using MvvmValidation;

    using Nethereum.Web3;

    using Xamarin.Forms;

    public class CreateTokenViewModel : BaseViewModel
    {
        private TokenService _tokenService;

        private Token _token;

        private string _contractAddress;

        private string _tokenName;

        private string _tokenSymbol;

        private int _decimals;

        private bool _isValid;

        private readonly Page _context;

        private ValidationErrorCollection _errors;

        public CreateTokenViewModel(Page context)
        {
            this._context = context;
            _tokenService = new TokenService(new Web3(GlobalSetting.Instance.EthereumNetwork));
            IsValid = false;
        }

        public string ContractAddress
        {
            get
            {
                return _contractAddress;
            }
            set
            {
                _contractAddress = value;
                OnPropertyChanged(nameof(ContractAddress));
               
                if (!this.ValidateTokenAddress())
                {
                    this.TokenName = string.Empty;
                    this.TokenSymbol = string.Empty;
                    this.Decimals = 0;

                    IsValid = false;
                    return;
                }

                IsBusy = true;
                try
                {
                    this._token = this._tokenService.GetTokenInfo(ContractAddress).Result;
                }
                catch (Exception e)
                {
                    this.TokenName = string.Empty;
                    this.TokenSymbol = string.Empty;
                    this.Decimals = 0;

                    IsValid = false;
                    return;
                }

                this.TokenName = _token.Name;
                this.TokenSymbol = _token.Symbol;
                this.Decimals = _token.Decimals;

                this.ValidateToken();

                if (this._token == null || !IsValid)
                {
                    this.TokenName = string.Empty;
                    this.TokenSymbol = string.Empty;
                    this.Decimals = 0;

                    IsValid = false;
                    return;
                }

                IsValid = true;
                IsBusy = false;
            }
        }

        public string TokenName
        {
            get
            {
                return _tokenName;
            }
            set
            {
                _tokenName = value;
                OnPropertyChanged(nameof(TokenName));
                this.ValidateToken();
            }
        }

        public string TokenSymbol
        {
            get
            {
                return _tokenSymbol;
            }
            set
            {
                _tokenSymbol = value;
                OnPropertyChanged(nameof(TokenSymbol));
                this.ValidateToken();
            }
        }

        public int Decimals
        {
            get
            {
                return _decimals;
            }
            set
            {
                _decimals = value;
                OnPropertyChanged(nameof(Decimals));
                this.ValidateToken();
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

        public bool IsValid
        {
            get
            {
                return this._isValid;
            }
            set
            {
                _isValid = value;
                this.OnPropertyChanged(nameof(this.IsValid));
            }
        }

        public ICommand CreateTokenCommand => new Command(async () => await OnTokenSave());

        public ICommand BackCommand => new Command(async () => await OnBack());

        private async Task OnTokenSave()
        {
            this.ValidateToken();

            if (!IsValid)
            {
                return;
            }

            // TODO save token

        }

        private async Task OnBack()
        {
            await this._context.Navigation.PopAsync();
        }

        private bool ValidateTokenAddress()
        {
            var validator = new ValidationHelper();
            validator.AddRequiredRule(() => ContractAddress, "Token address is required.");
            var resultOne = validator.ValidateAll();
            if (!resultOne.IsValid)
            {
                Errors = resultOne.ErrorList;

                return resultOne.IsValid;
            }

            bool isValidAddress;
            try
            {
                isValidAddress = new Regex("^(?=.{42}$)0x[a-zA-Z0-9]*").IsMatch(ContractAddress);
            }
            catch (Exception e)
            {
                isValidAddress = false;
            }
            validator.AddRule(ContractAddress, () => RuleResult.Assert(isValidAddress, "Token address is not valid Ethereum smart contract address."));

            var result = validator.ValidateAll();

            Errors = result.ErrorList;

            return result.IsValid;
        }

        private void ValidateToken()
        {
            var validator = new ValidationHelper();
            
            validator.AddRequiredRule(() => TokenName, "Token name is required.");
            validator.AddRequiredRule(() => TokenSymbol, "Token symbol is required.");
            validator.AddRequiredRule(() => TokenSymbol, "Token symbol is required.");

            var result = validator.ValidateAll();

            Errors = result.ErrorList;

            IsValid = result.IsValid;
        }
    }
}