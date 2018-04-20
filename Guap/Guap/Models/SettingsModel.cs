namespace Guap.Models
{
    using System.Windows.Input;

    public class SettingsModel
    {
        public delegate void MethodInvoke();
        
        public string Title { get; set; }

        public MethodInvoke Method { get; set; }

        public string Icon { get; set; }

        public bool IsVisible { get; set; }

        public bool Toggled { get; set; }

        public ICommand ToggledCommand { get; set; }
    }
}