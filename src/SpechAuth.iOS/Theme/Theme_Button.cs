using System;
using UIKit;
using System.Drawing;
using CoreGraphics;

namespace SpeechAuth.Views
{
    public partial class Theme
    {
        public static class Button 
        {
            public static UIButton ImageButton (CGRect frame, string normalPath, string highlightedPath, Action<UIButton> tune = null)
            {
                var button = new UIButton(frame);

                if (!string.IsNullOrEmpty(normalPath))
                {
                    button.SetBackgroundImage(UIImage.FromFile(normalPath), UIControlState.Normal);
                    button.SetBackgroundImage(UIImage.FromFile(normalPath), UIControlState.Disabled);
                }
                if (!string.IsNullOrEmpty(highlightedPath))
                {
                    button.SetBackgroundImage(UIImage.FromFile(highlightedPath), UIControlState.Highlighted);
                    button.SetBackgroundImage(UIImage.FromFile(highlightedPath), UIControlState.Selected);
                }

                if (tune != null)
                    tune(button);

                return button;
            }

            public static UIButton ImageButton(CGRect frame, UIImage normalImage, UIImage highlightedImage, Action<UIButton> tune = null)
            {
                var button = new UIButton(frame);

                if (normalImage != null)
                {
                    button.SetBackgroundImage(normalImage, UIControlState.Normal);
                    button.SetBackgroundImage(normalImage, UIControlState.Disabled);
                }
                if (highlightedImage != null)
                {
                    button.SetBackgroundImage(highlightedImage, UIControlState.Highlighted);
                    button.SetBackgroundImage(highlightedImage, UIControlState.Selected);
                }

                if (tune != null)
                    tune(button);

                return button;
            }

			public static UIButton ImageButton (string normalPath, string highlightedPath, Action<UIButton> tune = null)
			{
				var button = new UIButton();

                if (!string.IsNullOrEmpty(normalPath))
				{
					button.SetBackgroundImage(UIImage.FromFile(normalPath), UIControlState.Normal);
					button.SetBackgroundImage(UIImage.FromFile(normalPath), UIControlState.Disabled);
				}
                if (!string.IsNullOrEmpty(highlightedPath))
				{
					button.SetBackgroundImage(UIImage.FromFile(highlightedPath), UIControlState.Highlighted);
					button.SetBackgroundImage(UIImage.FromFile(highlightedPath), UIControlState.Selected);
				}

				if (tune != null)
					tune(button);


				return button;
			}

            public static UIButton ImageResizableButton(string normalPath, string highlightedPath, UIEdgeInsets insets, Action<UIButton> tune = null)
            {
                var button = new UIButton();

                button.SetBackgroundImage(ImageHelper.LoadResizableImage(normalPath, insets), UIControlState.Normal);
                button.SetBackgroundImage(ImageHelper.LoadResizableImage(highlightedPath, insets), UIControlState.Highlighted);
                button.SetBackgroundImage(ImageHelper.LoadResizableImage(highlightedPath, insets), UIControlState.Selected);

                if(tune != null)
                    tune(button);

                return button;
            }

            public static UIButton ClearButton ()
            {
                var button = new UIButton ();

                //touches began
                button.TouchDown += (object sender, EventArgs e) =>
                    Array.ForEach(button.Subviews, view =>
                        {
                            if ((view as UIImageView) != null)
                                (view as UIImageView).Highlighted = true;
                            else if ((view as UILabel) != null)
                                (view as UILabel).Highlighted = true;
                        });
                button.TouchDragInside += (object sender, EventArgs e) => 
                    Array.ForEach(button.Subviews, view =>
                        {
                            if ((view as UIImageView) != null)
                                (view as UIImageView).Highlighted = true;
                            else if ((view as UILabel) != null)
                                (view as UILabel).Highlighted = true;
                        });

                //touches ended
                button.TouchUpInside += (object sender, EventArgs e) => 
                    Array.ForEach(button.Subviews, view =>
                        {
                            if ((view as UIImageView) != null)
                                (view as UIImageView).Highlighted = false;
                            else if ((view as UILabel) != null)
                                (view as UILabel).Highlighted = false;
                        });
                button.TouchUpOutside += (object sender, EventArgs e) => 
                    Array.ForEach(button.Subviews, view =>
                        {
                            if ((view as UIImageView) != null)
                                (view as UIImageView).Highlighted = false;
                            else if ((view as UILabel) != null)
                                (view as UILabel).Highlighted = false;
                        });
                button.TouchDragOutside += (object sender, EventArgs e) => 
                    Array.ForEach(button.Subviews, view =>
                        {
                            if ((view as UIImageView) != null)
                                (view as UIImageView).Highlighted = false;
                            else if ((view as UILabel) != null)
                                (view as UILabel).Highlighted = false;
                        });
                button.TouchCancel += (object sender, EventArgs e) => 
                    Array.ForEach(button.Subviews, view =>
                        {
                            if ((view as UIImageView) != null)
                                (view as UIImageView).Highlighted = false;
                            else if ((view as UILabel) != null)
                                (view as UILabel).Highlighted = false;
                        });

                button.Highlighted = false;

                return button;
            }
        }
    }
}

