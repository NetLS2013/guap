using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Guap.Contracts;
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
    using Guap.Contracts;

    using SQLite;

    public class CreateAccountViewModel : BaseViewModel
    {
        private readonly Page _context;
        private readonly RequestProvider _requestProvider;

        private readonly EthereumService _ethereumService;
        private IRepository<Token> _repository;
        private string _emailInput;
        private ValidationErrorCollection _errors;
        private IMessage _message;

        public ICommand CteateAccountCommand => new Command(async () => await OnCreateAccount());
        public ICommand RestoreWalletCommand => new Command(async () => await OpenRestoreWallet());
        public ICommand OpenPageCreateWalletCommand => new Command(async () => await OpenPageCreateWallet());
        public ICommand ForgotPinCommand => new Command(async () => await ForgotPin());

        public CreateAccountViewModel(Page context)
        {
            _context = context;
            
            _requestProvider = new RequestProvider();
            _ethereumService = new EthereumService();

            string databasePath = DependencyService.Get<ISQLite>().GetDatabasePath(GlobalSetting.Instance.DbName);
            
            _repository = new Repository<Token>(new SQLiteAsyncConnection(databasePath));
            _message = DependencyService.Get<IMessage>();
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
        
        private bool Validate(ValidationHelper validator)
        {
            var result = validator.ValidateAll();

            Errors = result.ErrorList;

            return result.IsValid;
        }

        private async Task OnCreateAccount()
        {
            var words = EthereumService.MnenonicPhraseGenerate();
            var inputMnemonic = await BuildInputMnemonicPhrasePage(
                "Enter your mnenonic phrase below to create your wallet.",
                "The mnenonic phrase was an exatact match.\nYour wallet has been created.\nCheck out the dashboard.",
                s => string.Join(" ", words) == s);

            var mnemonicPage = new MnemonicPhrasePage(
                new CommonPageSettings
                {
                    HasNavigation = false,
                    HeaderText = "Mnemonic Phrase"
                });
        
            mnemonicPage.viewModel.Action = async () => await _context.Navigation.PushSingleAsync(inputMnemonic);
            mnemonicPage.viewModel.Words = words;

            await _context.Navigation.PushSingleAsync(mnemonicPage);
        }

        private async Task OpenRestoreWallet()
        {
            var inputMnemonic = await BuildInputMnemonicPhrasePage(
                "Enter your mnenonic phrase below to restore your wallet.",
                "The mnenonic phrase was an exatact match.\nYour wallet has been restored.\nCheck out the dashboard.",
                EthereumService.MnenonicPhraseValidate);
            
            await _context.Navigation.PushSingleAsync(inputMnemonic);
        }

        private async Task<Page> BuildInputMnemonicPhrasePage(string commonText1, string commonText2, Func<string, bool> valid)
        {
            var inputMnemonic = new InputMnemonicPhrasePage(
                new CommonPageSettings
                    {
                        HasNavigation = false,
                        HeaderText = "Mnemonic Phrase",
                        Text = commonText1,
                        LeftButtonText = "Back"
                    });


            inputMnemonic.ViewModel.Validators.Add(
                new KeyValuePair<string, Func<string, bool>>(
                    "The mnemonic phrase you entered is incorrect." + Environment.NewLine + "Typos can cause this."
                    + Environment.NewLine + "Please review your phrase and try again.",
                    valid));

            inputMnemonic.ViewModel.SuccessAction = async s =>
            {
                var result = await UpdateAddress(s);

                if (result)
                {
                    Settings.Set(Settings.Key.MnemonicPhrase, s);
                    Settings.Set(Settings.Key.IsLogged, true);
                    
                    await _context.Navigation.PushSingleAsync(
                        new SuccessSignup(
                            new CommonPageSettings
                            {
                                HasNavigation = false,
                                HeaderText = commonText2
                            },
                            async () =>
                                {
                                    var tokens = await _repository.Get();
                                    foreach (var token in tokens)
                                    {
                                        await _repository.Delete(token);
                                    }
                                    App.SetMainPage(new BottomTabbedPage());
                                }));
                }
            };
            
            return inputMnemonic;
        }

        private async Task ForgotPin()
        {
            var validator = new ValidationHelper();
            
            validator.AddRequiredRule(() => EmailInput, "The email is required.");
            
            if (!Validate(validator))
            {
                return;
            }

            try
            {
                var result = await _requestProvider
                    .PostAsync<UserModel, bool>(GlobalSetting.Instance.ForgotPinEndpoint, 
                        new UserModel
                        {
                            Email = _emailInput,
                            Pin = (string) Settings.Get(Settings.Key.Pin)
                        });
                
                if (result)
                {
                    await _context.Navigation.PopAsync();
                    
                    _message.LongAlert("PIN has been sent to your email address.");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"--- Error: {e.StackTrace}");
            }
        }
        
        private async Task OpenPageCreateWallet()
        {
            var validator = new ValidationHelper();
            
            validator.AddRequiredRule(() => EmailInput, "The email is required.");
            
            if (!Validate(validator))
            {
                return;
            }

            try
            {
                var result = await _requestProvider
                    .PostAsync<UserModel, ResultModel>(GlobalSetting.Instance.VerificationEmailEndpoint, 
                        new UserModel
                        {
                            Email = _emailInput,
                            VerificationCode = Settings.Get(Settings.Key.VerificationCode).ToString(),
                            PhoneNumber = Settings.Get(Settings.Key.PhoneNumber).ToString()
                        });
                
                validator.AddRule(EmailInput,
                    () => RuleResult.Assert(result.Result, result.Message));
                
                if (!Validate(validator))
                {
                    return;
                }
                
                if (result.Result)
                {
                    await _context.Navigation.PushSingleAsync(new NewUserExistPage());
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"--- Error: {e.StackTrace}");
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
                            PhoneNumber = (string) Settings.Get(Settings.Key.PhoneNumber),
                            VerificationCode = (string) Settings.Get(Settings.Key.VerificationCode)
                        });
            }
            catch (Exception e)
            {
                Debug.WriteLine($"--- Error: {e.StackTrace}");
            }
            
            return result;
        }
    }
}