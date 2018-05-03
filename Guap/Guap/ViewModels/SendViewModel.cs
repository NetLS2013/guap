using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace Guap.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Numerics;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Guap.Contracts;
    using Guap.Helpers;
    using Guap.Models;
    using Guap.Service;
    using Guap.Views.Modal;
    using Guap.Views.Profile;

    using MvvmValidation;

    using Nethereum.HdWallet;
    using Nethereum.Hex.HexTypes;
    using Nethereum.Util;
    using Nethereum.Web3;
    using Nethereum.Web3.Accounts;

    using Plugin.Connectivity;

    using Rg.Plugins.Popup.Extensions;

    using SQLite;

    using Xamarin.Forms;

    public class SendViewModel : BaseViewModel
    {
        private Page _context;
        private readonly BottomTabbedPage _tabbedContext;
        private TokenService _tokenService;
        private EthereumService _ethereumService;
        private IRepository<Token> _repository;
        private ObservableCollection<Token> _tokens;
        private Token _token;
        private int _tokenIndex;
        private ValidationErrorCollection _errors;
        private bool _isValid;
        private string _receiverAddress;
        private BigDecimal? _amount;
        private string _amountString;

        public ICommand SendCommand => new Command(async () => await OnSend());
        public ICommand ContactCommand => new Command(async () => await OnContact());
        public ICommand ScanCommand => new Command(OnScan);
        public ICommand RefreshBalanceCommand => new Command(async () => await OnRefreshBalance());

        public ObservableCollection<Token> Tokens
        {
            get
            {
                return _tokens;
            }
            set
            {
                this._tokens = value;
                OnPropertyChanged(nameof(Tokens));
            }
        }

        public Token Token
        {
            get
            {
                return _token;
            }
            set
            {
                this._token = value;
                this.OnRefreshBalance();
                OnPropertyChanged(nameof(Token));
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

        public string ReceiverAddress
        {
            get
            {
                return this._receiverAddress;
            }
            set
            {
                _receiverAddress = value;
                this.OnPropertyChanged(nameof(this.ReceiverAddress));
                IsValid = true;
            }
        }

        public string Amount
        {
            get
            {
                return this._amountString?.Replace(",", ".");
            }
            set
            {
                _amountString = value;
                this.OnPropertyChanged(nameof(this.Amount));
                IsValid = true;
            }
        }


        public int TokenSelectedIndex
        {
            get
            {
                return this._tokenIndex;
            }
            set
            {
                _tokenIndex = value;
                OnPropertyChanged(nameof(TokenSelectedIndex));
                Token = Tokens[_tokenIndex];
            }
        }

        public SendViewModel(Page context, BottomTabbedPage tabbedContext)
        {
            _context = context;
            _tabbedContext = tabbedContext;
            
            _token = null;
            
            string databasePath = DependencyService.Get<ISQLite>().GetDatabasePath(GlobalSetting.Instance.DbName);
            
            _tokenService = new TokenService(new Web3(GlobalSetting.Account, GlobalSetting.Instance.EthereumNetwork));
            _ethereumService = new EthereumService(new Web3(GlobalSetting.Account, GlobalSetting.Instance.EthereumNetwork));
            _repository = new Repository<Token>(new SQLiteAsyncConnection(databasePath));

            InitializeTokens();
        }

        public async void InitializeTokens()
        {
            var tokens = await _repository.Get();
            
            tokens.Insert(0, GlobalSetting.Instance.Guap);
            tokens.Insert(1, GlobalSetting.Instance.Ethereum);

            Tokens = new ObservableCollection<Token>(tokens);
        }

        public void SetReceiverInfo(string address, string amount)
        {
            CleanFields();

            ReceiverAddress = address;
            Amount = amount;
        }

        public async void SetReceiverTokenInfo(string addressContract, string addressReceiver, string amount)
        {
            CleanFields();

            try
            {
                var tokenSend = Tokens.First(token => token.Address == addressContract);
                Token = tokenSend;
            }
            catch (Exception e)
            {
                var tempToken = await _tokenService.GetTokenInfo(addressContract);
                await _repository.Insert(tempToken);

                Tokens.Add(tempToken);
                OnPropertyChanged(nameof(Tokens));

                Token = tempToken;
            }
            
            ReceiverAddress = addressReceiver;
            BigInteger.TryParse(amount, out BigInteger realAmount);
            Amount = TokenService.ConvertToBigDecimal(realAmount, Token.Decimals).ToString();
        }

        private async Task OnSend()
        {
            ValidateTransaction();
            if (IsValid)
            {
                string title = string.Empty;
                string transactionHash = string.Empty;

                if (this._token.Id == -1)
                {
                    transactionHash = await this._ethereumService.SendEther(
                                          new Wallet((string)Settings.Get(Settings.Key.MnemonicPhrase), "").GetAccount(
                                              0),
                                          ReceiverAddress,
                                          this._amount.Value);
                    if (!string.IsNullOrWhiteSpace(transactionHash))
                    {
                        title = "Your Ethereum was send successfully.";
                        this.CleanFields();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {

                    transactionHash = await this._tokenService.Send(
                                          Token,
                                          new Wallet((string)Settings.Get(Settings.Key.MnemonicPhrase), "").GetAccount(
                                              0),
                                          ReceiverAddress,
                                          this._amount.Value);

                    if (!string.IsNullOrWhiteSpace(transactionHash))
                    {
                        title = "Your Ethereum was send successfully.";
                        this.CleanFields();
                    }
                    else
                    {
                        return;
                    }

                }

                Device.BeginInvokeOnMainThread(
                    async () => await this._context.Navigation.PushPopupAsync(
                                    new TransactionModalPage(title, transactionHash)));


            }
        }

        private void OnScan()
        {
            Device.BeginInvokeOnMainThread(() =>_tabbedContext.CurrentPage = _tabbedContext.Children[2]);
        }

        private async Task OnContact()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Contacts);

            if (status != PermissionStatus.Granted)
            {
                var result = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Contacts);

                if (result[Permission.Contacts] != PermissionStatus.Granted)
                {
                    return;
                }
            }
            
            var page = new ContactListPage();
            
            page.ContactListViewModel.SelectHandler += address =>
            {
                ReceiverAddress = address;
                
                _context.Navigation.PopAsync();
            };
            
            await _context.Navigation.PushAsync(page);
        }

        private async Task OnRefreshBalance()
        {
            if (this._token == null)
            {
                DependencyService.Get<IMessage>().ShortAlert("Select token.");
                return;
            }

            if (this._token.Id == -1)
            {
                Token.Balance = await this._ethereumService.GetBalance(GlobalSetting.Account.Address);
            }
            else
            {
                Token.Balance = await this._tokenService.GetBalance(Token, GlobalSetting.Account.Address);
            }
            
            OnPropertyChanged(nameof(Token));
        }

        private void ValidateTransaction()
        {
            try
            {
                this._amount = BigDecimal.Parse(this._amountString);
            }
            catch (Exception e)
            {
                this._amount = null;
            }

            var validator = new ValidationHelper();

            validator.AddRequiredRule(() => ReceiverAddress, "Address receiver is required.");
            validator.AddRequiredRule(() => _amount, "Valid amount is required.");
            validator.AddRequiredRule(() => Token, "Select token to send.");

            var resultTemp = validator.ValidateAll();
            if (!resultTemp.IsValid)
            {
                Errors = resultTemp.ErrorList;
                IsValid = resultTemp.IsValid;
                return;
            }

            bool isValidAddress;
            try
            {
                isValidAddress = new Regex("^(?=.{42}$)0x[a-zA-Z0-9]*").IsMatch(ReceiverAddress);
            }
            catch (Exception e)
            {
                isValidAddress = false;
            }
            validator.AddRule(ReceiverAddress, () => RuleResult.Assert(isValidAddress, "Receiver address is not valid Ethereum account address."));
            validator.AddRule(Amount, () => RuleResult.Assert(this._amount > new BigDecimal(0), "Amount is zero."));
            validator.AddRule(Amount, () => RuleResult.Assert(this._amount < Token.Balance, "You don't have enough funds."));

            var result = validator.ValidateAll();

            Errors = result.ErrorList;

            IsValid = result.IsValid;
        }

        private void CleanFields()
        {
            ReceiverAddress = null;
            Amount = null;
            _token = null;
            OnPropertyChanged(nameof(Token));
        }
    }
}