using System;

using Android.Webkit;
using Java.Interop;
using Android.Content;


namespace NetApp.Droid.Jivosdk
{
    public class JivoInterface : Java.Lang.Object
    {
        private WebView mAppView;
        private IJivoDelegate JivoDelegate = null;
        private Context context;

        public JivoInterface(Context context)
        {
            this.context = context;
        }

        public JivoInterface(WebView appView, IJivoDelegate JivoDelegate)
        {
            this.mAppView = appView;
            this.JivoDelegate = JivoDelegate;
        }

        [Export]
        [JavascriptInterface]
        public void send(String name, String data)
        {
            if (JivoDelegate != null)
            {
                JivoDelegate.onEvent(name, data);
            }
        }
    }
}

