using Android.Content;
using Xamarin.Forms;
using NetApp;
using Xamarin.Forms.Platform.Android;
using NetApp.Droid;
using NetApp.Droid.Jivosdk;
using Android.Util;
using Android.App;
using Android.Views;
using Android.Runtime;
using Android.Webkit;

using System.ComponentModel;


[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]
namespace NetApp.Droid
{
    public class HybridWebViewRenderer : ViewRenderer<HybridWebView, Android.Webkit.WebView>
    {
        const string JavaScriptFunction = "function invokeCSharpAction(data){jsBridge.invokeAction(data);}";
        private readonly Context _context;

        private IJivoDelegate JivoDelegate;
        public ProgressDialog progr;

        public static Android.Webkit.WebView StaticWebView;

        public HybridWebViewRenderer(Context context) : base(context)
        {
            _context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<HybridWebView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                var webView = new Android.Webkit.WebView(_context);
                StaticWebView = webView;

                SetNativeControl(webView);
            }
            if (e.OldElement != null)
            {
                Control.RemoveJavascriptInterface("JivoInterface");
                var hybridWebView = e.OldElement as HybridWebView;
                hybridWebView.Cleanup();
            }
            if (e.NewElement != null)
            {
                //Control.AddJavascriptInterface(new JivoInterface(Control, MainActivity.jivoSdk.JivoDelegate), "JivoInterface");
                //Control.LoadUrl(/*string.Format("file:///android_asset/Content/{0}", Element.Uri)*/Element.Uri);
                //InjectJS(JavaScriptFunction);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == HybridWebView.UriProperty.PropertyName)
            {
                JivoDelegate = MainActivity.jivoSdk.JivoDelegate;

                DisplayMetrics dm = new DisplayMetrics();
                ((Activity)JivoDelegate).GetSystemService(Context.WindowService).JavaCast<IWindowManager>().DefaultDisplay.GetMetrics(dm);
                float density = dm.Density;


                OnGlobalLayoutListener list = new OnGlobalLayoutListener(Control, density);
                Control.ViewTreeObserver.AddOnGlobalLayoutListener(list);


                progr = new ProgressDialog(Control.Context);
                progr.SetTitle("JivoSite");
                progr.SetMessage("Загрузка...");

                WebSettings webSettings = Control.Settings;
                webSettings.JavaScriptEnabled = true;
                webSettings.DomStorageEnabled = true;
                webSettings.DatabaseEnabled = true;

                //пробрасываем JivoInterface в Javascript
                Control.AddJavascriptInterface(new JivoInterface(Control, JivoDelegate), "JivoInterface");
                Control.SetWebViewClient(new MyWebViewClient(JivoDelegate, progr));

                Control.LoadUrl(/*string.Format("file:///android_asset/Content/{0}", Element.Uri)*/Element.Uri);

                //Control.AddJavascriptInterface(new JSBridge(this), "jsBridge");
                //Control.LoadUrl(/*string.Format("file:///android_asset/Content/{0}", Element.Uri)*/Element.Uri);
                //InjectJS(JavaScriptFunction);
            }

            if (e.PropertyName == HybridWebView.UriScriptProperty.PropertyName)
            {
                Control.LoadUrl(/*string.Format("file:///android_asset/Content/{0}", Element.Uri)*/Element.UriScript);
            }
        }


        void InjectJS(string script)
        {
            if (Control != null)
            {
                Control.LoadUrl(string.Format("javascript: {0}", script));
            }
        }
    }
}