using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace XamarinStore.Services
{
    public interface INavigationService
    {
        void Navigate(string pagename);
    }
}
