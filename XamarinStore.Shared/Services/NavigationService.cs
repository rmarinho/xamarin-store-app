using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using XamarinStore.Views;

namespace XamarinStore.Services
{
    public class NavigationService : INavigationService
    {

        public void Navigate(string pagename)
        {
            switch (pagename)
            {
                case "detail":
                    MainFrame.Navigate(typeof(ProductDetailPage));
                    break;
                case "basket":
                    MainFrame.Navigate(typeof(BasketPage));
                    break;
                case "login":
                    MainFrame.Navigate(typeof(LoginPage));
                    break;
                default:
                    break;
            }
        }

        public Frame MainFrame
        {
            get
            {
                return App.RootFrame;
            }
         
        }
    }
}
