namespace Guap
{
    using NBitcoin;

    public class GlobalSetting
    {
        private class Nested
        {
            static Nested()
            {

            }

            internal static readonly GlobalSetting Instance = new GlobalSetting();
        }

        private GlobalSetting()
        {
            WordCount = WordCount.Twelve;
            Wordlist = Wordlist.English;
            EthereumNetwork = "https://ropsten.infura.io/E8XftGiqmaErL2KN5Cp3";
            WalletPath = "m/44'/60'/0'/0/x";
        }

        public static GlobalSetting Instance => Nested.Instance;

        public WordCount WordCount { get; }

        public Wordlist Wordlist { get; }

        public string EthereumNetwork { get; }

        public string WalletPath { get; }
    }
}