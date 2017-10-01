using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Octokit;
using Page = Windows.UI.Xaml.Controls.Page;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace RepositoryViewer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private SearchViewModel Model => (SearchViewModel)DataContext;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void NavigateToRepo(object id)
        {
            Frame.Navigate(typeof(RepositoryPage), id);
        }

        private async void SearchBox_OnSuggestionsRequested(SearchBox sender, SearchBoxSuggestionsRequestedEventArgs args)
        {
            if (string.IsNullOrWhiteSpace(args.QueryText))
                return;

            var deferral = args.Request.GetDeferral();

            SearchRepositoryResult result = await Util.Search(args.QueryText);

            foreach (Repository item in result.Items)
            {
                if (args.Request.IsCanceled)
                    return;

                args.Request.SearchSuggestionCollection.AppendResultSuggestion(
                    item.FullName,
                    item.Description ?? "No description",
                    item.Id.ToString(),
                    RandomAccessStreamReference.CreateFromUri(new Uri(item.Owner.AvatarUrl)),
                    "NoImage"
                );
            }

            deferral.Complete();
        }

        private async void SearchBox_OnQuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            var queryResult = await Util.Search(args.QueryText);

            ObservableCollection<Repository> repos = Model.SearchResult;
            repos.Clear();

            foreach (var repo in queryResult.Items)
                repos.Add(repo);
        }

        private void SearchBox_OnResultSuggestionChosen(SearchBox sender, SearchBoxResultSuggestionChosenEventArgs args)
        {
            NavigateToRepo(args.Tag);
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.FirstOrDefault() is Repository repo)
            {
                NavigateToRepo(repo.Id);
            }
        }
    }
}
