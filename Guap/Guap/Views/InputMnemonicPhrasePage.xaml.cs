using Xamarin.Forms;

namespace Guap.Views
{
    using Guap.Helpers;
    using Guap.ViewModels;

    public partial class InputMnemonicPhrasePage : ContentPage
	{
	    public InputMnemonicViewModel ViewModel { get; set; }

		public InputMnemonicPhrasePage(CommonPageSettings pageSettings)
		{
			InitializeComponent();

		    // init top bar
		    NavigationPage.SetHasNavigationBar(this, pageSettings.HasNavigation);
		    NavigationPage.SetHasBackButton(this, pageSettings.HasBack);
		    this.Title = pageSettings.Title;

            ViewModel = new InputMnemonicViewModel(this, pageSettings);

		    BindingContext = ViewModel;

		}
	}
}