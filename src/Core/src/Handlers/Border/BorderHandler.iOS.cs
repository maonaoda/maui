using System;
using UIKit;
using PlatformView = UIKit.UIView;

namespace Microsoft.Maui.Handlers
{
	public partial class BorderHandler : ViewHandler<IBorderView, ContentView>
	{
		protected override ContentView CreatePlatformView()
		{
			_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} must be set to create a {nameof(ContentView)}");
			_ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} cannot be null");

			return new ContentView
			{
				CrossPlatformLayout = VirtualView
			};
		}

		protected override void DisconnectHandler(ContentView platformView)
		{
			base.DisconnectHandler(platformView);

			platformView.ClearSubviews();
		}

		public override void SetVirtualView(IView view)
		{
			base.SetVirtualView(view);

			_ = PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");
			_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");

			PlatformView.View = VirtualView;
			PlatformView.CrossPlatformLayout = VirtualView;
		}

		static partial void UpdateContent(IBorderHandler handler)
		{
			_ = handler.PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");
			_ = handler.VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");
			_ = handler.MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

			// Cleanup the old view when reused
			var platformView = handler.PlatformView;
			platformView.ClearSubviews();

			if (handler.VirtualView.PresentedContent is IView content)
			{
				var platformContent = content.ToPlatform(handler.MauiContext);

				// If the content is a UIScrollView, we need a container to handle masks and clip shapes effectively.
				if (platformContent is UIScrollView)
				{
					var containerView = new UIView
					{
						BackgroundColor = UIColor.Clear,
						TranslatesAutoresizingMaskIntoConstraints = false 
					};

					platformContent.TranslatesAutoresizingMaskIntoConstraints = false;

					containerView.Tag = ContentView.ContentTag;
					containerView.AddSubview(platformContent);
					platformView.AddSubview(containerView);

					var padding = handler.VirtualView.Padding;
					var strokeThickness = handler.VirtualView.StrokeThickness;
					NSLayoutConstraint.ActivateConstraints(new[]
			  		{
						containerView.TopAnchor.ConstraintEqualTo(platformView.TopAnchor, (nfloat)padding.Top + (nfloat)strokeThickness),
						containerView.LeadingAnchor.ConstraintEqualTo(platformView.LeadingAnchor, (nfloat)padding.Left + (nfloat)strokeThickness),
						containerView.TrailingAnchor.ConstraintEqualTo(platformView.TrailingAnchor, -(nfloat)padding.Right - (nfloat)strokeThickness),
						containerView.BottomAnchor.ConstraintEqualTo(platformView.BottomAnchor, -(nfloat)padding.Bottom - (nfloat)strokeThickness),

						platformContent.TopAnchor.ConstraintEqualTo(containerView.TopAnchor),
						platformContent.LeadingAnchor.ConstraintEqualTo(containerView.LeadingAnchor),
						platformContent.TrailingAnchor.ConstraintEqualTo(containerView.TrailingAnchor),
						platformContent.BottomAnchor.ConstraintEqualTo(containerView.BottomAnchor)
					});
				}
				else
				{
					platformContent.Tag = ContentView.ContentTag;
					platformView.AddSubview(platformContent);
				}
			}
		}
	}
}