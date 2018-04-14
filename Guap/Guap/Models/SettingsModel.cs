namespace Guap.Models
{
    public class SettingsModel
    {
        public delegate void MethodInvoke();
        
        public string Title { get; set; }

        public MethodInvoke Method { get; set; }
    }
}