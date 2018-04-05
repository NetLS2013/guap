namespace Guap.Service
{
    using System;

    using NBitcoin;

    using Nethereum.HdWallet;
    using Nethereum.Web3.Accounts;

    public class EthereumService
    {
        public static string[] MnenonicPhraseGenerate()
        {
            Mnemonic mnemo = new Mnemonic(GlobalSetting.Instance.Wordlist, GlobalSetting.Instance.WordCount);
           
            return mnemo.Words;
        }

        public static bool MnenonicPhraseValidate(string words)
        {
            try
            {
                Wallet wallet = new Wallet(words, "", GlobalSetting.Instance.WalletPath);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }
    }
}