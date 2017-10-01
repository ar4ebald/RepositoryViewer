using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Octokit;

namespace RepositoryViewer
{
    class SearchViewModel
    {
        public string Query { get; set; }

        public ObservableCollection<Repository> SearchResult { get; }

        public SearchViewModel()
        {
            SearchResult = new ObservableCollection<Repository>();
        }
    }
}
