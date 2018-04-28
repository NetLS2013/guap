namespace Guap.Service
{
    using System;
    using System.Numerics;
    using System.Threading.Tasks;

    using Guap.Contracts;
    using Guap.Models;

    using Nethereum.Hex.HexConvertors.Extensions;
    using Nethereum.Hex.HexTypes;
    using Nethereum.Util;
    using Nethereum.Web3;
    using Nethereum.Web3.Accounts;

    using Plugin.Connectivity;

    using Xamarin.Forms;

    public class TokenService
    {
        private Web3 _web3;

        private IMessage _message;

        private string TokenStandartABI = @"[{""constant"":true,""inputs"":[],""name"":""name"",""outputs"":[{""name"":"""",""type"":""string""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":false,""inputs"":[{""name"":""_spender"",""type"":""address""},{""name"":""_amount"",""type"":""uint256""}],""name"":""approve"",""outputs"":[{""name"":""success"",""type"":""bool""}],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""totalSupply"",""outputs"":[{""name"":""totalSupply"",""type"":""uint256""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":false,""inputs"":[{""name"":""_from"",""type"":""address""},{""name"":""_to"",""type"":""address""},{""name"":""_amount"",""type"":""uint256""}],""name"":""transferFrom"",""outputs"":[{""name"":""success"",""type"":""bool""}],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""decimals"",""outputs"":[{""name"":"""",""type"":""uint8""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[{""name"":""_owner"",""type"":""address""}],""name"":""balanceOf"",""outputs"":[{""name"":""balance"",""type"":""uint256""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""owner"",""outputs"":[{""name"":"""",""type"":""address""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""symbol"",""outputs"":[{""name"":"""",""type"":""string""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":false,""inputs"":[{""name"":""_to"",""type"":""address""},{""name"":""_amount"",""type"":""uint256""}],""name"":""transfer"",""outputs"":[{""name"":""success"",""type"":""bool""}],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":true,""inputs"":[{""name"":""_owner"",""type"":""address""},{""name"":""_spender"",""type"":""address""}],""name"":""allowance"",""outputs"":[{""name"":""remaining"",""type"":""uint256""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""inputs"":[],""payable"":false,""stateMutability"":""nonpayable"",""type"":""constructor""},{""anonymous"":false,""inputs"":[{""indexed"":true,""name"":""_from"",""type"":""address""},{""indexed"":true,""name"":""_to"",""type"":""address""},{""indexed"":false,""name"":""_value"",""type"":""uint256""}],""name"":""Transfer"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":true,""name"":""_owner"",""type"":""address""},{""indexed"":true,""name"":""_spender"",""type"":""address""},{""indexed"":false,""name"":""_value"",""type"":""uint256""}],""name"":""Approval"",""type"":""event""}]";

        public TokenService(Web3 web3)
        {
            _web3 = web3;
            _message = DependencyService.Get<IMessage>();
        }

        public static BigDecimal ConvertToBigDecimal(BigInteger integer, int decimals)
        {
            return new BigDecimal(integer, decimals) / new BigDecimal(BigInteger.Pow(new BigInteger(10), decimals), decimals);
        }

        public static BigInteger ConvertToBigInteger(BigDecimal number, int decimals)
        {
            return BigInteger.Parse((number * new BigDecimal(BigInteger.Pow(new BigInteger(10), decimals), 0)).ToString());
        }

        public static string Convert64Hex(HexBigInteger number)
        {
            return number.HexValue.RemoveHexPrefix().PadLeft(64, '0');
        }

        public async Task<Token> GetTokenInfo(string contactAddress)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                Device.BeginInvokeOnMainThread(() => _message.ShortAlert("No internet connection! Cannot fetch data from blockchain."));
                
                return null;
            }

            var token = new Token() { Address = contactAddress };

            try
            {
                var contract = this._web3.Eth.GetContract(this.TokenStandartABI, contactAddress);
                token.Name = await contract.GetFunction("name").CallAsync<string>();
                token.Symbol = await contract.GetFunction("symbol").CallAsync<string>();
                token.Decimals = await contract.GetFunction("decimals").CallAsync<int>();

                return token;
            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread(() => _message.ShortAlert("Smart contract doesn't contain fields \"Name\", \"Symbol\" or \"Decimals\""));
                return null;
            }
        }

        public async Task<BigDecimal> GetBalance(Token token, string walletAddress)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                Device.BeginInvokeOnMainThread(() => _message.ShortAlert("No internet connection!Cannot get current balance from blockchain."));
                return new BigDecimal(0);
            }

            try
            {
                var contract = this._web3.Eth.GetContract(this.TokenStandartABI, token.Address);
                var balanceContract = await contract.GetFunction("balanceOf").CallAsync<BigInteger>(walletAddress);

                var balance = ConvertToBigDecimal(balanceContract, token.Decimals);

                return balance;
            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread(() => _message.ShortAlert("Smart contract doesn't contain method \"balanceOf\"."));
                return new BigDecimal(0);
            }
            
        }

        public async Task<string> Send(Token token, Account account, string toAddress, BigDecimal amount)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                Device.BeginInvokeOnMainThread(() => _message.ShortAlert("No internet connection! Cannot sent token."));
                return null;
            }

            try
            {
                var realAmount = ConvertToBigInteger(amount, token.Decimals);

                var contract = _web3.Eth.GetContract(this.TokenStandartABI, token.Address);
                var gas = await contract.GetFunction("transfer").EstimateGasAsync(account.Address, null, null, toAddress, realAmount);
                return await contract.GetFunction("transfer").SendTransactionAsync(account.Address, gas, null, null, toAddress, realAmount);
            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread(() => _message.ShortAlert("Smart contract doesn't contain method \"transfer\"."));
                return null;
            }
           
        }
    }
}