using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace XamarinStore.Services
{
    public interface INavigationService
    {
        Frame MainFrame { get; }
        void Navigate(string pagename);

    }
}
