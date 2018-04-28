using Android.App;
using Android.Support.V7.App;

namespace Guap.Droid
{
    [Activity(
        Label = "Guapcoin",
        Icon = "@drawable/icon",
        Theme = "@style/splashscreen",
        MainLauncher = true,
        NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnResume()
        {
            base.OnResume();
            
            StartActivity(typeof(MainActivity));
        }
    }
}