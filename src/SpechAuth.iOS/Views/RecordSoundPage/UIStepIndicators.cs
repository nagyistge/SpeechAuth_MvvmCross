using System;
using UIKit;
using System.Collections.Generic;

namespace SpechAuth.iOS.Views.RecordSoundPage
{
    public class UIStepIndicators : UIView
	{
        public UIStepIndicators()
        {
            
        }

        public void AddIndicator()
        {
            var indicator = new UIImageView (UIImage.FromFile ("Images/IndicatorStroke.png")).WithFrame (4 + (4 + 28) * Subviews.Length, 0, 28, 28);
            AddSubview (indicator);
        }

        public void FillIndicator(int index)
        {
            if (index < Subviews.Length)
                (Subviews[index] as UIImageView).Image = UIImage.FromFile("Images/IndicatorFill.png");
        }

        public void StrokeIndicator(int index)
        {
            if (index < Subviews.Length)
                (Subviews[index] as UIImageView).Image = UIImage.FromFile("Images/IndicatorStroke.png");
        }
	}

}

