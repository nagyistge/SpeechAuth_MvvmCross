using UIKit;
using System;
using SpechAuth.iOS;

namespace SpeechAuth.Views.ProfilesPage
{
    public partial class ProfilesViewController
    {
        private UIButton _authButton;
        private UIButton _addButton;
        private UIButton _fourierButton;
        private UIButton _waveletButton;

        private UITableView _items;

        public override void LoadView ()
        {
            base.LoadView ();

            Title = "Профили";
            View.BackgroundColor = Theme.Color.MainBackgroundColor;
            
            View.Add (
                _items = new UITableView ()
                .WithFrame (0, 0, View.Frame.Width, View.Frame.Height)
                .WithTune (tune => {
                    tune.ContentInset = new UIEdgeInsets (0, 0, 110, 0);
                    tune.BackgroundColor = UIColor.Clear;
                    tune.SeparatorStyle = UITableViewCellSeparatorStyle.None;
                })
            );

            View.AddSubviews (
                _waveletButton = Theme.Button.ImageButton(new CoreGraphics.CGRect(28 + 13, View.Frame.Height - 16 - 80 - 13 - 52.5f - 52.5f - 13, 52.5f, 52.5f), "Images/WaveletButtonIcon.png", "Images/WaveletButtonIcon_Pressed.png"),
                _fourierButton = Theme.Button.ImageButton(new CoreGraphics.CGRect(28 + 13, View.Frame.Height - 16 - 80 - 13 - 52.5f, 52.5f, 52.5f), "Images/FourierButtonIcon.png", "Images/FourierButtonIcon_Pressed.png"),
                _authButton = Theme.Button.ImageButton(new CoreGraphics.CGRect(28, View.Frame.Height - 16 - 80, 80, 80), "Images/AuthButtonIcon.png", "Images/AuthButtonIcon_Pressed.png"),
                _addButton = Theme.Button.ImageButton(new CoreGraphics.CGRect(View.Frame.Width - 28 - 80, View.Frame.Height - 16 - 80, 80, 80), "Images/AddButtonIcon.png", "Images/AddButtonIcon_Pressed.png")
            );

            _waveletButton.Frame = new CoreGraphics.CGRect (41, View.Frame.Height - 130, 52.5f, 52.5f);
            _waveletButton.Alpha = 0;
            _fourierButton.Frame = new CoreGraphics.CGRect (41, View.Frame.Height - 100, 52.5f, 52.5f);
            _fourierButton.Alpha = 0;

            InitializeDialog ();
        }

        void InitializeDialog ()
        {
            var window = AppDelegate.Window;

            var xOffset = (window.Frame.Width - 305) / 2;

            _shadow = new UIView (window.Frame).WithBackground (UIColor.FromRGBA (0, 0, 0, (nfloat)0.75));
            _shadow.Alpha = 0;
//            _shadow.AddGestureRecognizer (new UITapGestureRecognizer (() => HideGraphDialog (null)));

            _dialog = new UIImageView (ImageHelper.LoadResizableImage ("Images/DialogBackground.png", new UIEdgeInsets (8, 8, 8, 8)))
                .WithFrame (xOffset, window.Frame.Height, 305, 275)
                .WithTune (tune => tune.UserInteractionEnabled = true)
                .WithSubviews (
                    new StackPanel (StackPanelOrientation.Vertical)
                    .WithFrame (0, 0, 305, 275)
                    .WithSubviews (
                        Theme.Label.InfoLabel (Theme.Font.HelveticaNeue_Medium (19), UIColor.Black, "Выберите график")
                        .WithFrame (65.5f, 16, 170, 24), 
                        new UIView ()
                        .WithFrame (0, 16, 305, 0.5f)
                        .WithBackground (UIColor.FromRGB (151, 151, 151)), 

                        Theme.Button.ClearButton ()
                        .WithBackground (UIColor.Clear)
                        .WithFrame (0, 0, 305, 79)
                        .WithSubviews (
                            Theme.Label.InfoLabel (Theme.Font.HelveticaNeue_Medium (14), UIColor.Black, "Преобразование Фурье")
                            .WithFrame (20, 30.5f, 168, 18), 
                            new UIImageView (UIImage.FromFile ("Images/CheckFillIcon"), UIImage.FromFile ("Images/CheckStrokeIcon.png")) 
                            .WithFrame (259, 28, 26, 26)
                            .WithTune (tune => tune.Highlighted = false)
                        )
                        .WithTune (tune => tune.TouchUpInside += (s, e) => {
                            HideGraphDialog(null);
                            ViewModel.ShowFourierGraph.Execute(null);
                        }), 
                        new UIView ()
                        .WithFrame (0, 0, 305, 0.5f)
                        .WithBackground (UIColor.FromRGB (151, 151, 151)), 

                        Theme.Button.ClearButton ()
                        .WithBackground (UIColor.Clear)
                        .WithFrame (0, 0, 305, 79)
                        .WithSubviews (
                            Theme.Label.InfoLabel (Theme.Font.HelveticaNeue_Medium (14), UIColor.Black, "Вейвлет-преобразование")
                            .WithFrame (20, 30.5f, 180, 18), 
                            new UIImageView (UIImage.FromFile ("Images/CheckFillIcon"), UIImage.FromFile ("Images/CheckStrokeIcon.png"))
                            .WithFrame (259, 28, 26, 26)
                            .WithTune (tune => tune.Highlighted = false)
                        )
                        .WithTune (tune => tune.TouchUpInside += (s, e) => {
                            HideGraphDialog(null);
                            ViewModel.ShowWaveletGraph.Execute(null);
                        }), 
                        new UIView ()
                        .WithFrame (0, 0, 305, 0.5f)
                        .WithBackground (UIColor.FromRGB (151, 151, 151)), 

                        new UIButton ()
                        .WithFrame (92.5f, 14, 120, 25)
                        .WithTune (tune => {
                            tune.SetTitle ("Отмена", UIControlState.Normal);
                            tune.SetTitleColor (Theme.Color.MainBlueColor, UIControlState.Normal);
                            tune.SetTitleColor (Theme.Color.LightBlueColor, UIControlState.Highlighted);
                            tune.Font = Theme.Font.HelveticaNeue_Regular (16);  
                            tune.TouchUpInside += (sender, e) => HideGraphDialog(null);
                        })
                    )
                );
        }
    }
}

