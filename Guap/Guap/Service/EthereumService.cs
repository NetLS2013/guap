using System.Threading.Tasks;

namespace Guap.Service
{
    using System;
    using System.Numerics;

    using NBitcoin;

    using Nethereum.HdWallet;
    using Nethereum.Hex.HexTypes;
    using Nethereum.RPC.Eth.DTOs;
    using Nethereum.Util;
    using Nethereum.Web3;
    using Nethereum.Web3.Accounts;

    public class EthereumService
    {
        private Web3 _web3;

        public EthereumService(Web3 web3)
        {
            this._web3 = web3;
        }

        public EthereumService()
        {
        }

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

        public static Account GetAccount(string words, string pass = "")
        {

            Wallet wallet = new Wallet(words, pass);

            return wallet.GetAccount(0);
        }

        public string GetAddress(string words, int id = 0, string seedPassword = "")
        {
            var wallet = new Wallet(words, seedPassword, GlobalSetting.Instance.WalletPath);
            var account =  wallet.GetAccount(id);
            
            return account.Address;
        }

        public async Task<string> SendEther(Account account, string toAddress, BigDecimal amount)
        {
            var realAmount = new HexBigInteger(UnitConversion.Convert.ToWei(amount));
            return await _web3.Eth.TransactionManager.SendTransactionAsync( account.Address, toAddress, realAmount);
        }

        public async Task<BigDecimal> GetBalance(string address)
        { 
            var balance =  _web3.Eth.GetBalance.SendRequestAsync(address).Result;
            return UnitConversion.Convert.FromWeiToBigDecimal(balance.Value, UnitConversion.EthUnit.Ether);
        }
    }
}