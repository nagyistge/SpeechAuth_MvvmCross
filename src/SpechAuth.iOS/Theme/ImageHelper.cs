using System;
using Foundation;
using UIKit;
using System.Collections.Generic;
using System.Drawing;
using CoreGraphics;

namespace SpeechAuth.Views
{
	public static class ImageHelper
	{
        private static readonly Dictionary<string, UIImage> _imageCache = new Dictionary<string, UIImage>();

		public static UIImage LoadResizableImage (string path, UIEdgeInsets insets)
		{
            if (string.IsNullOrEmpty(path)) 
                return null;
//			return _imageCache.GetOrAdd (path, x =>
//			{
				return UIImage.FromFile(path).CreateResizableImage(insets);
//			});
		}

        public static UIImage WideImageWithFixedCenter(this UIImage image, float newWidth, float exArea1LeftGap, float exArea2RightGap)
        {
            return WideImageWithFixedCenter(image, newWidth, exArea1LeftGap, exArea1LeftGap + 1, (float)(image.Size.Width - exArea2RightGap), (float)(image.Size.Width - exArea2RightGap + 1));
        }

        private static UIImage WideImageWithFixedCenter(UIImage image, float newWidth, float from1, float to1, float from2, float to2)
        {
            var originalWidth = image.Size.Width;
            if (originalWidth == newWidth)
                return image;
            var tiledAreaWidth = (newWidth - originalWidth)/2f;

            UIGraphics.BeginImageContextWithOptions(new CGSize(originalWidth + tiledAreaWidth, (float)image.Size.Height), false, image.CurrentScale);

            var firstResizable = image.CreateResizableImage(new UIEdgeInsets(0, from1, 0, originalWidth - to1), UIImageResizingMode.Tile);
            firstResizable.Draw(new CGRect(0, 0, originalWidth + tiledAreaWidth, (float)image.Size.Height));

            var leftPart = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            UIGraphics.BeginImageContextWithOptions(new SizeF(newWidth, (float)image.Size.Height), false, image.CurrentScale);
            var secondResizable = leftPart.CreateResizableImage( new UIEdgeInsets(0, from2 + tiledAreaWidth, 0, originalWidth - to2), UIImageResizingMode.Tile);
            secondResizable.Draw(new CGRect(0, 0, newWidth, (float)image.Size.Height));

            var fullImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return fullImage;
        }
	}
}

