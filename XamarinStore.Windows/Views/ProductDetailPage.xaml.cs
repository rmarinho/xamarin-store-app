using Windows.UI.Xaml;
using XamarinStore.Helpers;

namespace XamarinStore.Views
{
    public sealed partial class ProductDetailPage : BasePage
    {
        public ProductDetailPage()
        {
            this.InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            topBar.IsOpen = true;
        }
      
    }
}
