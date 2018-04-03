using Xamarin.Forms;

namespace Guap.Views
{
    using Guap.ViewModels;

    public partial class InputMnemonicPhrasePagePage : ContentPage
	{
		public InputMnemonicPhrasePagePage(MnemonicPhraseViewModel viewModel)
		{
			InitializeComponent ();
		    BindingContext = new InputMnemonicViewModel(this, viewModel);
		}
	}
}