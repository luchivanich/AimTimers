using System;
using AimTimers.Di;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Unity;

namespace AimTimers.Droid
{
    [Activity(Label = "AimTimers", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                TabLayoutResource = Resource.Layout.Tabbar;
                ToolbarResource = Resource.Layout.Toolbar;

                base.OnCreate(savedInstanceState);

                Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);

                AndroidEnvironment.UnhandledExceptionRaiser -= StoreLogger;
                AndroidEnvironment.UnhandledExceptionRaiser += StoreLogger;

                global::Xamarin.Forms.Forms.SetFlags("Shell_Experimental", "Visual_Experimental", "CollectionView_Experimental", "FastRenderers_Experimental");
                Xamarin.Essentials.Platform.Init(this, savedInstanceState);
                global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

                Couchbase.Lite.Support.Droid.Activate(this);

                var diContainer = new DiContainer();
                var unityContainer = diContainer.SetupIoc();
                var application = unityContainer.Resolve<Xamarin.Forms.Application>();

                LoadApplication(application);
            }
            catch (Exception e)
            {
                var i = 0;
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void StoreLogger(object sender, RaiseThrowableEventArgs e)
        {
            Console.WriteLine(e.Exception.StackTrace);
            var i = 1;
        }
    }
}