using HtmlParser;
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
using WebScraper;

namespace SearchEngineInsightApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string APP_NAME = "Search Engine Insight";
        private const string RANKING_SUBJECT = "www.smokeball.com.au";
        private readonly GoogleScrapeWrapper _scraper;
        private SearchEngineModel _model = new SearchEngineModel();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = _model;
            _scraper = new GoogleScrapeWrapper(new HttpHandler());
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
            {
                return;
            }
            _model.SearchResults = _scraper.StartScraping(_model.Url, _model.Keywords, 100, RANKING_SUBJECT, out string[] ranks);
            string rank = ranks.Length == 0 ? "0" : string.Join(",", ranks);
            _model.Summary = $"Rank of {RANKING_SUBJECT}: {rank}";
        }
        
        private bool ValidateInput()
        {
            if(string.IsNullOrEmpty(_model.Url))
            {
                MessageBox.Show("Url cannot be empty or invalid.", APP_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrEmpty(_model.Keywords))
            {
                MessageBox.Show("Keywords cannot be empty.", APP_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
    }
}
