using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace VinylManager.Converters
{
    class SearchArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var args = (SearchBoxSuggestionsRequestedEventArgs)value;
            var displayHistory = (bool)parameter;

            if (args == null) return value;
            ISuggestionQuery item = new SuggestionQuery(args.Request, args.QueryText)
            {
                DisplayHistory = displayHistory
            };
            return item;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
