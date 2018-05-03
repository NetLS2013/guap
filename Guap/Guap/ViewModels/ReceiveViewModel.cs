namespace Guap.ViewModels
{
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Guap.Contracts;
    using Guap.Helpers;
    using Guap.Models;
    using Guap.Service;
    using Guap.Views.Profile;

    using Nethereum.Hex.HexTypes;
    using Nethereum.Util;
    using Nethereum.Web3;
    using Nethereum.Web3.Accounts;

    using Plugin.Connectivity;

    using SQLite;

    using Xamarin.Forms;

    public class ReceiveViewModel : BaseViewModel
    {
        private Page _context;
        private readonly BottomTabbedPage _tabbedContext;

        private TokenService _tokenService;

        private EthereumService _ethereumService;

        private Token _token;

        private decimal _amount;

        private string _requestString;

        public ICommand RequestAmountCommand => new Command(async () => await OnRequestAmount());

        public ICommand RefreshBalanceCommand => new Command(async () => OnRefreshBalance());

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

        public string Address
        {
            get
            {
                return GlobalSetting.Account.Address;
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

        public async void OnRefreshBalance()
        {
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

        public ReceiveViewModel(Page context, BottomTabbedPage tabbedContext)
        {
            _context = context;
            _tabbedContext = tabbedContext;

            _token = GlobalSetting.Instance.Ethereum;
            RequestString = GlobalSetting.Account.Address;
            
            _ethereumService = new EthereumService(new Web3(GlobalSetting.Account, GlobalSetting.Instance.EthereumNetwork));
            _tokenService = new TokenService(new Web3(GlobalSetting.Account, GlobalSetting.Instance.EthereumNetwork));
            
            if (CrossConnectivity.Current.IsConnected)
            {
                OnRefreshBalance();
            }
        }

        private async Task OnRequestAmount()
        {
            var page = new EnterAmountPage();
            
            page.ViewModel.AmountChanged += obj =>
            {
                Amount = obj;
                
                GenerateQueryString();
            };
            
            page.ViewModel.TokenChanged += obj =>
            {
                _token = obj;
                
                OnPropertyChanged(nameof(Token));
                GenerateQueryString();
            };

            await _context.Navigation.PushSingleAsync(page);
        }

        private void GenerateQueryString()
        {
            if (Token == null || Token.Id == -1)
            {
                if (this._amount > decimal.Zero)
                {
                    RequestString = string.Format(
                        "ethereum:{0}?value={1}",
                        GlobalSetting.Account.Address,
                        this._amount);
                }
                else
                {
                    RequestString = GlobalSetting.Account.Address;
                }
            }
            else
            {
                RequestString = string.Format(
                    "ethereum:{0}?data=0xa9059cbb{1}{2}",
                    Token.Address,
                    TokenService.Convert64Hex(new HexBigInteger(GlobalSetting.Account.Address)),
                    TokenService.Convert64Hex(new HexBigInteger(TokenService.ConvertToBigInteger(new BigDecimal(this._amount), Token.Decimals))));
            }
        }

    }
}