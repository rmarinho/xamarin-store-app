using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using XamarinStore.Helpers;

namespace XamarinStore
{
    public sealed partial class MainPage : BasePage
    {

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        private void lstProducts_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            (sender as ListView).SelectedItem = null;
        }

    }
}
