using Xamarin.Forms;

namespace Guap.Views
{
    using System;

    using DLToolkit.Forms.Controls;

    using Guap.Helpers;
    using Guap.ViewModels;

    public partial class MnemonicPhrasePage : ContentPage
    {
        public MnemonicPhraseViewModel viewModel { get; set; }

        public MnemonicPhrasePage(CommonPageSettings pageSettings)
        {
            InitializeComponent();

            // init top bar
            NavigationPage.SetHasNavigationBar(this, pageSettings.HasNavigation);
            NavigationPage.SetHasBackButton(this, pageSettings.HasBack);
            this.Title = pageSettings.Title;

            // init words table
            FlowListView.Init();

            viewModel = new MnemonicPhraseViewModel(this, pageSettings);
            BindingContext = viewModel;
        }
    }
}