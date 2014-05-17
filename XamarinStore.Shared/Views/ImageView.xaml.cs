using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;


// This control was inspired by Isak Savo Sample on CodeProject
// And now refactored by Rui Marinho for WIN Store apps

/// Sample photo frame control
/// Written by Isak Savo <isak.savo@gmail.com> for Code Project. 
/// Licensed under the Code Project Open License (http://www.codeproject.com/info/cpol10.aspx)

namespace XamarinStore.Views
{
    public sealed partial class ImageView : UserControl
    {
       
        #region Dependency Properties
        /// <summary>
        /// Gets or sets the interval at which the photos will be switched.
        /// </summary>
        public TimeSpan AnimationDuration
        {
            get { return (TimeSpan)GetValue(AnimationDurationProperty); }
            set { SetValue(AnimationDurationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Interval.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnimationDurationProperty =
            DependencyProperty.Register("AnimationDuration", typeof(TimeSpan), typeof(ImageView), new PropertyMetadata(TimeSpan.FromSeconds(5)));


        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Interval.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(ImageView), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets the folder containing the images that should be displayed. If the value 
        /// isn't rooted (according to <see cref="System.IO.Path.IsPathRooted"/>), then it is assumed to be relative to the current working directory.
        /// </summary>
        /// <value>The image folder, either absolute path or relative to the current working directory.</value>
        public string Image
        {
            get { return (string)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageFolder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(string), typeof(ImageView), new PropertyMetadata(null, new PropertyChangedCallback(HandleImageChanged)));

        static void HandleIntervalChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (ImageView)sender;
        }

        static void HandleImageChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (ImageView)sender;
            ctrl.m_images.Add(ctrl.CreateImageSource(ctrl.Image));
            //if (e.OldValue == null)
            ctrl.LoadNextImage();
        }
        #endregion

        #region Fields
        List<ImageSource> m_images = new List<ImageSource>();
        int m_currentSourceIndex;
        int m_currentCtrlIndex;
        bool isImageLoaded = false; 
        #endregion

        #region Ctor
        public ImageView()
        {
            this.InitializeComponent();
            this.Loaded += ImageView_Loaded;

        }

        void ImageView_Loaded(object sender, RoutedEventArgs e)
        {
            if (isImageLoaded)
                CreateAndStartKenBurnsEffect(c_image1);
        }

        #endregion

        #region Image Loading

        private void LoadNextImage()
        {
            if (m_images.Count == 0)
                return;
            //var oldCtrlIndex = m_currentCtrlIndex;
            //m_currentCtrlIndex = (m_currentCtrlIndex + 1) % 2;
            //m_currentSourceIndex = (m_currentSourceIndex + 1) % m_images.Count;


            ImageSource newSource = m_images[0];
            c_image1.Source = newSource;
            IsBusy = true;
            c_image1.ImageOpened += newImage_ImageOpened;

        }

        private ImageSource CreateImageSource(string url)
        {
            var src = new BitmapImage();
            src.UriSource = new Uri(url, UriKind.Absolute);
            return src;

        }

        void newImage_ImageOpened(object sender, RoutedEventArgs e)
        {
            IsBusy = false;
            BeginFade((sender as Image), true);
            CreateAndStartKenBurnsEffect((sender as Image));
            (sender as Image).ImageOpened -= newImage_ImageOpened;
            isImageLoaded = true;
        } 
        #endregion

        #region Animations
        /// <summary>
        /// Starts the fade-in animation of the specified image control. 
        /// </summary>
        /// <param name="img">The image control that is faded in (and will be shown for the next 'Interval' time)</param>
        private void BeginFade(Image img, bool fadeIn)
        {
            var storyboard = new Storyboard();
            var fade = new DoubleAnimation();
            if (fadeIn)
            {
                fade.From = 0.0;
                fade.To = 1.0;
            }
            else
            {
                fade.From = 1.0;
                fade.To = 0.0;
            }

            fade.Duration = new Duration(TimeSpan.FromSeconds(2.0));
            storyboard.Children.Add(fade);

            Storyboard.SetTargetProperty(fade, "Opacity");
            Storyboard.SetTarget(storyboard, img);

            storyboard.Begin();

        }

        /// <summary>
        /// Creates the ken burns effect animations and applies them to the specified image control.
        /// </summary>
        /// <param name="img">The image control to apply the effect on.</param>
        private void CreateAndStartKenBurnsEffect(Image img)
        {
            var rand = new Random();
            var duration = new Duration(AnimationDuration);
            double scaleFrom = rand.Next(1100, 1500) / 1000.0;
            double scaleTo = rand.Next(1100, 1500) / 1000.0;

            var storyboard = new Storyboard();
            storyboard.Duration = duration;

            for (int i = 0; i < 4; i++)
            {

                var dblAnim = new DoubleAnimation();
                switch (i)
                {
                    case 0:
                        {
                            Storyboard.SetTargetProperty(dblAnim, "(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)");
                            dblAnim.From = 0;
                            dblAnim.To = rand.Next(-50, 50);
                            break;
                        }
                    case 1:
                        {
                            Storyboard.SetTargetProperty(dblAnim, "(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.Y)");
                            dblAnim.From = 0;
                            dblAnim.To = rand.Next(-50, 50);
                            break;
                        }
                    case 2:
                        {
                            Storyboard.SetTargetProperty(dblAnim, "(UIElement.RenderTransform).(TransformGroup.Children)[1].(ScaleTransform.ScaleX)");
                            dblAnim.To = scaleTo;
                            dblAnim.From = scaleFrom;
                            break;
                        }
                    case 3:
                        {

                            Storyboard.SetTargetProperty(dblAnim, "(UIElement.RenderTransform).(TransformGroup.Children)[1].(ScaleTransform.ScaleY)");
                            dblAnim.To = scaleTo;
                            dblAnim.From = scaleFrom;
                            break;
                        }
                    default:
                        break;
                }
                dblAnim.Duration = duration;

                Storyboard.SetTarget(dblAnim, img);
                storyboard.Children.Add(dblAnim);
            }
            storyboard.Begin();

        } 
        #endregion

        
    }
}
