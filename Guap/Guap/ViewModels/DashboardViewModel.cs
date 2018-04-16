namespace Guap.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Guap.Contracts;
    using Guap.Helpers;
    using Guap.Models;
    using Guap.Service;
    using Guap.Views.Dashboard;
    using Guap.Views.Modal;

    using Nethereum.HdWallet;
    using Nethereum.Util;
    using Nethereum.Web3;
    using Nethereum.Web3.Accounts;

    using Rg.Plugins.Popup.Extensions;

    using SQLite;

    using Xamarin.Forms;

    public class DashboardViewModel : BaseViewModel
    {
        public ActionSelectModalPage ActionSelectModalPage { get; set; }

        private Page _context;

        private IRepository<Token> _repository;

        private TokenService _tokenService;

        private List<Token> _tokens;

        private Token _token;

        private Token _guap;

        private Account _account;

        public ICommand CreateTokenCommand => new Command( async () => await this._context.Navigation.PushAsync(new CreateTokenPage(this)));

        public ICommand SelectActionCommand => new Command( async () => Device.BeginInvokeOnMainThread(async () => await this._context.Navigation.PushPopupAsync(ActionSelectModalPage)));

        public Token Token
        {
            get
            {
                return _token;
            }
            set
            {
                _token = value;

                if (_token == null)
                    return;

                this._context.Navigation.PushAsync(new CreateTokenPage(this, this._token));
            }
        }

        public Token Guap
        {
            get
            {
                return _guap;
            }
            set
            {
                _guap = value;

                OnPropertyChanged(nameof(Guap));
            }
        }

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

        public DashboardViewModel(Page context)
        {
            ActionSelectModalPage = new ActionSelectModalPage();

            _context = context;
            _account = EthereumService.GetAccount((string)Settings.Get(Settings.Key.MnemonicPhrase));

            string databasePath = DependencyService.Get<ISQLite>().GetDatabasePath(GlobalSetting.Instance.DbName);
            _repository = new Repository<Token>(new SQLiteAsyncConnection(databasePath));

            _tokenService = new TokenService(new Web3(_account, GlobalSetting.Instance.EthereumNetwork));
            Task.Run(async () => { InitializeTokens(); }).Wait();
        }

        public List<Token> Tokens
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

        public async void InitializeTokens()
        {
            _guap = GlobalSetting.Instance.Guap;
            _guap.Balance = this._tokenService.GetBalance(_guap, _account.Address).Result;

            var tokens = new List<Token>();
            tokens = await _repository.Get();

            foreach (var token in tokens)
            {
                try
                {
                    token.Balance = await _tokenService.GetBalance(token, _account.Address);
                }
                catch (Exception e)
                {

                }
            }

            Tokens = tokens;
        }

    }
}