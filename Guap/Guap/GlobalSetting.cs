using NBitcoin;

namespace Guap
{
    using Guap.Models;

    public class GlobalSetting
    {
        private class Nested
        {
            static Nested() { }

            internal static readonly GlobalSetting Instance = new GlobalSetting();
        }

        private GlobalSetting()
        {
            WordCount = WordCount.Twelve;
            Wordlist = Wordlist.English;
            EthereumNetwork = "https://ropsten.infura.io/E8XftGiqmaErL2KN5Cp3";
            WalletPath = "m/44'/60'/0'/0/x";
            BaseEndpoint = "http://f51308c9.ngrok.io";
            DbName = "guap.db";
            BlockExplorer = "https://ropsten.etherscan.io";

            Guap = new Token()
                       {
                           Address = "0x9B333Edb02150abC217B746921499650dd3e448E",
                           Decimals = 18,
                           Id = 0,
                           Name = "Guap",
                           Symbol = "Guap"

            };

            Ethereum = new Token()
                           {
                               Name = "Ethereum",
                               Symbol = "ETH",
                               Id = -1,
                           };
        }

        public static GlobalSetting Instance => Nested.Instance;
        

        public WordCount WordCount { get; }

        public Wordlist Wordlist { get; }

        public string EthereumNetwork { get; }

        public string WalletPath { get; }
        
        public string DbName { get; }

        public Token Guap { get; set; }
        public Token Ethereum { get; set; }


        public string RegisterNumberEndpoint { get; set; }
        
        public string VerificationCodeEndpoint { get; set; }

        public string UpdateAddressEndpoint { get; set; }
        
        public string GetAddressByNumberEndpoint { get; set; }
        
        private string BaseEndpoint
        {
            set => UpdateEndpoint(value);
        }

        public string VerificationEmailEndpoint { get; set; }

        private void UpdateEndpoint(string baseEndpoint)
        {
            RegisterNumberEndpoint = $"{baseEndpoint}/api/Account/RegisterNumber";
            VerificationCodeEndpoint = $"{baseEndpoint}/api/Account/VerificationCode";
            UpdateAddressEndpoint = $"{baseEndpoint}/api/Wallet/UpdateAddress";
            GetAddressByNumberEndpoint = $"{baseEndpoint}/api/Wallet/GetAddressByNumber";
            VerificationEmailEndpoint = $"{baseEndpoint}/api/Account/VerificationEmail";
        }

        private string BlockExplorer
        {
            set
            {
                UpdateBlockExplorerEndpoint(value);
            }
        }

        public string ExplorerTransactionEndpoint { get; set; }

        private void UpdateBlockExplorerEndpoint(string baseEndpoint)
        {
            ExplorerTransactionEndpoint = $"{baseEndpoint}/tx/";
        }
    }
}