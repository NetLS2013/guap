using Xamarin.Forms;

namespace Guap.Views
{
    using DLToolkit.Forms.Controls;

    using Guap.Helpers;
    using Guap.ViewModels;

    public partial class MnemonicPhrasePage : ContentPage
    {
        public MnemonicPhrasePage(CommonPageSettings pageSettings)
        {
            NavigationPage.SetHasNavigationBar(this, pageSettings.HasNavigation);
            NavigationPage.SetHasBackButton(this, pageSettings.HasBack);
            this.Title = pageSettings.Title;

            InitializeComponent();
            FlowListView.Init();
            BindingContext = new MnemonicPhraseViewModel(this, pageSettings);
        }
    }
}