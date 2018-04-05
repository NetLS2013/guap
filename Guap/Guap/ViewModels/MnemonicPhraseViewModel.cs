namespace Guap.ViewModels
{
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Guap.Helpers;
    using Guap.Models;
    using Guap.Service;
    using Guap.Views;

    using MvvmValidation;

    using Xamarin.Forms;

    public class MnemonicPhraseViewModel : BaseViewModel
    {
        private readonly Page _context;

        private string _headerText;

        private bool _isCustomHeader;

        public MnemonicWallet Wallet { get; private set; }

        public ICommand NextCommand => new Command(async () => await OnContinue());

        public string HeaderText
        {
            get
            {
                return _headerText;
            }
            set
            {
                _headerText = value;
                OnPropertyChanged(nameof(HeaderText));
            }
        }

        public bool IsCustomHeader
        {
            get
            {
                return _isCustomHeader;
            }
            set
            {
                _isCustomHeader = value;
                OnPropertyChanged(nameof(IsCustomHeader));
            }
        }

        public MnemonicPhraseViewModel(Page context, CommonPageSettings pageSettings)
        {
            Wallet = new MnemonicWallet();
            Wallet.Words = EthereumService.MnenonicPhrasegenerate();
            this.HeaderText = pageSettings.HeaderText;
            this.IsCustomHeader = pageSettings.IsShowCustomHeader;

            _context = context;
        }

        private async Task OnContinue()
        {
            await _context.Navigation.PushAsync(new InputMnemonicPhrasePagePage(this));
        }
    }
}