using UIKit;
using SpeechAuth.Views;
using System;

namespace SpechAuth.iOS.Views.UserCreationPage
{
    public partial class UserCreationViewController
    {
        private UITextField _surname;
        private UITextField _name;
        private UITextField _midname;

        private UIButton _nextStepButton;

        public override void LoadView ()
        {
            base.LoadView ();
            InitializeControls ();
        }

        private void InitializeControls()
        {
            Title = "Новый пользователь";
            View.BackgroundColor = Theme.Color.MainBackgroundColor;

            View.AddSubviews(
                Theme.Label.InfoLabel(Theme.Font.HelveticaNeue_Medium(20), UIColor.White, "Фамилия")
                .WithFrame(20, 64 + 6, 100, 27)
                .WithTune (tune => tune.TextAlignment = UITextAlignment.Left),
                new UIImageView(ImageHelper.LoadResizableImage("Images/InputIcon.png", new UIEdgeInsets(4,8,4,8)))
                .WithTune(tune => tune.UserInteractionEnabled = true)
                .WithFrame(8, 64 + 35, View.Frame.Width - 16, 49)
                .WithSubviews(                    
                    _surname = new UITextField().WithFrame(5, 1, View.Frame.Width - 10, 47)
                    .WithTune(tune => {
                        tune.Font = Theme.Font.HelveticaNeue_Regular(20);
                        tune.TextColor = UIColor.White;
                        tune.ReturnKeyType = UIReturnKeyType.Next;
                        tune.ShouldReturn = field => {
                            _name.BecomeFirstResponder();
                            return true;
                        };
                        tune.KeyboardAppearance = UIKeyboardAppearance.Dark;
                    })
                ),

                Theme.Label.InfoLabel(Theme.Font.HelveticaNeue_Medium(20), UIColor.White, "Имя")
                .WithFrame(20, 64 + 92.5f, 100, 27)
                .WithTune (tune => tune.TextAlignment = UITextAlignment.Left),
                new UIImageView(ImageHelper.LoadResizableImage("Images/InputIcon.png", new UIEdgeInsets(4,8,4,8)))
                .WithTune(tune => tune.UserInteractionEnabled = true)
                .WithFrame(8, 64 + 92.5f + 27 + 6, View.Frame.Width - 16, 49)
                .WithSubviews(                    
                    _name = new UITextField().WithFrame(5, 1, View.Frame.Width - 10, 47)
                    .WithTune(tune => {
                        tune.Font = Theme.Font.HelveticaNeue_Regular(20);
                        tune.TextColor = UIColor.White;
                        tune.ReturnKeyType = UIReturnKeyType.Next;
                        tune.ShouldReturn = field => {
                            _midname.BecomeFirstResponder();
                            return true;
                        };
                        tune.KeyboardAppearance = UIKeyboardAppearance.Dark;
                    })
                ),

                Theme.Label.InfoLabel(Theme.Font.HelveticaNeue_Medium(20), UIColor.White, "Отчество")
                .WithFrame(20, 64 + 179, 100, 27)
                .WithTune (tune => tune.TextAlignment = UITextAlignment.Left),
                new UIImageView(ImageHelper.LoadResizableImage("Images/InputIcon.png", new UIEdgeInsets(4,8,4,8)))
                .WithTune(tune => tune.UserInteractionEnabled = true)
                .WithFrame(8, 64 + 179 + 27 + 6, View.Frame.Width - 16, 49)
                .WithSubviews(                    
                    _midname = new UITextField().WithFrame(5, 1, View.Frame.Width - 10, 47)
                    .WithTune(tune => {
                        tune.Font = Theme.Font.HelveticaNeue_Regular(20);
                        tune.TextColor = UIColor.White;
                        tune.ReturnKeyType = UIReturnKeyType.Done;
                        tune.ShouldReturn = field => {                            
                            ViewModel.NextCommand.Execute(null);
                            return true;
                        };
                        tune.KeyboardAppearance = UIKeyboardAppearance.Dark;
                    })
                ),

                _nextStepButton = new UIButton()
                .WithFrame((View.Frame.Width-120)/2,64 + 35 + 49*3 + 25*3 + 21,120,25)
                .WithTune(tune => {
                    tune.SetTitle ("Далее", UIControlState.Normal);
                    tune.SetTitleColor(Theme.Color.MainBlueColor, UIControlState.Normal);
                    tune.SetTitleColor(Theme.Color.LightBlueColor, UIControlState.Highlighted);
                    tune.Font = Theme.Font.HelveticaNeue_Regular(20);
                })
            );
        }
    }
}

