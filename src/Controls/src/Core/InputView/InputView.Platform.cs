using System;

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
#endif
	}
}
