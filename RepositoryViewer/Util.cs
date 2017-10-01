using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace RepositoryViewer
{
    static class Util
    {
        public static GitHubClient Client => new GitHubClient(new ProductHeaderValue("repository-viewer"));

        public static async Task<SearchRepositoryResult> Search(string query)
        {
            const int retries = 4;

            for (int i = 0; i < retries; ++i)
            {
                try
                {
                    return await Client.Search.SearchRepo(new SearchRepositoriesRequest(query));
                }
                catch
                {
                    if (i + 1 == retries)
                    {
                        throw;
                    }
                }
            }

            throw new NotImplementedException();
        }
    }
}
