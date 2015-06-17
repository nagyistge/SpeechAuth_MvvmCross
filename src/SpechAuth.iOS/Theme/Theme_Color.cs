using UIKit;

namespace SpeechAuth.Views
{
	public static partial class Theme
	{
		public static class Color
		{
            public static UIColor NavBarColor
            {
                get { return UIColor.Black; }
            }

            public static UIColor NavBarTintColor
            {
                get { return UIColor.White; }
            }

            public static UIColor MainBackgroundColor
            {
                get { return UIColor.FromRGB(74, 74, 74); }
            }

            public static UIColor MainBlueColor
            {
                get { return UIColor.FromRGB(74, 144, 226); }
            }

            public static UIColor LightBlueColor
            {
                get { return UIColor.FromRGB(104, 174, 226); }
            }
		}
	}
}