using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using XamarinStore.Helpers;

namespace XamarinStore
{
    public sealed partial class MainPage : BasePage
    {

        public MainPage()
        {
            this.InitializeComponent();

        }
      
        private void productsGridView_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as GridView).SelectedItem = null;
        }
    }
}
