namespace Guap.ViewModels
{
    using System;
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

        private string[] _words;

        private bool _isCustomHeader;

        public Action Action { get; set; }

        public string[] Words
        {
            get
            {
                return _words;
            }
            set
            {
                _words = value;
                OnPropertyChanged(nameof(Words));
            }
        }


        public ICommand NextCommand => new Command(async () => Action());

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
            this.HeaderText = pageSettings.HeaderText;
            this.IsCustomHeader = pageSettings.IsShowCustomHeader;

            _context = context;
        }

    }
}