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
