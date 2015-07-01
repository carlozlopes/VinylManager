using Windows.ApplicationModel.Search;
namespace VinylManager.Converters
{
    public interface ISuggestionQuery
    {
        SearchSuggestionsRequest Request { get; }
        string QueryText { get; }
        bool DisplayHistory { get; set; }
    }
}
