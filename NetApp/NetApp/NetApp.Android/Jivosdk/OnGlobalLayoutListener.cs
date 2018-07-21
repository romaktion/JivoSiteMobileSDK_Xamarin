using System;

using Android.Views;
using Android.Graphics;
using Android.Webkit;

namespace NetApp.Droid.Jivosdk
{
    public class OnGlobalLayoutListener : Java.Lang.Object, ViewTreeObserver.IOnGlobalLayoutListener
    {
        //public IntPtr Handle => throw new NotImplementedException();

        private WebView webView;
        private float density;
        private int previousHeightDiff = 0;

        public OnGlobalLayoutListener(WebView webView, float density)
        {
            this.webView = webView;
            this.density = density;
        }

        //public void Dispose()
        //{
        //    throw new NotImplementedException();
        //}

        public void OnGlobalLayout()
        {
            Rect r = new Rect();
            //r will be populated with the coordinates of your view that area still visible.
            webView.GetWindowVisibleDisplayFrame(r);

            int heightDiff = webView.RootView.Height - r.Bottom;
            int pixelHeightDiff = (int)(heightDiff / density);
            if (pixelHeightDiff > 100 && pixelHeightDiff != previousHeightDiff)
            { // if more than 100 pixels, its probably a keyboard...
              //String msg = "S" + Integer.toString(pixelHeightDiff);
                ExecJS("window.onKeyBoard({visible:false, height:0})");
            }
            else if (pixelHeightDiff != previousHeightDiff && (previousHeightDiff - pixelHeightDiff) > 100)
            {
                //String msg = "H";
                ExecJS("window.onKeyBoard({visible:false, height:0})");
            }
            previousHeightDiff = pixelHeightDiff;
        }

        public void ExecJS(String script)
        {
            webView.LoadUrl("javascript:" + script);
        }
    }
}