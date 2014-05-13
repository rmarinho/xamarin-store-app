using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace XamarinStore.Helpers
{
    /// <summary>
    /// Shows or hides our badge if we have more the 0 items.
    /// Parameter reverses
    /// </summary>
    public class ProductcountToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int)
            {
                if ((int)value > 0)
                    return (parameter == null) ? Visibility.Visible : Visibility.Collapsed;
                else
                    return (parameter == null) ? Visibility.Collapsed : Visibility.Visible;
            }
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool)
            {
                if ((bool)value)
                    return (parameter == null) ? Visibility.Visible : Visibility.Collapsed;
                else
                    return (parameter == null) ? Visibility.Collapsed : Visibility.Visible;
            }
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
