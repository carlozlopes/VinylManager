using VinylManager.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace VinylManager.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdminPage : NavigationPage
    {
        AdminPageViewModel viewModel;

        public AdminPage()
        {
            viewModel = new AdminPageViewModel();
            this.InitializeComponent();
            this.DataContext = viewModel;
            PageDetailFrame.Navigate(typeof(SinglesPage));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem mft = sender as MenuFlyoutItem;
            // Frame rootFrame = Window.Current.Content as Frame;

            if (mft != null && mft.Tag != null)
            {
                Type pageType = Type.GetType(mft.Tag.ToString());

                if (pageType != null)
                {
                    PageDetailFrame.Navigate(pageType);
                    // rootFrame.Navigate(pageType);
                    // (App.Current as App).Navigate(pageType);
                }
                else if (pageType == null)
                {
                    // TODO: Optional - Do something if page not found.
                }
            }
        }
    }
}
