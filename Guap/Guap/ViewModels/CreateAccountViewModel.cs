using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Guap.Helpers;
using Guap.Models;
using Guap.Service;
using Guap.Views;
using Guap.Views.Dashboard;
using Xamarin.Forms;

namespace Guap.ViewModels
{
    public class CreateAccountViewModel
    {
        private readonly Page _context;
        private readonly RequestProvider _requestProvider;

        private EthereumService _ethereumService;

        public ICommand CteateAccountCommand => new Command(async () => await OnCreateContact());
        public ICommand RestoreWalletCommand => new Command(async () => await OpenRestoreWallet());

        public CreateAccountViewModel(Page context)
        {
            _context = context;
            
            _requestProvider = new RequestProvider();
            _ethereumService = new EthereumService();
        }

        private async Task OnCreateContact()
        {
            var words = EthereumService.MnenonicPhraseGenerate();

            var inputMnemonic = new InputMnemonicPhrasePage(
                new CommonPageSettings
                {
                    HasNavigation = false,
                    HeaderText = "Mnemonic Phrase"
                });
        
            inputMnemonic.ViewModel.Validators.Add(
                new KeyValuePair<string, Func<string, bool>>(
                    "The mnemonic phrase you entered is incorrect." + Environment.NewLine + "Typos can cause this."
                    + Environment.NewLine + "Please review your phrase and try again",
                    s => string.Join(" ", words) == s));


            inputMnemonic.ViewModel.SuccessAction = async s =>
            {
                var result = await UpdateAddress(string.Join(" ", words));

                if (result)
                {
                    await _context.Navigation.PushAsync(
                        new SuccessSignup(
                            new CommonPageSettings
                            {
                                HasNavigation = false,
                                HeaderText =
                                    "The mnenonic phrase was an exatact match."
                                    + Environment.NewLine + "Your wallet has been created."
                                    + Environment.NewLine + "Check out the dashboard."
                            },
                            () => App.SetMainPage(new Dashboard())));
                
                    // save mnenonic phrase 
                    Settings.Set(Settings.Key.MnemonicPhrase, s);
                    Settings.Set(Settings.Key.IsLogged, true);

                }
            };

            var mnemonicPage = new MnemonicPhrasePage(
                new CommonPageSettings
                {
                    HasNavigation = false,
                    HeaderText = "Mnemonic Phrase"
                });
        
            mnemonicPage.viewModel.Action = () => _context.Navigation.PushAsync(inputMnemonic);
            mnemonicPage.viewModel.Words = words;

            await _context.Navigation.PushAsync(mnemonicPage);
        }

        private async Task OpenRestoreWallet()
        {
            var inputMnemonic = new InputMnemonicPhrasePage(
                new CommonPageSettings
                    { 
                        HasNavigation = false,
                        HeaderText = "Mnemonic Phrase"
                    });

            inputMnemonic.ViewModel.Validators.Add(
                new KeyValuePair<string, Func<string, bool>>(
                    "The mnemonic phrase you entered is incorrect." + Environment.NewLine + "Typos can cause this."
                    + Environment.NewLine + "Please review your phrase and try again",
                    s => EthereumService.MnenonicPhraseValidate(s)));

            inputMnemonic.ViewModel.SuccessAction = async s =>
            {
                var result = await UpdateAddress(s);

                if (result)
                {
                    await _context.Navigation.PushAsync(
                        new SuccessSignup(
                            new CommonPageSettings
                            {
                                HasNavigation = false,
                                HeaderText =
                                    "The mnenonic phrase was an exatact match."
                                    + Environment.NewLine + "Your wallet has been created."
                                    + Environment.NewLine + "Check out the dashboard."
                            },
                            () => App.SetMainPage(new Dashboard())));

                    // save mnenonic phrase 
                    Settings.Set(Settings.Key.MnemonicPhrase, s);
                    Settings.Set(Settings.Key.IsLogged, true);
                }
            };

            await _context.Navigation.PushAsync(inputMnemonic);
        }

        private async Task<bool> UpdateAddress(string words)
        {
            var address = _ethereumService.GetAddress(words);
            var result = false;
            
            try
            {
                result = await _requestProvider
                    .PostAsync<UserModel, bool>(GlobalSetting.Instance.UpdateAddressEndpoint,
                        new UserModel
                        {
                            Address = address,
                            PhoneNumber = (string) Settings.Get(Settings.Key.PhoneNumber)
                        });
            }
            catch (Exception e)
            {
                Debug.WriteLine($"--- Error: {e.Message}");
            }
            
            return result;
        }
    }
}