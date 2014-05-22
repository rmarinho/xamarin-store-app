using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using XamarinStore.Helpers;
using XamarinStore.ViewModel;

namespace XamarinStore.Views
{
    public sealed partial class ProcessingPage : BasePage
    {
        public ProcessingPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            this.Loaded += ProcessingPage_Loaded;
        }

        void ProcessingPage_Loaded(object sender, RoutedEventArgs e)
        {
            (this.DataContext as BasketViewModel).ProcessOrder();
        }

    }
}