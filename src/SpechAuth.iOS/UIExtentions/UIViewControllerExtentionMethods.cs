using System;
using UIKit;
using Foundation;
using System.Drawing;
using System.Collections.Generic;
using CoreGraphics;

namespace System
{
	public static class UIViewControllerExtentionMethods
	{

		private class ModalViewControllerPresenter
		{
			private string _completeButtonText;
			private string _cancelButtonText;
			private Action _completeAction;
			private Action _cancelAction;
			private UIViewController _childController;
			private UINavigationController _navigationController;
			private UIBarButtonItem _completeButton;
			private UIBarButtonItem _cancelButton;

			public void PresentViewControllerAnimated(UIViewController parent, UIViewController controller, string completeButton = null, Action completeAction = null, string cancelButton = null, Action cancelAction = null, Action animationCompleted = null)
			{
				if (completeButton == null && cancelButton == null)
					completeButton = "Готово";
				if (completeButton == string.Empty)
					completeButton = null;
				_completeButtonText = completeButton;
				_cancelButtonText = cancelButton;
				_childController = controller;
				_completeAction = completeAction;
				_cancelAction = cancelAction;
				_navigationController = new UINavigationController();
				_navigationController.AddChildViewController(_childController);
                _navigationController.NavigationBar.Translucent = false;
            
				if (_completeButtonText != null)
				{
					_completeButton = new UIBarButtonItem(_completeButtonText, UIBarButtonItemStyle.Plain, CompleteButtonClicked);
					_childController.NavigationItem.SetRightBarButtonItem(_completeButton, true);
				} else 
				{
					_childController.NavigationItem.RightBarButtonItem = null;
				}
				if (_cancelButtonText != null)
				{
					_cancelButton = new UIBarButtonItem(_cancelButtonText, UIBarButtonItemStyle.Plain, CancelButtonClicked);
					_childController.NavigationItem.SetLeftBarButtonItem(_cancelButton, true);
				}
				parent.PresentViewController(_navigationController, true, animationCompleted);
			}

			public void PresentViewController(UIViewController parent, UIViewController controller, string completeButton = null, Action completeAction = null, string cancelButton = null, Action cancelAction = null)
			{
				if (completeButton == null && cancelButton == null)
					completeButton = "Готово";
				if (completeButton == string.Empty)
					completeButton = null;
				_completeButtonText = completeButton;
				_cancelButtonText = cancelButton;
				_childController = controller;
				_completeAction = completeAction;
				_cancelAction = cancelAction;
				_navigationController = new UINavigationController();
				_navigationController.AddChildViewController(_childController);
                _navigationController.NavigationBar.Translucent = false;

				if (_completeButtonText != null) 
				{
					_completeButton = new UIBarButtonItem (_completeButtonText, UIBarButtonItemStyle.Plain, CompleteButtonClicked);
					_childController.NavigationItem.SetRightBarButtonItem (_completeButton, true);
				} else 
				{
					_childController.NavigationItem.RightBarButtonItem = null;
				}
				if (_cancelButtonText != null)
				{
					_cancelButton = new UIBarButtonItem(_cancelButtonText, UIBarButtonItemStyle.Plain, CancelButtonClicked);
					_childController.NavigationItem.SetLeftBarButtonItem(_cancelButton, true);
				}
				parent.PresentViewController(_navigationController, false, null);
			}

			private void CompleteButtonClicked(object sender, System.EventArgs args)
			{
				_completeButton.Clicked -= CompleteButtonClicked;
				if (_cancelButton != null)
					_cancelButton.Clicked -= CancelButtonClicked;
				_childController.DismissViewController(true, OnComplete);
			}

			private void OnComplete()
			{
				if (_completeAction != null)
					_completeAction();
//				_childController.UnbindFrame();
				_childController.RemoveFromParentViewController();
				_childController.Dispose();
			}

			private void CancelButtonClicked(object sender, System.EventArgs args)
			{
				_cancelButton.Clicked -= CancelButtonClicked;
				if (_completeButton != null)
					_completeButton.Clicked -= CompleteButtonClicked;
				_childController.DismissViewController(true, OnCancel);
			}

			private void OnCancel()
			{
				if (_cancelAction != null)
					_cancelAction();
//				_childController.UnbindFrame();
				_childController.RemoveFromParentViewController();
				_childController.Dispose();
			}
		}

		public static void ShowModalViewControllerAnimated(this UIViewController self, UIViewController controller, string completeButton = null, Action completeAction = null, string cancelButton = null, Action cancelAction = null, Action animationCompleted = null)
		{
			new ModalViewControllerPresenter().PresentViewControllerAnimated(self, controller, completeButton, completeAction, cancelButton, cancelAction, animationCompleted);
		}

		public static void ShowModalViewController(this UIViewController self, UIViewController controller, string completeButton = null, Action completeAction = null, string cancelButton = null, Action cancelAction = null)
		{
			new ModalViewControllerPresenter().PresentViewController(self, controller, completeButton, completeAction, cancelButton, cancelAction);
		}

		public static UIView FindFirstResponder (this UIView view)
		{
			if (view.IsFirstResponder)
				return view;
			foreach (UIView subView in view.Subviews) {
				var firstResponder = subView.FindFirstResponder();
				if (firstResponder != null)
					return firstResponder;
			}
			return null;
		}

        public static T WithSubviews<T>(this T self, params UIView[] subviews)
			where T: UIView
		{
			self.AddSubviews(subviews);
			return self;
		}

		public static T WithTune<T>(this T self, Action<T> tune)
			where T: UIView
		{
			if (tune != null)
				tune(self);
			return self;
		}

		public static T WithFrame<T>(this T self, CGRect frame)
			where T: UIView
		{
			self.Frame = frame;
			return self;
		}

		public static T WithFrame<T>(this T self, float left, float top, float width, float height)
			where T: UIView
		{
			self.Frame = new CGRect(left, top, width, height);
			return self;
		}

        public static T WithFrame<T>(this T self, nfloat left, nfloat top, nfloat width, nfloat height)
            where T: UIView
        {
            self.Frame = new CGRect(left, top, width, height);
            return self;
        }

		public static T WithBackground<T>(this T self, UIColor backgroundColor)
			where T: UIView
		{
			self.BackgroundColor = backgroundColor;
			return self;
		}

		public static UILabel WithLineBreakMode(this UILabel self, UILineBreakMode lineBreakMode)
		{
			self.LineBreakMode = lineBreakMode;
			return self;
		}

		public static UILabel WithTextAlignment(this UILabel self, UITextAlignment textAlignment)
		{
			self.TextAlignment = textAlignment;
			return self;
		}

		public static UILabel WithLinesNumber(this UILabel self, int linesNumber)
		{
			self.Lines = linesNumber;
			return self;
		}

		public static List<UIView> AllSubviews(this UIView self, params Type[] types)
		{
			var result = new List<UIView>();
			CollectSubviews (self, result, types);
			return result;
		}

		private static void CollectSubviews(UIView parent, List<UIView> views, Type[] types)
		{
			foreach (var view in parent.Subviews) 
			{
				var valid = true;
				if (types.Length > 0)
				{
					valid = false;
					for (int i = 0; i < types.Length; i++)
					{
						if (types[i].IsAssignableFrom(view.GetType()))
						{
							valid = true;
							break;
						}
					}
				}
				if (valid)
					views.Add(view);
				CollectSubviews (view, views, types);
			}
		}

        public static UILabel WithUnderlinedText (this UILabel self, NSUnderlineStyle style)
		{
			self.AttributedText = new NSAttributedString(self.Text, new UIStringAttributes
			                                             {
				UnderlineStyle = style,
				Font = self.Font,
				Shadow = new NSShadow { ShadowColor = UIColor.White, ShadowOffset = new SizeF(0, 1) },
				ForegroundColor = self.TextColor
			});
			
			return self;
		}
	}
}

