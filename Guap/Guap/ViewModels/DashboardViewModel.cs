using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Guap.Contracts;
using Guap.Models;
using Guap.Service;
using Guap.Views.Dashboard;
using Guap.Views.Modal;
using Nethereum.Web3;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Extensions;
using SQLite;
using Xamarin.Forms;

namespace Guap.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        public ActionSelectModalPage ActionSelectModalPage { get; set; }

        private Page _context;
        private bool _isRefreshing;
        private IRepository<Token> _repository;
        private TokenService _tokenService;
        private List<Token> _tokens;
        private Token _token;
        private Token _guap;
        private IMessage _message;

        public ICommand CreateTokenCommand => new Command( async () => await this._context.Navigation.PushAsync(new CreateTokenPage(this)));
        public ICommand SelectActionCommand => new Command( async () => Device.BeginInvokeOnMainThread(async () => await this._context.Navigation.PushPopupAsync(ActionSelectModalPage)));
        public ICommand RefreshTokensListCommand => new Command( async () => InitializeTokens());

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
                this._token = null;
                OnPropertyChanged(nameof(Token));
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

        public bool IsRefreshing
        {
            get
            {
                return this._isRefreshing;
            }
            set
            {
                _isRefreshing = value;

                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public DashboardViewModel(Page context)
        {
            _context = context;
            
            string databasePath = DependencyService.Get<ISQLite>().GetDatabasePath(GlobalSetting.Instance.DbName);
            
            _repository = new Repository<Token>(new SQLiteAsyncConnection(databasePath));
            _message = DependencyService.Get<IMessage>();
            _tokenService = new TokenService(new Web3(GlobalSetting.Instance.Account, GlobalSetting.Instance.EthereumNetwork));
            
            ActionSelectModalPage = new ActionSelectModalPage();

            Task.Run(() => { InitializeTokens(); }).Wait();
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
            IsRefreshing = true;

            var guap = GlobalSetting.Instance.Guap;
            var tokens = new List<Token>();
            
            tokens = await _repository.Get();

            if (!CrossConnectivity.Current.IsConnected)
            {
                Device.BeginInvokeOnMainThread(
                    () => _message.LongAlert("No internet connection! Cannot load tokens balances."));
            }
            else
            {
                guap.Balance = await this._tokenService.GetBalance(guap, GlobalSetting.Instance.Account.Address);

                foreach (var token in tokens)
                {
                    try
                    {
                        token.Balance = await _tokenService.GetBalance(token, GlobalSetting.Instance.Account.Address);
                    }
                    catch (Exception e)
                    {

                    }
                }
            }

            this.Guap = guap;
            Tokens = tokens;

            IsRefreshing = false;
        }
    }
}