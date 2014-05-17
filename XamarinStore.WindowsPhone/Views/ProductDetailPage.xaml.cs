using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using XamarinStore.Helpers;
using XamarinStore.ViewModel;
namespace XamarinStore.Views
{
    public sealed partial class ProductDetailPage : BasePage
    {
        public ProductDetailPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            this.Loaded += ProductDetailPage_Loaded;
            this.Unloaded += ProductDetailPage_Unloaded;
        }

        void ProductDetailPage_Unloaded(object sender, RoutedEventArgs e)
        {
            GoToCart.Completed -= GoToCart_Completed;
        }

        void ProductDetailPage_Loaded(object sender, RoutedEventArgs e)
        {
            GoToCart.Completed += GoToCart_Completed;
        }

        void GoToCart_Completed(object sender, object e)
        {
            grid.Width = 0;
            grid.Height = 0;
            grid.Margin = new Thickness(0, 0, 0, 0);
            grid.RenderTransform =   new CompositeTransform();
            (this.DataContext as MainViewModel).ProductCount++;
        }


    }
}
