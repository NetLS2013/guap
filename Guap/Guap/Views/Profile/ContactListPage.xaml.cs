using Guap.ViewModels;

namespace Guap.Views.Profile
{
    public partial class ContactListPage
    {
        public ContactListViewModel ContactListViewModel { get; set; }

        public ContactListPage()
        {
            InitializeComponent();
            ContactListViewModel = new ContactListViewModel();
            BindingContext = ContactListViewModel;
        }
    }
}