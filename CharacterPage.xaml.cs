using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;

namespace BushidoApp
{
    public partial class CharacterPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        const double MaxScale = 10;

        double _scale = 1.0;
        double _minScale;
        double _coercedScale;
        double _originalScale;

        Size _viewportSize;
        bool _pinching;
        Point _screenMidpoint;
        Point _relativeMidpoint;

        BitmapImage _bitmap;

        /// <summary> 
        /// Either the user has manipulated the image or the size of the viewport has changed. We only 
        /// care about the size. 
        /// </summary> 
        void viewport_ViewportChanged(object sender, System.Windows.Controls.Primitives.ViewportChangedEventArgs e)
        {
            Size newSize = new Size(viewport.Viewport.Width, viewport.Viewport.Height);
            if (newSize != _viewportSize) {
                _viewportSize = newSize;
                CoerceScale(true);
                ResizeImage(false);
            }
        }

        /// <summary> 
        /// Handler for the ManipulationStarted event. Set initial state in case 
        /// it becomes a pinch later. 
        /// </summary> 
        void OnManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            _pinching = false;
            _originalScale = _scale;
        }

        /// <summary> 
        /// Handler for the ManipulationDelta event. It may or may not be a pinch. If it is not a  
        /// pinch, the ViewportControl will take care of it. 
        /// </summary> 
        /// <param name="sender"></param> 
        /// <param name="e"></param> 
        void OnManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (e.PinchManipulation != null) {
                e.Handled = true;

                if (!_pinching) {
                    _pinching = true;
                    Point center = e.PinchManipulation.Original.Center;
                    _relativeMidpoint = new Point(center.X / MiniImage.ActualWidth, center.Y / MiniImage.ActualHeight);

                    var xform = MiniImage.TransformToVisual(viewport);
                    _screenMidpoint = xform.Transform(center);
                }

                _scale = _originalScale * e.PinchManipulation.CumulativeScale;

                CoerceScale(false);
                ResizeImage(false);
            } else if (_pinching) {
                _pinching = false;
                _originalScale = _scale = _coercedScale;
            }
        }

        /// <summary> 
        /// The manipulation has completed (no touch points anymore) so reset state. 
        /// </summary> 
        void OnManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            _pinching = false;
            _scale = _coercedScale;
        }


        /// <summary> 
        /// When a new image is opened, set its initial scale. 
        /// </summary> 
        void OnImageOpened(object sender, RoutedEventArgs e)
        {
            _bitmap = (BitmapImage)MiniImage.Source;

            // Set scale to the minimum, and then save it. 
            _scale = 0;
            CoerceScale(true);
            _scale = _coercedScale;

            ResizeImage(true);
        }

        /// <summary> 
        /// Adjust the size of the image according to the coerced scale factor. Optionally 
        /// center the image, otherwise, try to keep the original midpoint of the pinch 
        /// in the same spot on the screen regardless of the scale. 
        /// </summary> 
        /// <param name="center"></param> 
        void ResizeImage(bool center)
        {
            if (_coercedScale != 0 && _bitmap != null) {
                double newWidth = canvas.Width = Math.Round(_bitmap.PixelWidth * _coercedScale);
                double newHeight = canvas.Height = Math.Round(_bitmap.PixelHeight * _coercedScale);

                xform.ScaleX = xform.ScaleY = _coercedScale;

                viewport.Bounds = new Rect(0, 0, newWidth, newHeight);

                if (center) {
                    viewport.SetViewportOrigin(
                        new Point(
                            Math.Round((newWidth - viewport.ActualWidth) / 2),
                            Math.Round((newHeight - viewport.ActualHeight) / 2)
                            ));
                } else {
                    Point newImgMid = new Point(newWidth * _relativeMidpoint.X, newHeight * _relativeMidpoint.Y);
                    Point origin = new Point(newImgMid.X - _screenMidpoint.X, newImgMid.Y - _screenMidpoint.Y);
                    viewport.SetViewportOrigin(origin);
                }
            }
        }

        /// <summary> 
        /// Coerce the scale into being within the proper range. Optionally compute the constraints  
        /// on the scale so that it will always fill the entire screen and will never get too big  
        /// to be contained in a hardware surface. 
        /// </summary> 
        /// <param name="recompute">Will recompute the min max scale if true.</param> 
        void CoerceScale(bool recompute)
        {
            if (recompute && _bitmap != null && viewport != null) {
                // Calculate the minimum scale to fit the viewport 
                double minX = viewport.ActualWidth / _bitmap.PixelWidth;
                double minY = viewport.ActualHeight / _bitmap.PixelHeight;

                _minScale = Math.Min(minX, minY);
            }

            _coercedScale = Math.Min(MaxScale, Math.Max(_scale, _minScale));

        }

        int charIndex { get; set; }
        int factIndex { get; set; }

        private System.Windows.Media.SolidColorBrush color;
        public System.Windows.Media.SolidColorBrush Color
        {
            get { return color; }
            set
            {
                if (value == color) return;
                color = value;
                NotifyPropertyChanged("Color");
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string nomPropriete) {
            if (PropertyChanged != null)    
                PropertyChanged(this, new PropertyChangedEventArgs(nomPropriete));
        }

        public void loadImage()
        {
            //MiniImage.Source = FactionList.Factions()[factIndex].Characters[charIndex].MinisImage;
            MiniImage.Source = Image.Get(FactionList.Factions()[factIndex].Characters[charIndex].MinisPath);
            int cnt = 1;
            BitmapImage img;
            // foreach (BitmapImage img in FactionList.Factions()[factIndex].Characters[charIndex].ProfileImage)
            foreach (string path in FactionList.Factions()[factIndex].Characters[charIndex].ProfilePaths)   
            {
                img = Image.Get(path);
                switch(cnt)
                {
                    case 1: ProfileImageFirst_1.Source = img;
                            ProfileImageBack_1.Source = img;
                            break;
                    case 2: ProfileImageFirst_2.Source = img;
                            ProfileImageBack_2.Source = img;
                            break;
                    case 3: ProfileImageFirst_3.Source = img;
                            ProfileImageBack_3.Source = img;
                            break;
                    case 4: ProfileImageFirst_4.Source = img;
                            ProfileImageBack_4.Source = img;
                            break;
                    case 5: ProfileImageFirst_5.Source = img;
                            ProfileImageBack_5.Source = img;
                            break;
                }
                cnt += 1;
            }
            
            cnt *= 2;
            cnt -= 1;
            
            int max = Pivot.Items.Count;
            for (int i = cnt; i < max; i++)
            {
                    Pivot.Items.RemoveAt(Pivot.Items.Count-1);
            }
            //if (FactionList.Factions()[factIndex].Characters[charIndex].MinisImage == null) 
            if (MiniImage.Source == null) 
            {
                Pivot.Items.RemoveAt(0);
            }
        }

        public CharacterPage()
        {
            InitializeComponent();
            //ResizeImage(false);
            DataContext = this;
        }
        
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string charIndexStr, factIndexStr;
            if (NavigationContext.QueryString.TryGetValue("charIndex", out charIndexStr) &&
                NavigationContext.QueryString.TryGetValue("factIndex", out factIndexStr)) {
                charIndex = int.Parse(charIndexStr);
                factIndex = int.Parse(factIndexStr);
                ChangeColor(FactionList.Factions()[factIndex].Name);
                loadImage();
            }
            base.OnNavigatedTo(e);
        }


        protected void ChangeColor(string factionName){
            ApplicationTitle.Foreground = Faction.GetColor(factionName);
            Color = Faction.GetColor(factionName);
            // Pivot.ItemTemplate.Foreground = Faction.GetColor(factionName);
            
            //miniHeader.Foreground = Faction.GetColor(factionName);
            /*
            HeaderF1.Foreground = Faction.GetColor(factionName);
            HeaderF2.Foreground = Faction.GetColor(factionName);
            HeaderF3.Foreground = Faction.GetColor(factionName);
            HeaderF4.Foreground = Faction.GetColor(factionName);
            HeaderF5.Foreground = Faction.GetColor(factionName);
            HeaderB1.Foreground = Faction.GetColor(factionName);
            HeaderB2.Foreground = Faction.GetColor(factionName);
            HeaderB3.Foreground = Faction.GetColor(factionName);
            HeaderB4.Foreground = Faction.GetColor(factionName);
            HeaderB5.Foreground = Faction.GetColor(factionName);
            //*/
            
        }
     }
}