using System;
using UIKit;
using System.Collections.Generic;
using SpeechAuth.Views;

namespace SpechAuth.iOS.Views.RecordSoundPage
{
    public partial class RecordSoundViewController
    {
        private UIButton  _startRecord;
        private UIButton  _stopRecord;
        private UILabel _secretWord;
        public UIStepIndicators StepIndicators;

        private static UIImageView _micro;

        public override void LoadView ()
        {
            base.LoadView ();
            InitializeControls ();
        }

        private void InitializeControls()
        {
            View.BackgroundColor = Theme.Color.MainBackgroundColor;

            View.AddSubviews(
                _micro = new UIImageView(UIImage.FromFile("Images/MicrophoneIcon.png"))
                .WithFrame((View.Frame.Width - 70)/2,32 + 64,70,110),
                _secretWord = Theme.Label.InfoLabel(Theme.Font.HelveticaNeue_Medium(20), UIColor.White)
                .WithFrame(70, 182 + 64, View.Frame.Width - 140, 60)
                .WithTune (tune => {
                    tune.Lines = 2;
                    tune.LineBreakMode = UILineBreakMode.WordWrap;
                    tune.TextAlignment = UITextAlignment.Center;
                }),
                StepIndicators = new UIStepIndicators().WithFrame((View.Frame.Width-320)/2 - 2, 293 + 64, 300, 30),
                _startRecord = Theme.Button.ImageResizableButton("Images/BlueButtonBackground.png", "Images/BlueButtonBackground_Pressed.png", new UIEdgeInsets(8,8,8,8))
                .WithFrame(25, 346 + 64, View.Frame.Width - 50, 60)
                .WithTune(tune => {
                    tune.SetTitleColor(UIColor.White, UIControlState.Normal);
                    tune.SetTitle ("Начать запись", UIControlState.Normal);
                }),
                _stopRecord = Theme.Button.ImageResizableButton("Images/BlueButtonBackground.png", "Images/BlueButtonBackground_Pressed.png", new UIEdgeInsets(8,8,8,8))
                .WithFrame(25, 420 + 64, View.Frame.Width - 50, 60)
                .WithTune(tune => {
                    tune.SetTitleColor(UIColor.White, UIControlState.Normal);
                    tune.SetTitle ("Завершить запись", UIControlState.Normal);
                })
            );
        }
    }
}

