using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace XamarinStore.Helpers
{
    class Extensions
    {
    }

    public class Clip
    {
        public static bool GetToBounds(DependencyObject depObj)
        {
            return (bool)depObj.GetValue(ToBoundsProperty);
        }

        public static void SetToBounds(DependencyObject depObj, bool clipToBounds)
        {
            depObj.SetValue(ToBoundsProperty, clipToBounds);
        }

        /// <summary>
        /// Identifies the ToBounds Dependency Property.
        /// <summary>
        public static readonly DependencyProperty ToBoundsProperty =
            DependencyProperty.RegisterAttached("ToBounds", typeof(bool),
            typeof(Clip), new PropertyMetadata(false, OnToBoundsPropertyChanged));

        private static void OnToBoundsPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement fe = d as FrameworkElement;
            if (fe != null)
            {
                ClipToBounds(fe);

                // whenever the element which this property is attached to is loaded
                // or re-sizes, we need to update its clipping geometry
                fe.Loaded += new RoutedEventHandler(fe_Loaded);
                fe.SizeChanged += new SizeChangedEventHandler(fe_SizeChanged);
            }
        }

        /// <summary>
        /// Creates a rectangular clipping geometry which matches the geometry of the
        /// passed element
        /// </summary>
        private static void ClipToBounds(FrameworkElement fe)
        {
            if (GetToBounds(fe))
            {
                fe.Clip = new RectangleGeometry()
                {
                    Rect = new Rect(0, 0, fe.ActualWidth, fe.ActualHeight)
                };
            }
            else
            {
                fe.Clip = null;
            }
        }

        static void fe_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ClipToBounds(sender as FrameworkElement);
        }

        static void fe_Loaded(object sender, RoutedEventArgs e)
        {
            ClipToBounds(sender as FrameworkElement);
        }
    }
  
    public interface ICloneable
    {
        // Summary:
        //     Creates a new object that is a copy of the current instance.
        //
        // Returns:
        //     A new object that is a copy of this instance.
        object Clone();
    }

    class MessageBox
    {

        public static async Task<MessageBoxResult> ShowAsync(string messageBoxText,
                                                             string caption,
                                                             MessageBoxButton button)
        {

            MessageDialog md = new MessageDialog(messageBoxText, caption);
            MessageBoxResult result = MessageBoxResult.None;
            if (button.HasFlag(MessageBoxButton.OK))
            {
                md.Commands.Add(new UICommand("OK",
                    new UICommandInvokedHandler((cmd) => result = MessageBoxResult.OK)));
            }
            if (button.HasFlag(MessageBoxButton.Yes))
            {
                md.Commands.Add(new UICommand("Yes",
                    new UICommandInvokedHandler((cmd) => result = MessageBoxResult.Yes)));
            }
            if (button.HasFlag(MessageBoxButton.No))
            {
                md.Commands.Add(new UICommand("No",
                    new UICommandInvokedHandler((cmd) => result = MessageBoxResult.No)));
            }
            if (button.HasFlag(MessageBoxButton.Cancel))
            {
                md.Commands.Add(new UICommand("Cancel",
                    new UICommandInvokedHandler((cmd) => result = MessageBoxResult.Cancel)));
                md.CancelCommandIndex = (uint)md.Commands.Count - 1;
            }
            var op = await md.ShowAsync();
            return result;
        }

        public static async Task<MessageBoxResult> ShowAsync(string messageBoxText)
        {
            return await MessageBox.ShowAsync(messageBoxText, null, MessageBoxButton.OK);
        }
    }

    // Summary:
    //     Specifies the buttons to include when you display a message box.
    [Flags]
    public enum MessageBoxButton
    {
        // Summary:
        //     Displays only the OK button.
        OK = 1,
        // Summary:
        //     Displays only the Cancel button.
        Cancel = 2,
        //
        // Summary:
        //     Displays both the OK and Cancel buttons.
        OKCancel = OK | Cancel,
        // Summary:
        //     Displays only the OK button.
        Yes = 4,
        // Summary:
        //     Displays only the Cancel button.
        No = 8,
        //
        // Summary:
        //     Displays both the OK and Cancel buttons.
        YesNo = Yes | No,
    }

    // Summary:
    //     Represents a user's response to a message box.
    public enum MessageBoxResult
    {
        // Summary:
        //     This value is not currently used.
        None = 0,
        //
        // Summary:
        //     The user clicked the OK button.
        OK = 1,
        //
        // Summary:
        //     The user clicked the Cancel button or pressed ESC.
        Cancel = 2,
        //
        // Summary:
        //     This value is not currently used.
        Yes = 6,
        //
        // Summary:
        //     This value is not currently used.
        No = 7,
    }
}
