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
using Guap.Views.Profile;
using MvvmValidation;
using Xamarin.Forms;

namespace Guap.ViewModels
{
    public class CreateAccountViewModel : BaseViewModel
    {
        private readonly Page _context;
        private readonly RequestProvider _requestProvider;

        private readonly EthereumService _ethereumService;
        private string _emailInput;
        private ValidationErrorCollection _errors;

        public ICommand CteateAccountCommand => new Command(async () => await OnCreateAccount());
        public ICommand RestoreWalletCommand => new Command(async () => await OpenRestoreWallet());
        public ICommand OpenPageCreateWalletCommand => new Command(async () => await OpenPageCreateWallet());

        public CreateAccountViewModel(Page context)
        {
            _context = context;
            
            _requestProvider = new RequestProvider();
            _ethereumService = new EthereumService();
        }
        
        public string EmailInput
        {
            get
            {
                return _emailInput;
            }
            set
            {
                if (_emailInput != value)
                {
                    _emailInput = value;
                    
                    OnPropertyChanged(nameof(EmailInput));
                }
            }
        }
        
        public ValidationErrorCollection Errors
        {
            get
            {
                return _errors;
            }
            set
            {
                _errors = value;
                
                OnPropertyChanged(nameof(Errors));
            }
        }
        
        private bool ValidateVerifyNumber(ValidationHelper validator)
        {
            var result = validator.ValidateAll();

            Errors = result.ErrorList;

            return result.IsValid;
        }

        private async Task OnCreateAccount()
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
                            () => App.SetMainPage(new BottomTabbedPage())));
                
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
                            () => App.SetMainPage(new BottomTabbedPage())));

                    // save mnenonic phrase 
                    Settings.Set(Settings.Key.MnemonicPhrase, s);
                    Settings.Set(Settings.Key.IsLogged, true);
                }
            };

            await _context.Navigation.PushAsync(inputMnemonic);
        }
        
        private async Task OpenPageCreateWallet()
        {
            var validator = new ValidationHelper();
            
            validator.AddRequiredRule(() => EmailInput, "The email is required.");
            
            if (!ValidateVerifyNumber(validator))
            {
                return;
            }

            try
            {
                var result = await _requestProvider
                    .PostAsync<UserModel, bool>(GlobalSetting.Instance.VerificationEmailEndpoint, 
                        new UserModel
                        {
                            Email = _emailInput,
                            VerificationCode = Settings.Get(Settings.Key.VerificationCode).ToString(),
                            PhoneNumber = Settings.Get(Settings.Key.PhoneNumber).ToString()
                        });
                
                if (result)
                {
                    await _context.Navigation.PushAsync(new NewUserExistPage());
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"--- Error: {e.Message}");
            }
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