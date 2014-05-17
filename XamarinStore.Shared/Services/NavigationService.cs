using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using XamarinStore.Helpers;
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
                case "address":
                    MainFrame.Navigate(typeof(ShippingAddressView));
                    break;
                case "processing":
                    MainFrame.Navigate(typeof(ProcessingView));
                    break;
                case "done":
                    MainFrame.Navigate(typeof(MainPage));
                    MainFrame.BackStack.Clear();
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
