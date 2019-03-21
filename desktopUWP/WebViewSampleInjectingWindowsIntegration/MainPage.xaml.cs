using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WebViewSampleInjectingWindowsIntegration
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            webView.NavigationCompleted += WebView_NavigationCompleted;
            webView.ScriptNotify += WebView_ScriptNotify;
            webView.Navigate(new Uri("https://webviewpwademo.azurewebsites.net/"));
        }

        private void WebView_ScriptNotify(object sender, NotifyEventArgs e)
        {
            switch(e.Value.ToLower())
            {
                case "refresh":
                    {
                        webView.Refresh();
                        break;
                    };
                case "add local files":
                    {
                        InsertFileList();
                        break;
                    };
                default:
                    {
                        //Don't know what event just got bubbled. Should log and move on. 
                        break;
                    }
            }
        }

        private void WebView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            InsertLocalFileScriptIntoWebView();
        }

        //Reads the list of files, creates a JSON object and inserts it into the page
        private async void InsertFileList()
        {
            var tcs = new TaskCompletionSource<string>();

            try
            {
                var files = Directory.EnumerateFiles(Windows.ApplicationModel.Package.Current.InstalledLocation.Path);

                string items = BuildJSONFromListOfFiles(files);

                webView.InvokeScriptAsync("addItems", new string[] { items });
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }

        }

        private static string BuildJSONFromListOfFiles(IEnumerable<string> files)
        {
            string items = "[";
            bool firstFile = true;
            foreach (string currentFile in files)
            {
                if (firstFile)
                {
                    firstFile = false;
                }
                else
                {
                    items += ", ";
                }

                string currentFileSafeString = System.Net.WebUtility.HtmlEncode(currentFile);
                currentFileSafeString = currentFileSafeString.Replace("\\", "\\\\");
                items += "{ \"url\" : " + "\"" + currentFileSafeString + "\", " + "\"title\" : " + "\"" + currentFileSafeString + "\"}";
            }
            items += "]";
            return items;
        }

        //Inserts local JS file into the HTML page that's running in the WebView. 
        private void InsertLocalFileScriptIntoWebView()
        {
            string localFileReference = "var js = document.createElement('script');"
            + " js.async = true;"
            + " js.src = \"ms-appx-web:///Assets/js/localAccess.js\"; "
            + " document.body.appendChild(js);";

            webView.InvokeScriptAsync("eval", new string[] { localFileReference });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            InsertFileList();
        }
    }
}
