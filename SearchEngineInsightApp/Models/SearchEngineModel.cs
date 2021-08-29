using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SearchEngineInsightApp
{
    public class SearchEngineModel : INotifyPropertyChanged
    {
        private List<string> _searchResults = new List<string>();
        private string _url;
        private string _keywords;
        private string _summary;
        public event PropertyChangedEventHandler PropertyChanged;

        public string Url 
        { 
            get 
            {
                return _url;
            }
            set
            {
                _url = value;
                OnPropertyChanged("Url");
            }
        }

        public string Keywords
        {
            get
            {
                return _keywords;
            }
            set
            {
                _keywords = value;
                OnPropertyChanged("Keywords");
            }
        }

        public string Summary
        {
            get
            {
                return _summary;
            }
            set
            {
                _summary = value;
                OnPropertyChanged("Summary");
            }
        }

        public SearchEngineModel()
        {
            Url = string.Empty;
            Keywords = string.Empty;
            Summary = string.Empty;
        }

        public List<string> SearchResults
        {
            get => _searchResults;
            set
            {
                _searchResults = value;
                OnPropertyChanged("SearchResults");
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
