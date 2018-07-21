using Xamarin.Forms;


namespace NetApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            //InitializeComponent();
        }

        public HybridWebView InitWebView()
        {
            var hybridWebView = new HybridWebView
            {
                //Uri = "file:///android_asset/html/index.html",
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
            };

            Padding = new Thickness(0, 20, 0, 0);

            Content = hybridWebView;

            //hybridWebView.RegisterAction(data => DisplayAlert("Alert", "Hello " + data, "OK"));

            return Content as HybridWebView;
            
        }

    }
}