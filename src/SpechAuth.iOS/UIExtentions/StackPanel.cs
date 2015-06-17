using System;
using System.Linq;
using UIKit;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;

namespace System
{
    public class StackPanel : UIView
    {
		private Dictionary<int, float> _margins = new Dictionary<int, float>();
		private StackPanelOrientation _orientation;
		private StackPanelAligment _panelAligment;
		private UIView _backgroundView;
			
		public event EventHandler<EventArgs> Resized;
			
		public StackPanel(IntPtr handle)
			: base(handle)
		{
			LayoutGrowsOnly = true;	
		}
			
		public StackPanel()
			: base ()
		{
			_orientation = StackPanelOrientation.Vertical;
			_panelAligment = StackPanelAligment.LeftTop;
			LayoutGrowsOnly = true;	
		} 
			
		public StackPanel(StackPanelOrientation orientation)
			: base ()
		{
			_orientation = orientation;
			LayoutGrowsOnly = true;	
		}
			
		public StackPanel (StackPanelAligment aligment, StackPanelOrientation orientation)
			: this (orientation)
		{
			_panelAligment = aligment;
		}
			
		public bool LayoutGrowsOnly { get; set; }
			
		private UIEdgeInsets _contentInsets = UIEdgeInsets.Zero;
		public UIEdgeInsets ContentInsets 
		{ 
			get { return _contentInsets; }
			set 
			{ 
				_contentInsets = value;
				SetNeedsLayout();
			}
		}
			
		public float StackPadding { get; set; }
			
		public override void LayoutSubviews()
		{
			LayoutSubviewsCore();
		}
			
		public UIView BackgroundView
		{
			get { return _backgroundView; }
			set
			{ 
				if (_backgroundView == value)
					return;
				if (_backgroundView != null)
					_backgroundView.RemoveFromSuperview();
				_backgroundView = value;
				if (_backgroundView != null)
				{
					_backgroundView.Frame = Bounds;
					AddSubview(_backgroundView);
				}
			}
		}
			
		private void LayoutSubviewsCore()
		{
			if (_orientation == StackPanelOrientation.Vertical)
				LayoutSubviewsVerical();
			else
				LayoutSubviewsHorizontal();
		}
			
		private void LayoutSubviewsVerical()
		{
			float top = (float)_contentInsets.Top;
			bool first = true;
				
			foreach (var view in Subviews)
			{
				if (view == _backgroundView)
					continue;
				if (view.Hidden)
					continue;
					
				if (first)
					first = false;
				else
					top += StackPadding;
					
				var margin = GetMargin(view);
				var delta = margin + view.Bounds.Height;
					
					
				if(delta >= 0 || !LayoutGrowsOnly )
				{
					view.Frame = new CGRect(view.Frame.Left+_contentInsets.Left, margin + top, view.Bounds.Width, view.Bounds.Height);
                    top += (float)delta;
				}
				else
				{
					view.Frame = new CGRect(view.Frame.Left+_contentInsets.Left, margin + top, view.Bounds.Width, view.Bounds.Height);
				}
			}
            top+=(float)_contentInsets.Bottom;
            ApplyStackAligment ((float)Frame.Width, Math.Max (0, top));
		}
			
		private void LayoutSubviewsHorizontal()
		{
			float left = (float)_contentInsets.Left;
			bool first = true;
				
			foreach (var view in Subviews)
			{
				if (view == _backgroundView)
					continue;
				if (view.Hidden)		
					continue;
					
					
				if (first)		
					first = false;
				else
					left += StackPadding;
					
				var margin = GetMargin(view);
				var delta = margin + view.Bounds.Width;
				view.Frame = new CGRect(margin + left, view.Frame.Top + _contentInsets.Top, view.Bounds.Width, view.Bounds.Height);
                left += (float)delta;
			}
				
            left+=(float)_contentInsets.Right;
            ApplyStackAligment (Math.Max (0, left), (float)Frame.Height);
		}
			
		private void ApplyStackAligment (float left, float top)
		{
				
			CGRect expandedFrame;
				
			switch (_panelAligment) {
			case StackPanelAligment.LeftTop:
				expandedFrame = new CGRect (Frame.Left, Frame.Top, Math.Max (0, left), Math.Max (0, top));
				break;
			case StackPanelAligment.RightTop:
				expandedFrame = new CGRect (Frame.Left + Frame.Width - Math.Max (0, left), Frame.Top, Math.Max (0, left), Math.Max (0, top));
				break;
			case StackPanelAligment.LeftBottom:
				expandedFrame = new CGRect (Frame.Left, Frame.Top + Frame.Height - Math.Max (0, top), Math.Max (0, left), Math.Max (0, top));
				break;
			case StackPanelAligment.RightBottom:
				expandedFrame = new CGRect (Frame.Left + Frame.Width - Math.Max (0, left), Frame.Top + Frame.Height - Math.Max (0, top), Math.Max (0, left), Math.Max (0, top));
				break;
			case StackPanelAligment.Center:
				expandedFrame = new CGRect (Frame.Left + (Frame.Width - Math.Max (0, left)) / 2, Frame.Top + (Frame.Height - Math.Max (0, top)) / 2, Math.Max (0, left), Math.Max (0, top));
				break;
			default:
				expandedFrame = new CGRect (Frame.Left, Frame.Top, Math.Max (0, left), Math.Max (0, top));
				break;
			}
				
			if (Frame != expandedFrame) 
			{
				Frame = expandedFrame;
				RaiseResizedEvent();
				if (_backgroundView != null)
					_backgroundView.Frame = Bounds;
			}
		}
			
		public override bool Hidden 
		{
			get 
			{
				return base.Hidden;
			}
			set 
			{
				WillChangeValue("hidden");
				base.Hidden = value;
				DidChangeValue("hidden");
			}
		}

		private void RaiseResizedEvent ()
		{
			var handler = Resized;
			if (handler != null)
				handler(this, new EventArgs());
		}
			
		private float GetMargin(UIView view)
		{
            if (_orientation == StackPanelOrientation.Vertical)
                return _margins.GetOrAdd(view.GetHashCode(), (float)view.Frame.Top);
			else
                return _margins.GetOrAdd(view.GetHashCode(), (float)view.Frame.Left);
		}
    }
}
