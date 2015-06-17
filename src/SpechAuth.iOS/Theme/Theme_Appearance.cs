using System;
using System.Drawing;
using UIKit;
using CoreGraphics;

namespace SpeechAuth.Views
{
	public static partial class Theme
	{
		public static void InitAppearance ()//lock
		{
            UINavigationBar.Appearance.BarTintColor = Theme.Color.NavBarColor;
            UINavigationBar.Appearance.TintColor = Theme.Color.NavBarTintColor;
            UINavigationBar.Appearance.BackgroundColor = Theme.Color.NavBarColor;

			var attributes = new UITextAttributes ();
            attributes.TextColor = UIColor.White;
            attributes.Font = UIFont.BoldSystemFontOfSize(18);
			attributes.TextShadowColor = UIColor.FromWhiteAlpha (0, 0.12f);
            
            UINavigationBar.Appearance.SetTitleTextAttributes (attributes);
            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);			
		}
	}
}

