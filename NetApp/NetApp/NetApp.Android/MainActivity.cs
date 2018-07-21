using System;

using Android.App;
using Android.Content.PM;
using Android.OS;
using NetApp.Droid.Jivosdk;
using Android.Content;


namespace NetApp.Droid
{
    [Activity(Label = "NetApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IJivoDelegate
    {
        public static JivoSdk jivoSdk;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());

            String lang = Java.Util.Locale.Default.ToString().IndexOf("ru") >= 0 ? "ru" : "en";

            App app = Xamarin.Forms.Application.Current as App;

            MainPage mainPage = app.MainPage as MainPage;

            HybridWebView webView = mainPage.InitWebView();

            jivoSdk = new JivoSdk(webView, lang)
            {
                JivoDelegate = this
            };
            jivoSdk.Prepare();
        }

        void IJivoDelegate.onEvent(string name, string data)
        {
            if (name.Equals("url.click"))
            {
                Intent browserIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(data));
                StartActivity(browserIntent);
            }

            if (name.Equals("chat.ready"))
            {
                jivoSdk.callApiMethod("sendMessage", "\"Это тестовое сообщение от разработчика, ответьте пожалуйста на него!\"");
            }

            if (name.Equals("agent.message"))
            {
                String str = data;
            }
        }
    }
}

