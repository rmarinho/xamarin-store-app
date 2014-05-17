
using Windows.UI.Xaml.Input;
using XamarinStore.Helpers;
using XamarinStore.ViewModel;

namespace XamarinStore.Views
{
    public sealed partial class LoginPage : BasePage
    {
        public LoginPage()
        {
            this.InitializeComponent();

        }

        private void txbPass_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            this.Focus(Windows.UI.Xaml.FocusState.Pointer);
            if (e.Key != Windows.System.VirtualKey.Enter)
                return;
            e.Handled = true;
            (this.DataContext as LoginViewModel).LoginCommand.Execute(null);
        }
    }
}
