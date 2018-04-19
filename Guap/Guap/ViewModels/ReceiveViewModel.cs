namespace Guap.ViewModels
{
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Guap.Contracts;
    using Guap.Helpers;
    using Guap.Models;
    using Guap.Service;
    using Guap.Views.Profile;

    using Nethereum.Web3;
    using Nethereum.Web3.Accounts;

    using SQLite;

    using Xamarin.Forms;

    public class ReceiveViewModel : BaseViewModel
    {
        private Page _context;

        private TokenService _tokenService;

        private EthereumService _ethereumService;

        private Account _account;

        private Token _token;

        private decimal _amount;

        private string _requestString;


        public ICommand RequestAmountCommand => new Command(async () => await OnRequestAmount());

        public ICommand RefreshBalanceCommand => new Command(async () => OnRefreshBalance());

        public Account Account
        {
            get
            {
                return _account;
            }
            set
            {
                _account = value;

                OnPropertyChanged(nameof(Account));
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
            }
        }

        public decimal Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                _amount = value;
                if (this._amount > decimal.Zero)
                {
                    RequestString = string.Format("ethereum:{0}?value={1}", this._account.Address, this._amount).ToString();
                }
                OnPropertyChanged(nameof(Amount));
            }
        }

        public string RequestString
        {
            get
            {
                return _requestString;
            }
            set
            {
                _requestString = value;
                
                OnPropertyChanged(nameof(RequestString));
            }
        }

        private async void OnRefreshBalance()
        {
            if (this._token.Id == -1)
            {
                Token.Balance = await this._ethereumService.GetBalance(this._account.Address);
            }
            else
            {
                Token.Balance = await this._tokenService.GetBalance(Token, this._account.Address);
            }

            OnPropertyChanged(nameof(Token));
        }

        public ReceiveViewModel(Page context)
        {
            this._context = context;

            this._account = GlobalSetting.Instance.Account;
            this._token = GlobalSetting.Instance.Ethereum;
            RequestString = _account.Address;

            _ethereumService = new EthereumService(new Web3(_account, GlobalSetting.Instance.EthereumNetwork));
            OnRefreshBalance();

            GlobalSetting.Instance.AccountUpdate += () => { InitializeAccount(); };
        }

        private async void InitializeAccount()
        {
            Account = GlobalSetting.Instance.Account;
            _tokenService = new TokenService(new Web3(Account, GlobalSetting.Instance.EthereumNetwork));
            _ethereumService = new EthereumService(new Web3(Account, GlobalSetting.Instance.EthereumNetwork));
            OnRefreshBalance();
        }

        private async Task OnRequestAmount()
        {
            var page = new EnterAmountPage();
            page.ViewModel.AmountChanged += obj => Amount = obj;

            await this._context.Navigation.PushAsync(page);
        }
    }
}