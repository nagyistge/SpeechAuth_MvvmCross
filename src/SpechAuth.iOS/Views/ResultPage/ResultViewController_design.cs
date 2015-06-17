using UIKit;
using SpeechAuth.Views;
using System;

namespace SpechAuth.iOS.Views.ResultPage
{
    public partial class ResultViewController
    {
        private UIButton _toRootButton;
        private UILabel _message;

        public override void LoadView ()
        {
            base.LoadView ();
            InitializeControls ();
        }

        private void InitializeControls ()
        {
            View.BackgroundColor = Theme.Color.MainBackgroundColor;

            View.AddSubviews (                
                _message = Theme.Label.InfoLabel(Theme.Font.HelveticaNeue_Regular(20), UIColor.White)
                .WithFrame(50, 290, View.Frame.Width - 100, 60)
                .WithTune(tune => {
                    tune.Lines = 2;
                    tune.LineBreakMode = UILineBreakMode.WordWrap;
                    tune.TextAlignment = UITextAlignment.Center;
                }),
                _toRootButton = new UIButton()
                .WithFrame((View.Frame.Width - 120)/2, 420, 120, 40)
                .WithTune(tune => {
                    tune.SetTitle ("На главную", UIControlState.Normal);
                    tune.SetTitleColor(Theme.Color.MainBlueColor, UIControlState.Normal);
                    tune.Font = Theme.Font.HelveticaNeue_Regular(20);
                })
            );
        }
    }
}

