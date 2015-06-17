using System;
using UIKit;

namespace SpeechAuth.Views
{
    public static partial class Theme
    {
        public static class Label
        {
            public static UILabel InfoLabel (UIFont font, UIColor textColor, string text = null)
            {
				var label = new UILabel
                {
                    Font = font,
                    TextColor = textColor,
                    TextAlignment = UITextAlignment.Center,
                    Lines = 0,
                    LineBreakMode = UILineBreakMode.TailTruncation
                };

                label.TranslatesAutoresizingMaskIntoConstraints = false;

                if (!string.IsNullOrEmpty(text))
                    label.Text = text;

                return label;
            }
        }
    }
}

