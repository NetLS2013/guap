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

    using Nethereum.HdWallet;
    using Nethereum.Util;
    using Nethereum.Web3;

    using SQLite;

    using Xamarin.Forms;

    public class DashboardViewModel : BaseViewModel
    {
        private Page _context;

        private IRepository<Token> _repository;

        private TokenService _tokenService;

        private List<Token> _tokens;

        private Token _token;

        public ICommand CreateTokenCommand => new Command( async () => await this._context.Navigation.PushAsync(new CreateTokenPage(this)));

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

        public DashboardViewModel(Page context)
        {
            this._context = context;

            string databasePath = DependencyService.Get<ISQLite>().GetDatabasePath(GlobalSetting.Instance.DbName);
            _repository = new Repository<Token>(new SQLiteAsyncConnection(databasePath));

            _tokenService = new TokenService(new Web3(new Wallet((string)Settings.Get(Settings.Key.MnemonicPhrase), "").GetAccount(0), GlobalSetting.Instance.EthereumNetwork));
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
            var tokens = new List<Token>();
            try
            {
                tokens = await _repository.Get();
                foreach (var token in tokens)
                {
                    token.Balance = await _tokenService.GetBalance(token, "0xED4e06BE10d494E753a2AC3F446D1070d048B442");
                }
            }
            catch (Exception e)
            {
                
            }
           
            Tokens = tokens;
        }

    }
}