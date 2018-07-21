using System;
using System.Text;

using Android.App;
using Android.Webkit;
using Android.Graphics;




namespace NetApp.Droid.Jivosdk
{
    class MyWebViewClient : WebViewClient
    {
        private IJivoDelegate JivoDelegate = null;
        private ProgressDialog progr;

        public MyWebViewClient(IJivoDelegate JivoDelegate, ProgressDialog progr)
        {
            this.JivoDelegate = JivoDelegate;
            this.progr = progr;
        }

        public override bool  ShouldOverrideUrlLoading(WebView view, String url)
        {
            if (url.ToLower().IndexOf("jivoapi://") == 0)
            {
                String[] components = url.Replace("jivoapi://", "").Split('/');

                String apiKey = components[0];
                String data = "";
                if (components.Length > 1)
                {
                    data = DecodeString(components[1]);
                }

                if (JivoDelegate != null)
                {
                    JivoDelegate.onEvent(apiKey, data);
                }

                return true;
            }

            // Otherwise, the link is not for a page on my site, so launch another Activity that handles URLs
            //Intent intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(url));
            //StartActivity(intent);
            return true;
        }

        
        public override void OnPageStarted(WebView view, String url, Bitmap favicon)
        {
            base.OnPageStarted(view, url, favicon);
        }

        
        public override void OnPageFinished(WebView view, String url)
        {
            base.OnPageFinished(view, url);
            progr.Dismiss();
        }

        private static string DecodeString(String encodedURI)
        {
            char actualChar;

            StringBuilder buffer = new StringBuilder();

            int bytePattern, sumb = 0;

            for (int i = 0, more = -1; i < encodedURI.Length; i++)
            {
                actualChar = encodedURI[i];

                switch (actualChar)
                {
                    case '%':
                        {
                            actualChar = encodedURI[++i];
                            int hb = (char.IsDigit(actualChar) ? actualChar - '0'
                                    : 10 + char.ToLower(actualChar) - 'a') & 0xF;
                            actualChar = encodedURI[++i];
                            int lb = (char.IsDigit(actualChar) ? actualChar - '0'
                                    : 10 + char.ToLower(actualChar) - 'a') & 0xF;
                            bytePattern = (hb << 4) | lb;
                            break;
                        }
                    case '+':
                        {
                            bytePattern = ' ';
                            break;
                        }
                    default:
                        {
                            bytePattern = actualChar;
                            break;
                        }
                }
                
                if ((bytePattern & 0xc0) == 0x80)
                { // 10xxxxxx
                    sumb = (sumb << 6) | (bytePattern & 0x3f);
                    if (--more == 0)
                        buffer.Append((char)sumb);
                }
                else if ((bytePattern & 0x80) == 0x00)
                { // 0xxxxxxx
                    buffer.Append((char)bytePattern);
                }
                else if ((bytePattern & 0xe0) == 0xc0)
                { // 110xxxxx
                    sumb = bytePattern & 0x1f;
                    more = 1;
                }
                else if ((bytePattern & 0xf0) == 0xe0)
                { // 1110xxxx
                    sumb = bytePattern & 0x0f;
                    more = 2;
                }
                else if ((bytePattern & 0xf8) == 0xf0)
                { // 11110xxx
                    sumb = bytePattern & 0x07;
                    more = 3;
                }
                else if ((bytePattern & 0xfc) == 0xf8)
                { // 111110xx
                    sumb = bytePattern & 0x03;
                    more = 4;
                }
                else
                { // 1111110x
                    sumb = bytePattern & 0x01;
                    more = 5;
                }
            }
            return buffer.ToString();
        }
    }
}