﻿namespace Guap.ViewModels
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

    using Plugin.Connectivity;

    using SQLite;

    using Xamarin.Forms;

    public class ReceiveViewModel : BaseViewModel
    {
        private Page _context;
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
                    RequestString = string.Format("ethereum:{0}?value={1}", GlobalSetting.Instance.Account.Address, this._amount);
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
                Token.Balance = await this._ethereumService.GetBalance(GlobalSetting.Instance.Account.Address);
            }
            else
            {
                Token.Balance = await this._tokenService.GetBalance(Token, GlobalSetting.Instance.Account.Address);
            }

            OnPropertyChanged(nameof(Token));
        }

        public ReceiveViewModel(Page context)
        {
            this._context = context;

            this._token = GlobalSetting.Instance.Ethereum;
            RequestString = GlobalSetting.Instance.Account.Address;

            _ethereumService = new EthereumService(new Web3(GlobalSetting.Instance.Account, GlobalSetting.Instance.EthereumNetwork));
            
            if (CrossConnectivity.Current.IsConnected)
            {
                OnRefreshBalance();
            }
        }

        private async Task OnRequestAmount()
        {
            var page = new EnterAmountPage();
            page.ViewModel.AmountChanged += obj => Amount = obj;

            await this._context.Navigation.PushAsync(page);
        }
    }
}