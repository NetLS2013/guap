using Guap.Droid.Service;

[assembly: Xamarin.Forms.Dependency(typeof(MessageAndroid))]
namespace Guap.Droid.Service
{
    using Android.App;
    using Android.Widget;

    using Guap.Contracts;

    public class MessageAndroid : IMessage
    {
        public void LongAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
    }
}