using Android.App;
using Android.Content.PM;
using Android.OS;

namespace Guap.Droid
{
    using Android;
    using Android.Support.V4.App;
    using Android.Support.V4.Content;

    using Plugin.CurrentActivity;
    using Plugin.Permissions;

    [Activity(Label = "Guap.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        //protected override void OnStart()
        //{
            
        //    if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != Permission.Granted)
        //    {
        //        ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage }, 0);
        //    }
        //    else
        //    {
        //        System.Diagnostics.Debug.WriteLine("Permission Granted!!!");
        //    }
        //    base.OnStart();
        //}

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            
            base.OnCreate(bundle);
          //  CrossCurrentActivity.Current.Activity = this;
            global::Xamarin.Forms.Forms.Init(this, bundle);
            
            App.ScreenHeight = (int) (Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);
            App.ScreenWidth = (int) (Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);
            
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}