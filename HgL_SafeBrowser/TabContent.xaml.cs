using HgLBrowser.model;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HgL_SafeBrowser
{
    /// <summary>
    /// Interaction logic for TabContent.xaml
    /// </summary>
    public partial class TabContent : UserControl
    {
        private List<SearchEngine> searchEngines = new List<SearchEngine>
        {
            new SearchEngine
            {
                Name = "Google",
                ImageUrl = "pack://application:,,,/Assets/SearchEngines/gg.png",
                UrlTemplate = "https://www.google.com/search?q={0}"
            },
            new SearchEngine
            {
                Name = "Yahoo",
                ImageUrl = "pack://application:,,,/Assets/SearchEngines/ya.png",
                UrlTemplate = "https://search.yahoo.com/search?p={0}"
            },
            new SearchEngine
            {
                Name = "DuckDuckGo",
                ImageUrl = "pack://application:,,,/Assets/SearchEngines/duck.png",
                UrlTemplate = "https://duckduckgo.com/?q={0}"
            },
            new SearchEngine
            {
                Name = "Microsoft Bing",
                ImageUrl = "pack://application:,,,/Assets/SearchEngines/bing.png",
                UrlTemplate = "https://www.bing.com/search?q={0}"
            },
            new SearchEngine
            {
                Name = "Cốc Cốc",
                ImageUrl = "pack://application:,,,/Assets/SearchEngines/cc.png",
                UrlTemplate = "https://coccoc.com/search?query={0}"
            }
        };

        public TabContent()
        {
            InitializeComponent();
            cmbSearchEngine.ItemsSource = searchEngines;
            cmbSearchEngine.SelectedIndex = 0;

            imgSelectedEngine.Source = new BitmapImage(new Uri(searchEngines[0].ImageUrl, UriKind.RelativeOrAbsolute));
        }

        public string Url
        {
            get { return (string)GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }

        public static readonly DependencyProperty UrlProperty =
            DependencyProperty.Register("Url", typeof(string), typeof(TabContent), new PropertyMetadata("https://www.google.com"));

        private async void InitializeBrowser(object sender, EventArgs e)
        {
            await Browser.EnsureCoreWebView2Async();
            Browser.CoreWebView2.NavigationStarting += Browser_NavigationStarting;
            Browser.CoreWebView2.NavigationCompleted += Browser_NavigationCompleted;
            Browser.Source = new Uri(Url);
            Browser.NavigationCompleted += Browser_NavigationCompleted;

            var coreWebView2 = Browser.CoreWebView2;
            coreWebView2.NewWindowRequested += (s, args) =>
            {
                args.Handled = true; // Ngăn không cho mở cửa sổ mới
                coreWebView2.Navigate(args.Uri); 
            };
            Browser.Source = new Uri(Url);
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PerformSearch();
            }
        }

        private void PerformSearch()
        {
            string query = txtSearch.Text;

            if (string.IsNullOrWhiteSpace(query))
            {
                return;
            }

            if (cmbSearchEngine.SelectedItem is SearchEngine selectedEngine)
            {
                if (query.StartsWith("localhost"))
                {
                    try
                    {
                        Browser.Source = new Uri(query);
                    }
                    catch (UriFormatException ex)
                    {
                        MessageBox.Show("URL không hợp lệ. Vui lòng nhập lại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    string searchUrl = string.Format(selectedEngine.UrlTemplate, Uri.EscapeDataString(query));
                    Browser.Source = new Uri(searchUrl);
                }
            }
        }


        private void Browser_NavigationStarting(object? sender, CoreWebView2NavigationStartingEventArgs e)
        {
            LoadingProgress.Visibility = Visibility.Visible;
        }

        private void Browser_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            LoadingProgress.Visibility = Visibility.Collapsed;
        }

        private void cmbSearchEngine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSearchEngine.SelectedItem is SearchEngine selectedEngine)
            {
                imgSelectedEngine.Source = new BitmapImage(new Uri(selectedEngine.ImageUrl));
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (Browser.CoreWebView2 != null && Browser.CoreWebView2.CanGoBack)
            {
                Browser.GoBack();
            }
        }

        private void btnForward_Click(object sender, RoutedEventArgs e)
        {
            if (Browser.CoreWebView2 != null && Browser.CoreWebView2.CanGoForward)
            {
                Browser.GoForward();
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Browser.Reload();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}
