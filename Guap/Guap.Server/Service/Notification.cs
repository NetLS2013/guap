using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Guap.Server.Data.Repositories;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using System.Numerics;
using Guap.Server.Data.Entities;
using Guap.Server.Models;
using Guap.Server.Utils;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;

namespace Guap.Server.Service
{
    public interface INotification
    {
        Task Toggle(NotificationModel notification);
        Task WatchChain();
    }

    public class Notification : INotification
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailSender _emailSender;
        
        private Web3 _web3;
        private BigInteger _currentBlock;
        private BigInteger _lastBlock;
        private UnitConversion _unitConversion;
        private TokenSignature _tokenSignature;

        static ConcurrentDictionary<BigInteger, NotificationModel> Addresses { get; set; }
        
        public Notification(
            IUserRepository userRepository,
            IEmailSender emailSender)
        {
            _userRepository = userRepository;
            _emailSender = emailSender;
            
            _web3 = new Web3("https://ropsten.infura.io/E8XftGiqmaErL2KN5Cp3");
            _unitConversion = new UnitConversion();
            _tokenSignature = new TokenSignature();;
            
            Addresses = new ConcurrentDictionary<BigInteger, NotificationModel>();

            Task.Run(async () => await FillAddress());
        }

        private async Task FillAddress()
        {
            var users = _userRepository.GetAllUsers();
            
            foreach (var it in users)
            {
                if (!string.IsNullOrWhiteSpace(it.Address))
                {
                    Addresses.TryAdd(
                        new HexBigInteger(it.Address).Value, 
                        new NotificationModel
                        {
                            NotificationsEnabled = it.NotificationsEnabled,
                            Email = it.Email
                        });
                }
            }
        }

        public async Task WatchChain()
        {
            _lastBlock = _web3.Eth.Blocks.GetBlockNumber.SendRequestAsync().Result.Value - 1;
            
            while (true)
            {
                try
                {
                    _currentBlock = _web3.Eth.Blocks.GetBlockNumber.SendRequestAsync().Result.Value;
                    
                    if (_currentBlock != _lastBlock)
                    {
                        var difference = _currentBlock - _lastBlock;
                        var processBlock = _currentBlock - (difference - 1);

                        _lastBlock = processBlock;

                        await ProcessBlockNumber(processBlock);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"problem at block {_lastBlock} " + e.StackTrace);
                }
                
                Thread.Sleep(5000);
            }
        }

        private async Task ProcessBlockNumber(BigInteger newBlock)
        {
            var transactions = await _web3.Eth.Blocks.GetBlockWithTransactionsByNumber
                .SendRequestAsync(new HexBigInteger(newBlock));

            if (transactions != null)
            {
                foreach (var it in transactions.Transactions)
                {
                    if (string.IsNullOrEmpty(it.To))
                    {
                        continue;
                    }

                    string to;
                    BigInteger value;

                    if (TokenSignature.IsTransfer(it.Input))
                    {
                        to = TokenSignature.TransferTo(it.Input);
                        value = TokenSignature.TransferValue(it.Input);
                    }
                    else
                    {
                        to = it.To;
                        value = it.Value.Value;
                    }

                    if (Addresses.TryGetValue(new HexBigInteger(to).Value, out var notifData))
                    {
                        if (notifData.NotificationsEnabled)
                        {
                            await NotifyEyEmail(it, notifData, to, value);
                        }
                    }
                }
            }
        }

        private async Task NotifyEyEmail(Transaction transactions, NotificationModel user, string to, BigInteger value)
        {
            string tokenSymbol = "Ether";

            if (TokenSignature.IsTransfer(transactions.Input))
            {
                var contract = _web3.Eth.GetContract(TokenSignature.StandartABI, transactions.To);

                tokenSymbol = _tokenSignature.GetTokenSymbol(contract).Result;
            }

            await _emailSender.SendEmailAsync(user.Email, "Guapcoin Notification Service",
                "Guapcoin\n\n"
                + $"You received {_unitConversion.FromWei(value)} {tokenSymbol}, in your address {to}.\n\n"
                + "For detailed information please open this link: "
                + $"https://ropsten.etherscan.io/tx/{transactions.TransactionHash}\n\n");
        }
        
        public async Task Toggle(NotificationModel notification)
        {
            var hexAddress = new HexBigInteger(notification.Address).Value;
            
            Addresses.AddOrUpdate(hexAddress, notification, (address, data) => notification);
        }
    }
}