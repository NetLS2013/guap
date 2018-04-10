using NBitcoin;

namespace Guap
{
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
            BaseEndpoint = DefaultEndpoint;
            DbName = "guap.db";
        }

        public static GlobalSetting Instance => Nested.Instance;
        

        public WordCount WordCount { get; }

        public Wordlist Wordlist { get; }

        public string EthereumNetwork { get; }

        public string WalletPath { get; }
        
        public string DbName { get; }

        
        public string RegisterNumberEndpoint { get; set; }
        
        public string VerificationCodeEndpoint { get; set; }

        public string UpdateAddressEndpoint { get; set; }
        
        public string GetAddressByNumberEndpoint { get; set; }
        
        
        private const string DefaultEndpoint = "http://127.0.0.1:56057";
        
        private string BaseEndpoint
        {
            set => UpdateEndpoint(value);
        }
        
        private void UpdateEndpoint(string baseEndpoint)
        {
            RegisterNumberEndpoint = $"{baseEndpoint}/api/Account/RegisterNumber";
            VerificationCodeEndpoint = $"{baseEndpoint}/api/Account/VerificationCode";
            UpdateAddressEndpoint = $"{baseEndpoint}/api/Wallet/UpdateAddress";
            GetAddressByNumberEndpoint = $"{baseEndpoint}/api/Wallet/GetAddressByNumber";
        }
    }
}