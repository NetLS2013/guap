using Guap.ViewModels;

namespace Guap.Views.Profile
{
    public partial class ContactListPage
    {
        public ContactListPage()
        {
            InitializeComponent();

            BindingContext = new ContactListViewModel();
        }
    }
}