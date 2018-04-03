using Xamarin.Forms;

namespace Guap.Views
{
    using DLToolkit.Forms.Controls;

    using Guap.ViewModels;

    public partial class MnemonicPhrasePage : ContentPage
    {
        public MnemonicPhrasePage()
        {
            InitializeComponent();
            FlowListView.Init();
            BindingContext = new MnemonicPhraseViewModel(this);
        }
    }
}