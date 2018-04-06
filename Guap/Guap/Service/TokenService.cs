namespace Guap.Service
{
    using System;
    using System.Numerics;
    using System.Threading.Tasks;

    using Guap.Models;

    using Nethereum.Hex.HexTypes;
    using Nethereum.StandardTokenEIP20;
    using Nethereum.Web3;

    public class TokenService
    {
        private Web3 _web3;

        private string TokenInfoABI =
            @"[  {""constant"": true, ""inputs"": [], ""name"": ""name"", ""outputs"": [ {  ""name"": """", ""type"": ""string"" } ], ""payable"": false,  ""type"": ""function""  }, { ""constant"": true, ""inputs"": [], ""name"": ""decimals"",    ""outputs"": [      {        ""name"": """",        ""type"": ""uint8""      }    ],    ""payable"": false,    ""type"": ""function""  },  {    ""constant"": true,    ""inputs"": [      {        ""name"": ""_owner"",        ""type"": ""address""      }    ],    ""name"": ""balanceOf"",    ""outputs"": [      {        ""name"": ""balance"",        ""type"": ""uint256""      }    ],    ""payable"": false,    ""type"": ""function""  },  {    ""constant"": true,    ""inputs"": [],    ""name"": ""symbol"",    ""outputs"": [      {        ""name"": """",        ""type"": ""string""      }    ],    ""payable"": false,    ""type"": ""function""  }]";

        private string TokenStandartABI = @"[{""constant"":false,""inputs"":[{""name"":""spender"",""type"":""address""},{""name"":""value"",""type"":""uint256""}],""name"":""approve"",""outputs"":[{""name"":"""",""type"":""bool""}],""payable"":false,""type"":""function""},{""constant"":true,""inputs"":[],""name"":""totalSupply"",""outputs"":[{""name"":"""",""type"":""uint256""}],""payable"":false,""type"":""function""},{""constant"":false,""inputs"":[{""name"":""from"",""type"":""address""},{""name"":""to"",""type"":""address""},{""name"":""value"",""type"":""uint256""}],""name"":""transferFrom"",""outputs"":[{""name"":"""",""type"":""bool""}],""payable"":false,""type"":""function""},{""constant"":true,""inputs"":[{""name"":""who"",""type"":""address""}],""name"":""balanceOf"",""outputs"":[{""name"":"""",""type"":""uint256""}],""payable"":false,""type"":""function""},{""constant"":false,""inputs"":[{""name"":""to"",""type"":""address""},{""name"":""value"",""type"":""uint256""}],""name"":""transfer"",""outputs"":[{""name"":"""",""type"":""bool""}],""payable"":false,""type"":""function""},{""constant"":false,""inputs"":[{""name"":""spender"",""type"":""address""},{""name"":""value"",""type"":""uint256""},{""name"":""extraData"",""type"":""bytes""}],""name"":""approveAndCall"",""outputs"":[{""name"":"""",""type"":""bool""}],""payable"":false,""type"":""function""},{""constant"":true,""inputs"":[{""name"":""owner"",""type"":""address""},{""name"":""spender"",""type"":""address""}],""name"":""allowance"",""outputs"":[{""name"":"""",""type"":""uint256""}],""payable"":false,""type"":""function""},{""anonymous"":false,""inputs"":[{""indexed"":true,""name"":""owner"",""type"":""address""},{""indexed"":true,""name"":""spender"",""type"":""address""},{""indexed"":false,""name"":""value"",""type"":""uint256""}],""name"":""Approval"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":true,""name"":""from"",""type"":""address""},{""indexed"":true,""name"":""to"",""type"":""address""},{""indexed"":false,""name"":""value"",""type"":""uint256""}],""name"":""Transfer"",""type"":""event""}]";

        public TokenService(Web3 web3)
        {
            this._web3 = web3;
        }

        public async Task<Token> GetTokenInfo(string contactAddress)
        {
            var token = new Token() { Address = contactAddress };

            var contract = this._web3.Eth.GetContract(this.TokenInfoABI, contactAddress);
            token.Name =  contract.GetFunction("name").CallAsync<string>().Result;
            token.Symbol =  contract.GetFunction("symbol").CallAsync<string>().Result;
            token.Decimals =  contract.GetFunction("decimals").CallAsync<int>().Result;


            return token;
        }

        public async Task<BigInteger> GetBalance(Token token, string walletAddress)
        {
            var contract = this._web3.Eth.GetContract(this.TokenInfoABI, token.Address);
            var balance = contract.GetFunction("balanceOf").CallAsync<BigInteger>(walletAddress).Result;

            return balance;
        }
    }
}