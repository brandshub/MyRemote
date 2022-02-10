using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using MyRemote.Lib.Server;
using MyRemote.AndroidClient.Business;
using MyRemote.Lib.Action;
using Android.Provider;

namespace MyRemote.AndroidClient.Droid
{
    [Activity(Label = "MyRemote.AndroidClient", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    [IntentFilter(new[] { Android.Content.Intent.ActionSend }, Categories = new[] { Android.Content.Intent.CategoryDefault }, DataMimeTypes = new[] { "text/plain", "image/*" })]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            Forms.SetFlags("SwipeView_Experimental");
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());



            if (Intent.Action.Equals(Android.Content.Intent.ActionSend) &&
                Intent.Type != null)
            {
                if (Intent.Type.Equals("text/plain"))

                {
                    handleSendUrl();
                }
                else if (Intent.Type.StartsWith("image") && Intent.Extras.ContainsKey(Android.Content.Intent.ExtraStream))
                {
                    var fileUrl = GetFilePath((Android.Net.Uri)Intent.Extras.GetParcelable(Android.Content.Intent.ExtraStream));

                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private async void handleSendUrl()
        {
            if (Globals.CurrentConfig == null)
                await Globals.TryLoadDefaultData();

            if (Globals.CurrentConfig != null && Globals.CurrentConfig.Server != null)
            {
                var url = Intent.GetStringExtra(Android.Content.Intent.ExtraText);

                var view = new LinearLayout(this) { Orientation = Orientation.Vertical };

                var urlTextView = new TextView(this) { Gravity = GravityFlags.Center };
                urlTextView.Text = url;

                view.AddView(urlTextView);
                if (url.StartsWith("http://") || url.StartsWith("https://"))
                {
                    new AlertDialog.Builder(this)
                             .SetTitle("Open Url")
                             .SetView(view)
                             .SetPositiveButton("Open", (dialog, whichButton) =>
                             {
                                 //Save off the url and description here
                                 //Remove dialog and navigate back to app or browser that shared                 
                                 //the link

                                 Server.SendRequest(Globals.CurrentConfig, RunProcessAction.GenericRequest(url));
                                 FinishAndRemoveTask();
                                 FinishAffinity();
                             })
                             .SetNegativeButton("Cancel", (dialog, whichButton) =>
                             {
                                 FinishAndRemoveTask();
                                 FinishAffinity();
                             })
                             .Show();
                }
                else
                {
                    new AlertDialog.Builder(this).SetTitle("Not a valid URL").SetView(view).SetNegativeButton("Cancel", (dialog, whichButton) =>
                    {
                        FinishAndRemoveTask();
                        FinishAffinity();
                    }).Show();
                }
            }
        }


        private string GetFilePath(Android.Net.Uri uri)
        {
            string[] proj = { MediaStore.Images.ImageColumns.Data };
            var cursor = ContentResolver.Query(uri, proj, null, null, null);
            var colIndex = cursor.GetColumnIndex(MediaStore.Images.ImageColumns.Data);
            cursor.MoveToFirst();
            return cursor.GetString(colIndex);
        }

    }
}