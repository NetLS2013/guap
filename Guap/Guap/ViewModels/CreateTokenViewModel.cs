namespace Guap.ViewModels
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Guap.Contracts;
    using Guap.Models;
    using Guap.Service;

    using MvvmValidation;

    using Nethereum.Web3;

    using SQLite;

    using Xamarin.Forms;

    public class CreateTokenViewModel : BaseViewModel
    {
        private TokenService _tokenService;

        private IRepository<Token> _repository;

        private Token _token;

        private bool _isValid;

        private bool _isEdit;

        private readonly Page _context;

        private ValidationErrorCollection _errors;

        private DashboardViewModel _dashboardViewModel;

        public CreateTokenViewModel(Page context, DashboardViewModel viewModel, Token token = null)
        {
            this._context = context;
            this._dashboardViewModel = viewModel;
            
            if (token != null)
            {
                Token = token;
                this.IsEdit = true;
                this.IsValid = true;
            }
            else
            {
                Token = new Token();
                IsValid = false;
            }
            
            string databasePath = DependencyService.Get<ISQLite>().GetDatabasePath(GlobalSetting.Instance.DbName);
            _tokenService = new TokenService(new Web3(GlobalSetting.Instance.EthereumNetwork));
            _repository = new Repository<Token>(new SQLiteAsyncConnection(databasePath));
        }

        public string ContractAddress
        {
            get
            {
                return this._token.Address;
            }
            set
            {
                this._token.Address = value;
                OnPropertyChanged(nameof(ContractAddress));
               
                if (!IsEdit)
                {
                    if (!this.ValidateTokenAddress())
                    {
                        ClearFields();

                        IsValid = false;
                        return;
                    }

                    IsBusy = true;
                    try
                    {
                        Token = this._tokenService.GetTokenInfo(ContractAddress).Result;
                    }
                    catch (Exception e)
                    {
                        ClearFields();

                        IsValid = false;
                        return;
                    }

                    this.ValidateToken();

                    if (this._token == null || !IsValid)
                    {
                        ClearFields();

                        IsValid = false;
                        return;
                    }

                    IsValid = true;
                    IsBusy = false;
                }
               
            }
        }

        public Token Token
        {
            get
            {
                return this._token;
            }
            set
            {
                this._token = value;
                OnPropertyChanged(nameof(Token));
                OnPropertyChanged(nameof(ContractAddress));
                OnPropertyChanged(nameof(TokenSymbol));
                OnPropertyChanged(nameof(TokenName));
                OnPropertyChanged(nameof(Decimals));
            }
        }

        public string TokenName
        {
            get
            {
                return this._token.Name;
            }
            set
            {
                this._token.Name = value;
                OnPropertyChanged(nameof(TokenName));
                IsValid = true;
            }
        }

        public string TokenSymbol
        {
            get
            {
                return this._token.Symbol;
            }
            set
            {
                this._token.Symbol = value;
                OnPropertyChanged(nameof(TokenSymbol));
                IsValid = true;
            }
        }

        public int Decimals
        {
            get
            {
                return this._token.Decimals;
            }
            set
            {
                this._token.Decimals = value;
                OnPropertyChanged(nameof(Decimals));
                IsValid = true;
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

        public bool IsEdit
        {
            get
            {
                return this._isEdit;
            }
            set
            {
                _isEdit = value;
                this.OnPropertyChanged(nameof(this.IsEdit));
            }
        }

        public ICommand SaveTokenCommand => new Command(async () =>
            {
                if (IsEdit)
                {
                    await this.OnTokenEdit();
                }
                else
                {
                    await this.OnTokenSave();
                }
            });

        public ICommand BackCommand => new Command(async () => await OnBack());

        public ICommand DeleteCommand => new Command(async () => await OnTokenDelete());

        private async Task OnTokenSave()
        {
            this.ValidateToken();

            if (!IsValid || !this.ValidateTokenAddress())
            {
                return;
            }
          
            await _repository.Insert(this._token);
            await this._context.Navigation.PopAsync();
            _dashboardViewModel.InitializeTokens();
        }

        private async Task OnTokenEdit()
        {
            this.ValidateToken();

            if (!IsValid)
            {
                return;
            }

            await _repository.Update(this._token);
            await this._context.Navigation.PopAsync();
            _dashboardViewModel.InitializeTokens();
        }

        private async Task OnTokenDelete()
        {
            await _repository.Delete(this._token);
            await this._context.Navigation.PopAsync();
            _dashboardViewModel.InitializeTokens();
        }

        private async Task OnBack()
        {
            await this._context.Navigation.PopAsync();
            _dashboardViewModel.InitializeTokens();
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

        private void ClearFields()
        {
            this.TokenName = string.Empty;
            this.TokenSymbol = string.Empty;
            this.Decimals = 0;
        }
    }
}