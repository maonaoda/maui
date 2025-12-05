using System;
using System.Threading;

namespace Microsoft.Maui.Controls
{
	public partial class InputView
	{
#if ANDROID || IOS
		internal static void MapIsFocused(IViewHandler handler, IView view)
		{
			var manager = handler?.GetService<HideSoftInputOnTappedChangedManager>();
			if (manager == null)
			{
				return;
			}

			switch (view)
			{
				case InputView iv:
					manager.UpdateFocusForView(iv);
					break;
#if IOS
				case DatePicker datePicker:
					manager.UpdateFocusForView(datePicker);
					break;
				case TimePicker timePicker:
					manager.UpdateFocusForView(timePicker);
					break;
				case Picker picker:
					manager.UpdateFocusForView(picker);
					break;
#endif
			}
		}

		internal static void MapIsVisible(IViewHandler handler, IView view)
		{
			if (view is not InputView inputView || handler?.PlatformView == null)
			{
				return;
			}

			// Prevent input queuing when InputView is hidden
			// Dismiss soft keyboard on Android/iOS to stop background input processing
			if (!inputView.IsVisible && inputView.IsSoftInputShowing())
			{
				inputView.HideSoftInputAsync(CancellationToken.None);
			}
		}
#endif
	}
}
