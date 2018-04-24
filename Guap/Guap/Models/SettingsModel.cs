namespace Guap.Models
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;

    using Guap.Annotations;
    using Guap.ViewModels;

    public class SettingsModel : BaseViewModel
    {
        private bool _toggled;

        public delegate void MethodInvoke();
        
        public string Title { get; set; }

        public MethodInvoke Method { get; set; }

        public string Icon { get; set; }

        public bool IsVisible { get; set; }

        public bool Toggled
        {
            get
            {
                return _toggled;
            }
            set
            {
                _toggled = value;
                OnPropertyChanged(nameof(Toggled));
            }
        }

        public ICommand ToggledCommand { get; set; }

    }
}