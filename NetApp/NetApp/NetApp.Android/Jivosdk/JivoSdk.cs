using System;


namespace NetApp.Droid.Jivosdk
{
    public class JivoSdk
    {
        public HybridWebView webView;
        //private ProgressDialog progr;
        private readonly String language;
        public IJivoDelegate JivoDelegate = null;

        public JivoSdk(HybridWebView webView)
        {
            this.webView = webView;
            this.language = "";
        }

        public JivoSdk(HybridWebView webView, String language)
        {
            this.webView = webView;
            this.language = language;
        }

        public void Prepare()
        {
            //DisplayMetrics dm = new DisplayMetrics();
            //((Activity)JivoDelegate).GetSystemService(Context.WindowService).JavaCast<IWindowManager>().DefaultDisplay.GetMetrics(dm);
            //float density = dm.Density;

            //OnGlobalLayoutListener list = new OnGlobalLayoutListener(this.webView, density);


            //webView.ViewTreeObserver.AddOnGlobalLayoutListener(list);

            //создаем спиннер
            //progr = new ProgressDialog(webView.Context);
            //progr.SetTitle("JivoSite");
            //progr.SetMessage("Загрузка...");

            //WebSettings webSettings = webView.Settings;
            //webSettings.JavaScriptEnabled = true;
            //webSettings.DomStorageEnabled = true;
            //webSettings.DatabaseEnabled = true;

            ////пробрасываем JivoInterface в Javascript
            //webView.AddJavascriptInterface(new JivoInterface(webView, JivoDelegate), "JivoInterface");
            //webView.SetWebViewClient(new MyWebViewClient(JivoDelegate, progr));

            if (this.language.Length > 0)
            {
                webView.Uri = "file:///android_asset/html/index_" + this.language + ".html";

                //webView.LoadUrl("file:///android_asset/html/index_" + this.language + ".html");
            }
            else
            {
                webView.Uri = "file:///android_asset/html/index.html";

                //webView.LoadUrl("file:///android_asset/html/index.html");
            }
        }


        public void callApiMethod(String methodName, String data)
        {
            webView.UriScript = "javascript:window.jivo_api." + methodName + "(" + data + ");";

            //webView.LoadUrl("javascript:window.jivo_api." + methodName + "(" + data + ");");
        }
    }

}
