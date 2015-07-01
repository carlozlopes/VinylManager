using Windows.ApplicationModel.Search;

namespace VinylManager.Converters
{
    public class SuggestionQuery : ISuggestionQuery
    {
        public SuggestionQuery(SearchSuggestionsRequest request, string queryText)
        {
            Request = request;
            QueryText = queryText;
        }

        public SearchSuggestionsRequest Request { get; private set; }
        public string QueryText { get; private set; }
        public bool DisplayHistory { get; set; }
    }
}
