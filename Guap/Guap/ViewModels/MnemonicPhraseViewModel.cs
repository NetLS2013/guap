namespace Guap.ViewModels
{
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Guap.Models;
    using Guap.Service;
    using Guap.Views;

    using MvvmValidation;

    using Xamarin.Forms;

    public class MnemonicPhraseViewModel : BaseViewModel
    {
        private readonly Page _context;

        public MnemonicWallet Wallet { get; private set; }

        public ICommand NextCommand => new Command(async () => await OnContinue());

        public MnemonicPhraseViewModel(Page context)
        {
            Wallet = new MnemonicWallet();
            Wallet.Words = EthereumService.MnenonicPhrasegenerate();

            _context = context;
        }

        private async Task OnContinue()
        {
            await _context.Navigation.PushAsync(new InputMnemonicPhrasePagePage(this));
        }
    }
}