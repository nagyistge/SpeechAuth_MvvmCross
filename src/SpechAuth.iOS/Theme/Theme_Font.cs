using UIKit;

namespace SpeechAuth.Views
{
	public static partial class Theme
	{
		public static class Font
		{
            public static UIFont HelveticaNeue_Regular(float size)
            {
                return UIFont.FromName("HelveticaNeue", size);
            }

            public static UIFont HelveticaNeue_Light(float size)
            {
                return UIFont.FromName("HelveticaNeue-Light", size);
            }

            public static UIFont HelveticaNeue_Medium(float size)
            {
                return UIFont.FromName("HelveticaNeue-Medium", size);
            }

            public static UIFont HelveticaNeue_Bold(float size)
            {
                return UIFont.FromName("HelveticaNeue-Bold", size);
            }
		}
	}
}

