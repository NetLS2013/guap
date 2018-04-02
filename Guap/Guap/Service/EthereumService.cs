namespace Guap.Service
{
    using NBitcoin;

    using Nethereum.HdWallet;
    using Nethereum.Web3.Accounts;

    public class EthereumService
    {
        public static string[] MnenonicPhrasegenerate()
        {
            Mnemonic mnemo = new Mnemonic(GlobalSetting.Instance.Wordlist, GlobalSetting.Instance.WordCount);

            return mnemo.Words;
        }
    }
}