using Xamarin.Forms;

namespace Guap.Helpers
{
    public static class Settings
    {
        public static object Get(Key key)
        {
            Application.Current.Properties.TryGetValue(key.ToString(), out object value);
                
            return value;
        }
        
        public static async void Set(Key key, object value)
        {
            Application.Current.Properties[key.ToString()] = value;
            
            await Application.Current.SavePropertiesAsync();
        }

        public enum Key
        {
            ResumePage,
            PinSetupPage,
            Pin,
            MnemonicPhrase,
            PhoneNumber,
            IsLogged
        }
    }
}